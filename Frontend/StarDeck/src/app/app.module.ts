import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Routes,RouterModule, RouterOutlet} from "@angular/router";
import { MatGridListModule } from '@angular/material/grid-list';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthModule} from "./auth/auth.module";
import { AuthRoutingModule} from "./auth/auth-routing.module";
import { AdminRoutingModule } from './admin/admin-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { PlayerRoutingModule} from "./player/player-routing.module";
import { PlayerModule} from "./player/player.module";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    AppRoutingModule,
    AuthRoutingModule,
    AuthModule,
    MatGridListModule,
    AdminRoutingModule,
    PlayerRoutingModule,
    PlayerModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
