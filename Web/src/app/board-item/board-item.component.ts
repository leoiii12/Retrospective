import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, flatMap, takeUntil } from 'rxjs/operators';
import { BoardService } from '../../services/board.service';
import { BoardItem } from '../../types/board-item.interface';

@Component({
  selector: 'app-board-item',
  templateUrl: './board-item.component.html',
  styleUrls: ['./board-item.component.scss']
})
export class BoardItemComponent implements OnInit, OnDestroy {

  @Input() public item: BoardItem;
  @Input() public isDisplayingContent: boolean;

  private itemChanged: Subject<BoardItem> = new Subject<BoardItem>();
  private unsubscribe = new Subject<void>();

  constructor(private boardService: BoardService) {
  }

  ngOnInit() {
    this.itemChanged
      .pipe(
        takeUntil(this.unsubscribe),
        debounceTime(1000),
        flatMap((item: BoardItem) => this.boardService.updateItem(item.id, item.title, item.content, item.type))
      )
      .subscribe(item => {
        console.log(item);
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  public onTitleChange(event) {
    this.item.title = event;

    this.itemChanged.next(this.item);
  }

  public onContentChange(event) {
    this.item.content = event;

    this.itemChanged.next(this.item);
  }

}
