import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AdminRoutingModule } from './admin-routing.module';
import { CreateCardComponent } from './create-card/create-card.component';
import { HttpClientModule }  from "@angular/common/http";


@NgModule({
  declarations: [
    CreateCardComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    ReactiveFormsModule,
    RouterModule,
    HttpClientModule
  ],
  exports:[
    CreateCardComponent
  ],


})
export class AdminModule {

}
