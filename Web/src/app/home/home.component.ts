import { Component, OnInit } from '@angular/core';
import { BoardService } from '../../services/board.service';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public boardId: string = '';
  public password: string = '';

  constructor(private boardService: BoardService,
              private router: Router,
              private spinner: NgxSpinnerService) {
  }

  ngOnInit() {
  }

  public onClickCreate() {
    if (this.boardId.length === 0) {
      alert('Please input everything.');
      return;
    }

    this.spinner.show();

    this.boardService
      .create(this.boardId)
      .pipe(
        finalize(() => this.spinner.hide())
      )
      .subscribe(data => {
        this.navigateToBoard(this.boardId, data.password);
      }, error => {
        console.log(error);
      });
  }

  public onClickJoin() {
    if (this.boardId.length === 0 || this.password.length === 0) {
      alert('Please input everything.');
      return;
    }

    this.navigateToBoard(this.boardId, this.password);
  }

  private navigateToBoard(boardId: string, password: string) {
    this.router.navigate(['/board', boardId.trim().split(' ').join('-'), password]);
  }

}
