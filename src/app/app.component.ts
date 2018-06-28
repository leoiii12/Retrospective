import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfigService } from '../services/config.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

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

  private unsubscribe = new Subject<void>();

  constructor(private configService: ConfigService) {
  }

  ngOnInit() {
    this.configService.isDisplayingContent
      .pipe(
        takeUntil(this.unsubscribe)
      )
      .subscribe((isDisplayingContent) => {
        this.pageControls.isDisplayingContent = isDisplayingContent;
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
