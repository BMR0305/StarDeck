import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DeckComponent } from './deck/deck.component';
import { GameComponent } from './game/game.component';
import { MatchmakingComponent } from './matchmaking/matchmaking.component';

const routes: Routes = [
  {path : 'deck', component: DeckComponent},
  {path : 'game', component: GameComponent},
  {path : 'match', component: MatchmakingComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlayerRoutingModule { }
