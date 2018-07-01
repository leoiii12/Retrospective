import { Component, Input, OnInit } from '@angular/core';
import { DndDropEvent, DropEffect } from 'ngx-drag-drop';
import { BoardService } from '../../services/board.service';
import { BoardItem } from '../../types/board-item.interface';
import { BoardItemType } from '../../types/board-item-type.enum';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-board-column',
  templateUrl: './board-column.component.html',
  styleUrls: ['./board-column.component.scss']
})
export class BoardColumnComponent implements OnInit {

  @Input() public items: BoardItem[] = [];
  @Input() public type: BoardItemType = null;
  @Input() public isDisplayingContent: boolean = false;

  public pageControls = {
    title: ''
  };

  private currentDraggableEvent: DragEvent;

  constructor(private boardService: BoardService) {
  }

  ngOnInit() {
    if (this.items == null || this.type == null) throw new Error();
    if (this.isDisplayingContent) this.isDisplayingContent = true;

    switch (this.type) {
      case BoardItemType.Well:
        this.pageControls.title = 'Well';
        break;
      case BoardItemType.NotWell:
        this.pageControls.title = 'Not Well';
        break;
      case BoardItemType.Suggested:
        this.pageControls.title = 'Suggestions';
        break;
    }
  }

  public onDragStart(event: DragEvent) {
    this.currentDraggableEvent = event;
  }

  public onDragEnd(event: DragEvent) {
    this.currentDraggableEvent = event;
  }

  public onDragged(item: BoardItem, items: any[], effect: DropEffect) {
    if (!item) return;
    if (!items) return;

    const index = items.indexOf(item);

    items.splice(index, 1);
  }

  public onDrop(event: DndDropEvent, items: any[]) {
    if (!event) return;
    if (!items) return;

    const item = event.data;
    const index = event.index;

    items.splice(index, 0, event.data);

    this.boardService
      .updateItem(item.id, item.title, item.content, this.type)
      .subscribe();
  }

  public onClickTitle() {
    this.boardService
      .createItem(this.type)
      .pipe(
        take(1)
      )
      .subscribe();
  }

  public boardItemTrackByFn(item: BoardItem) {
    return item.id;
  }

}
