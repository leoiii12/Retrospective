import { Injectable } from '@angular/core';
import * as PusherJS from 'pusher-js';
import { Channel, Pusher } from 'pusher-js';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { BoardItemType } from '../types/board-item-type.enum';
import { BoardItem } from '../types/board-item.interface';

@Injectable({
  providedIn: 'root'
})
export class BoardService {

  public endpoint: string = 'https://retrospective-api.azurewebsites.net/api/';

  private pusher: Pusher;
  private channel: Channel;

  private boardIdSource: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  public boardId: Observable<string> = this.boardIdSource.asObservable();

  private passwordSource: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  public password: Observable<string> = this.passwordSource.asObservable();

  private clientIdSource: BehaviorSubject<string> = new BehaviorSubject<string>(null);
  public clientId: Observable<string> = this.clientIdSource.asObservable();

  private itemsSource: BehaviorSubject<BoardItem[]> = new BehaviorSubject<BoardItem[]>([]);
  public items: Observable<BoardItem[]> = this.itemsSource.asObservable();

  private itemChange: Subject<BoardItem> = new Subject<BoardItem>();

  constructor(private http: HttpClient) {
    this.pusher = new PusherJS('023cc24c6e0f818d4e15', {
      cluster: 'ap1',
      encrypted: true
    });

    this.itemChange.subscribe(changedItem => {
      const items = this.itemsSource.getValue();
      const item = items.find(i => i.id === changedItem.id);

      if (item) {
        item.title = changedItem.title;
        item.content = changedItem.content;
        item.type = changedItem.type;
        item.dateTime = changedItem.dateTime;

        this.itemsSource.next(items);
      } else {
        items.push(changedItem);

        this.itemsSource.next(items);
      }
    });
  }

  public createBoard(boardId: string): Observable<{ boardId: string, password: string }> {
    return this.http
      .post<any>(this.endpoint + 'CreateBoard', {boardId: boardId})
      .pipe(
        map(output => {
          if (output.success) {
            return <{ boardId: string, password: string }> output.data.board;
          } else {
            throw new Error(output.message);
          }
        })
      );
  }

  public joinBoard(boardId: string, password: string) {
    this.boardIdSource.next(boardId.trim().split(' ').join('-'));
    this.passwordSource.next(password);

    return this.http
      .post<any>(this.endpoint + 'JoinBoard', {
        boardId: this.boardIdSource.getValue(),
        password: this.passwordSource.getValue()
      })
      .pipe(
        map(output => {
          if (output.success) {
            const data = output.data;

            this.clientIdSource.next(data.clientId);

            this.channel = this.pusher.subscribe(data.channel);
            this.channel.bind('BoardItem_Create', (eventData) => {
              this.onItemCreate(eventData);
            });
            this.channel.bind('BoardItem_Update', (eventData) => {
              this.onItemUpdate(eventData);
            });
            this.channel.bind('Sync_AskForSync', (eventData) => {
              this.onAskForSync(eventData);
            });

            setTimeout(() => {
              this.askForSync();
            }, 1000);

            return output;
          } else {
            throw new Error(output.message);
          }
        })
      );
  }

  public createItem(type: BoardItemType) {
    return this.http
      .post(this.endpoint + 'CreateBoardItem', {
        boardId: this.boardIdSource.getValue(),
        password: this.passwordSource.getValue(),
        type: type
      });
  }

  public updateItem(id: string, title: string, content: string, type: BoardItemType) {
    return this.http
      .post<any>(this.endpoint + 'UpdateBoardItem', {
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
            throw new Error(output.message);
          }
        })
      );
  }

  public resetAll() {
    if (this.channel) {
      this.pusher.unsubscribe(this.channel.name);
      this.channel = null;
    }

    this.boardIdSource.next(null);
    this.passwordSource.next(null);
    this.itemsSource.next([]);
  }

  public askForSync() {
    if (!this.channel) return;

    this.http
      .post<any>(this.endpoint + 'AskForSync', {
        boardId: this.boardIdSource.getValue(),
        password: this.passwordSource.getValue(),
        clientId: this.clientIdSource.getValue()
      })
      .subscribe(() => {
        let hasReceived = false;

        this.channel.bind('Sync_GiveSync', (eventData) => {
          const receivedItems = eventData.BoardItems;
          if (!receivedItems)
            return;

          for (const item of receivedItems) {
            this.itemChange.next({id: item.Id, title: item.Title, content: item.Content, type: item.Type, dateTime: item.DateTime});
          }

          hasReceived = true;
        });

        // Wait for 5 seconds
        setTimeout(() => {
          if (hasReceived) {
            this.channel.unbind('Board_GiveSync');
          } else {
            this.noGiveSync();
          }
        }, 5000);
      });
  }

  public noGiveSync() {
    this.http
      .post<any>(this.endpoint + 'NoGiveSync', {
        boardId: this.boardIdSource.getValue(),
        password: this.passwordSource.getValue()
      })
      .subscribe();
  }

  private onItemCreate(eventData) {
    this.itemChange.next({id: eventData.Id, title: '', content: '', type: eventData.Type, dateTime: null});
  }

  private onItemUpdate(eventData) {
    this.itemChange.next({
      id: eventData.Id,
      title: eventData.Title,
      content: eventData.Content,
      type: eventData.Type,
      dateTime: eventData.DateTime
    });
  }

  private hasSynced(): boolean {
    return true;
  }

  private onAskForSync(eventData) {
    const notMyself = eventData.AskForSyncClientId !== this.clientIdSource.getValue();

    if (
      (eventData.HasMaster === true && notMyself) ||
      (eventData.HasMaster === false && this.hasSynced())) {

      this.http
        .post<any>(this.endpoint + 'GiveSync', {
          boardId: this.boardIdSource.getValue(),
          password: this.passwordSource.getValue(),
          askForSyncClientId: eventData.AskForSyncClientId,
          boardItems: this.itemsSource.getValue()
        })
        .subscribe();
    }
  }

}
