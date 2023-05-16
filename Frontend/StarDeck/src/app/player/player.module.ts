import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from "@angular/router";
import { PlayerRoutingModule } from './player-routing.module';
import { MatGridListModule } from "@angular/material/grid-list";
import { HttpClientModule } from "@angular/common/http";
import { DeckComponent } from './deck/deck.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { GameComponent } from './game/game.component';
import { MatchmakingComponent } from './matchmaking/matchmaking.component';
import { StartComponent } from './start/start.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { PlayerComponent } from './player.component';
@NgModule({
  declarations: [
    DeckComponent,
    GameComponent,
    MatchmakingComponent,
    StartComponent,
    SidebarComponent,
    PlayerComponent
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
