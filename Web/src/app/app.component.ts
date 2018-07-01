import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfigService } from '../services/config.service';
import { filter, map, takeUntil } from 'rxjs/operators';
import { Observable, Subject, zip } from 'rxjs';
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
          this.boards.push({boardId: z[0].split('-').join(' '), password: z[1]});
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
