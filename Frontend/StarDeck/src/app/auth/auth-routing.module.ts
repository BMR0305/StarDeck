import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { CardSelectionComponent } from './card-selection/card-selection.component';


const routes: Routes = [

  {path : 'register', component: RegisterComponent},
  {path : 'card-selection', component: CardSelectionComponent}

];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
  ],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
