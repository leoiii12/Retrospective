import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BoardComponent } from './board/board.component';
import { HomeComponent } from './home/home.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DndModule } from 'ngx-drag-drop';
import { NotFoundComponent } from './not-found/not-found.component';
import { InlineEditorModule } from '@fradev/ngx-inline-editor';
import { FormsModule } from '@angular/forms';
import { BoardColumnComponent } from './board-column/board-column.component';

@NgModule({
  declarations: [
    AppComponent,
    BoardComponent,
    HomeComponent,
    NotFoundComponent,
    BoardColumnComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    NgbModule.forRoot(),
    DndModule,
    InlineEditorModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
