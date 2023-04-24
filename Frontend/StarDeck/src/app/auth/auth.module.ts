import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from "@angular/router";
import { AuthRoutingModule } from './auth-routing.module';
import { RegisterComponent } from './register/register.component';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CardSelectionComponent } from './card-selection/card-selection.component';
import { MatGridListModule } from "@angular/material/grid-list";
import { HttpClientModule } from "@angular/common/http";



@NgModule({
  declarations: [
    RegisterComponent,
    CardSelectionComponent
  ],
  imports: [
    CommonModule,
    AuthRoutingModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    MatGridListModule,
    HttpClientModule
  ],
  exports: [
    RegisterComponent,
  ],


})
export class AuthModule { }
