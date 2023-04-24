import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ApiserviceService } from "../shared/apiservice.service";
import { AdminRoutingModule } from './admin-routing.module';
import { CreateCardComponent } from './create-card/create-card.component';


@NgModule({
  declarations: [
    CreateCardComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    ReactiveFormsModule,
    RouterModule
  ],
  exports:[
    CreateCardComponent
  ],
  providers: [ApiserviceService]
})
export class AdminModule {

}
