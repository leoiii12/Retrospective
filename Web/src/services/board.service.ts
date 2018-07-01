import { Injectable } from '@angular/core';
import * as PusherJS from 'pusher-js';
import { Channel, Pusher } from 'pusher-js';
import { HttpClient } from '@angular/common/http';
import { filter, map } from 'rxjs/operators';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { BoardItemType } from '../types/board-item-type.enum';
import { BoardItem } from '../types/board-item.interface';

@Injectable({
  providedIn: 'root'
})
export class BoardService {

  private pusher: Pusher;
  private channel: Channel;

  private boardIdSource: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  public boardId: Observable<string> = this.boardIdSource.asObservable();

  private passwordSource: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  public password: Observable<string> = this.passwordSource.asObservable();

  private itemsSource: BehaviorSubject<BoardItem[]> = new BehaviorSubject<BoardItem[]>([]);
  public items: Observable<BoardItem[]> = this.itemsSource.asObservable();

  constructor(private http: HttpClient) {
    this.pusher = new PusherJS('023cc24c6e0f818d4e15', {
      cluster: 'ap1',
      encrypted: true
    });
  }

  public create(boardId: string): Observable<{ boardId: string, password: string }> {
    return this.http
      .post<any>('https://retrospective-api.azurewebsites.net/api/CreateBoard', {boardId: boardId})
      .pipe(
        map(output => {
          if (output.success) {
            return <{ boardId: string, password: string }> output.data;
          } else {
            throw new Error(output.message);
          }
        })
      );
  }

  public init(boardId: string, password: string) {
    this.boardIdSource.next(boardId.trim().split(' ').join('-'));
    this.passwordSource.next(password);

    return this;
  }

  public join() {
    if (this.channel) this.leave();

    return this.http
      .post<any>('https://retrospective-api.azurewebsites.net/api/JoinBoard', {
        boardId: this.boardIdSource.getValue(),
        password: this.passwordSource.getValue()
      })
      .pipe(
        map(output => {
          if (output.success) {
            const data = output.data;

            this.channel = this.pusher.subscribe(data.channel);
            this.channel.bind('BoardItem-Create', (eventData) => {
              this.onItemCreate(eventData);
            });
            this.channel.bind('BoardItem-Update', (eventData) => {
              this.onItemUpdate(eventData);
            });

            return output;
          } else {
            throw new Error(output.message);
          }
        })
      );
  }

  public createItem(type: BoardItemType) {
    return this.http
      .post('https://retrospective-api.azurewebsites.net/api/CreateBoardItem', {
        boardId: this.boardIdSource.getValue(),
        password: this.passwordSource.getValue(),
        type: type
      });
  }

  public updateItem(id: string, title: string, content: string, type: BoardItemType) {
    return this.http
      .post<any>('https://retrospective-api.azurewebsites.net/api/UpdateBoardItem', {
        boardId: this.boardIdSource.getValue(),
        password: this.passwordSource.getValue(),
        id: id,
        title: title,
        content: content,
        type: type
      })
      .pipe(
        map(output => {
          if (output.success) {
            return output;
          } else {
            throw new Error();
          }
        })
      );
  }

  public leave() {
    if (this.channel) {
      this.pusher.unsubscribe(this.channel.name);
      this.channel = null;
    }

    this.boardIdSource.next(null);
    this.passwordSource.next(null);
    this.itemsSource.next([]);
  }

  private onItemCreate(eventData) {
    const items = this.itemsSource.getValue();
    items.push({id: eventData.Id, title: '', content: '', type: eventData.Type});

    this.itemsSource.next(items);
  }

  private onItemUpdate(eventData) {
    const items = this.itemsSource.getValue();
    const item = items.find(i => i.id === eventData.Id);

    item.title = eventData.Title;
    item.content = eventData.Content;
    item.type = eventData.Type;

    this.itemsSource.next(items);
  }

}
