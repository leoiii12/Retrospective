import { Component, Input, OnInit } from '@angular/core';
import { DndDropEvent, DropEffect } from 'ngx-drag-drop';

@Component({
  selector: 'app-board-column',
  templateUrl: './board-column.component.html',
  styleUrls: ['./board-column.component.scss']
})
export class BoardColumnComponent implements OnInit {

  @Input()
  public items: DraggableBoardItem[];

  @Input()
  public title: string;

  @Input()
  public isDisplayingContent: boolean;

  private currentDraggableEvent: DragEvent;

  constructor() {
  }

  ngOnInit() {
    if (this.items == null) this.items = [];
    if (this.title == null) this.title = 'Board Column';
    if (this.isDisplayingContent) this.isDisplayingContent = true;
  }

  public onDragStart(event: DragEvent) {
    this.currentDraggableEvent = event;
  }

  public onDragEnd(event: DragEvent) {
    this.currentDraggableEvent = event;
  }

  public onDragged(item: any, list: any[], effect: DropEffect) {
    const index = list.indexOf(item);
    list.splice(index, 1);
  }

  public onDrop(event: DndDropEvent, list?: any[]) {
    let index = event.index;

    if (typeof index === 'undefined') {
      index = list.length;
    }

    list.splice(index, 0, event.data);
  }

  public onClickSuggestions() {
    this.items.push({title: '4-word title', content: '', effectAllowed: 'move'});
  }

}
