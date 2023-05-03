import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from "@angular/router";
import { PlayerRoutingModule } from './player-routing.module';
import { MatGridListModule } from "@angular/material/grid-list";
import { HttpClientModule } from "@angular/common/http";
import { DeckComponent } from './deck/deck.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
@NgModule({
  declarations: [
    DeckComponent
  ],
  imports: [
    CommonModule,
    PlayerRoutingModule,
    MatGridListModule,
    HttpClientModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,

  ],

})
export class PlayerModule { }
