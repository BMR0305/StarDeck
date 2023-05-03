import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateCardComponent } from './create-card/create-card.component';
import { CreatePlanetComponent } from "./create-planet/create-planet.component";

const routes: Routes = [
  {path : 'create-card', component: CreateCardComponent},
  {path : 'create-planet', component: CreatePlanetComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
