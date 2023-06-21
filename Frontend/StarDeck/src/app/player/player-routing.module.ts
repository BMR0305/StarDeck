import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DeckComponent } from './deck/deck.component';
import { GameComponent } from './game/game.component';
import { MatchmakingComponent } from './matchmaking/matchmaking.component';
import { PlayerComponent } from './player.component';
import { StartComponent} from "./start/start.component";

const routes: Routes = [
  {path : 'playerview', component : PlayerComponent, children : [
      {path : 'deck', component: DeckComponent},
      {path : 'match', component: MatchmakingComponent},
      {path : 'start', component: StartComponent},
    ]
  },
  {path : 'game', component : GameComponent},
];



@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlayerRoutingModule { }
