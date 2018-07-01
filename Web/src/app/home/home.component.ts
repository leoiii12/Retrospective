import { Component, OnInit } from '@angular/core';
import { BoardService } from '../../services/board.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public boardId: string = '';
  public password: string = '';

  constructor(private boardService: BoardService,
              private router: Router) {
  }

  ngOnInit() {
  }

  public onClickCreate() {
    if (this.boardId.length === 0) {
      alert('Please input everything.');
      return;
    }

    this.boardService
      .create(this.boardId)
      .subscribe(data => {
        this.router.navigate(['/board', data.boardId, data.password]);
      }, error => {
        console.log(error);
      });
  }

  public onClickJoin() {
    if (this.boardId.length === 0 || this.password.length === 0) {
      alert('Please input everything.');
      return;
    }

    this.router.navigate(['/board', this.boardId, this.password]);
  }

}