import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateCardComponent } from './create-card/create-card.component';
import { AdminComponent} from "./admin.component";
import { CreatePlanetComponent } from "./create-planet/create-planet.component";

const routes: Routes = [
  {path : 'adminview', component : AdminComponent, children : [
      {path : 'create-card', component: CreateCardComponent},
      {path : 'create-planet', component: CreatePlanetComponent},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
