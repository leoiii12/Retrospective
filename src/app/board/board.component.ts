import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfigService } from '../../services/config.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';


@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.scss']
})
export class BoardComponent implements OnInit, OnDestroy {

  public isDisplayingContent = false;

  public wellItems: DraggableBoardItem[] = [];
  public notWellItems: DraggableBoardItem[] = [];
  public suggestedItems: DraggableBoardItem[] = [];

  private unsubscribe = new Subject<void>();

  constructor(private configService: ConfigService) {
  }

  ngOnInit() {
    this.configService.isDisplayingContent
      .pipe(
        takeUntil(this.unsubscribe)
      )
      .subscribe((isDisplayingContent) => {
        this.isDisplayingContent = isDisplayingContent;
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

}
