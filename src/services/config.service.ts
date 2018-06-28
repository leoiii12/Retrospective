import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {

  private isDisplayingContentSource = new BehaviorSubject(true);
  public isDisplayingContent = this.isDisplayingContentSource.asObservable();

  constructor() {
  }

  public changeIsDisplayingContent(value) {
    this.isDisplayingContentSource.next(value);
  }

}
