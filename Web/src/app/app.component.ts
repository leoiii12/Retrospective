import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfigService } from '../services/config.service';
import { takeUntil } from 'rxjs/operators';
import { Subject, zip } from 'rxjs';
import { BoardService } from '../services/board.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {

  public pageControls = {
    isCollapsed: true,
    isDisplayingContent: true
  };

  public boards: { boardId: string, password: string }[] = [];

  private unsubscribe = new Subject<void>();

  constructor(private configService: ConfigService,
              private boardService: BoardService) {
  }

  ngOnInit() {
    this.configService.isDisplayingContent
      .pipe(
        takeUntil(this.unsubscribe)
      )
      .subscribe((isDisplayingContent) => {
        this.pageControls.isDisplayingContent = isDisplayingContent;
      });

    zip(this.boardService.boardId, this.boardService.password)
      .subscribe(z => {
        if (z[0] == null && z[1] == null) {
          this.boards = [];
        } else {
          const boardId = z[0];
          const password = z[1];

          if (this.boards.every(b => b.boardId !== boardId && b.password !== password)) {
            setTimeout(() => {
              this.boards.push({boardId: boardId, password: password});
            });
          }
        }
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  public onIsDisplayingContentChange() {
    this.configService.changeIsDisplayingContent(this.pageControls.isDisplayingContent);
  }

}
