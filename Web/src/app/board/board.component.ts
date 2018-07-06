import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfigService } from '../../services/config.service';
import { Subject } from 'rxjs';
import { flatMap, takeUntil } from 'rxjs/operators';
import { BoardService } from '../../services/board.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BoardItem } from '../../types/board-item.interface';
import { BoardItemType } from '../../types/board-item-type.enum';


@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.scss']
})
export class BoardComponent implements OnInit, OnDestroy {

  public BoardItemType = BoardItemType;

  public pageControls = {
    isDisplayingContent: false
  };

  public wellItems: BoardItem[] = [];
  public notWellItems: BoardItem[] = [];
  public suggestedItems: BoardItem[] = [];

  private unsubscribe = new Subject<void>();

  constructor(private activatedRoute: ActivatedRoute,
              private configService: ConfigService,
              private boardService: BoardService,
              private router: Router) {
  }

  ngOnInit() {
    this.configService.isDisplayingContent
      .pipe(
        takeUntil(this.unsubscribe)
      )
      .subscribe((isDisplayingContent) => {
        this.pageControls.isDisplayingContent = isDisplayingContent;
      });

    this.boardService.items
      .pipe(
        takeUntil(this.unsubscribe)
      )
      .subscribe((items: BoardItem[]) => {
        this.wellItems = [];
        this.notWellItems = [];
        this.suggestedItems = [];

        for (const item of items) {
          switch (item.type) {
            case 0:
              this.wellItems.push(item);
              break;
            case 1:
              this.notWellItems.push(item);
              break;
            case 2:
              this.suggestedItems.push(item);
              break;
          }
        }
      });

    this.activatedRoute
      .params
      .pipe(
        flatMap(params => this.boardService.joinBoard(params.boardId, params.password))
      )
      .subscribe(output => {
      }, error => {
        alert(error);
        this.router.navigate(['/home']);
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();

    this.boardService.resetAll();
  }

}
