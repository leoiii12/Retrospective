import { Component } from '@angular/core';
import { ConfigService } from '../services/config.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  public pageControls = {
    isCollapsed: true,
    isDisplayingContent: true
  };

  constructor(private configService: ConfigService) {
  }

  public onIsDisplayingContentChange() {
    this.configService.changeIsDisplayingContent(this.pageControls.isDisplayingContent);
  }

}
