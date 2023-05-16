import { Component } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {ApiService} from "../../shared/api-module/api.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  cardForm = new FormGroup({
    mail: new FormControl('',[Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(8), Validators.pattern('^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$')]),
  });

  constructor( private router: Router, private apiService: ApiService) { }

  /**
   * Submit form and check if user exists
   */

  onSubmit() {

    this.apiService.get(`Users/login?data=`+this.cardForm.get('mail')?.value+`&data=`+this.cardForm.get('password')?.value).subscribe((data)=>{

      if (data == "User found") {
        localStorage.setItem("email", JSON.stringify(this.cardForm.get('mail')?.value));
        this.haveCards();
      } else {
        alert(data)
      }
    });

  }

  /**
   * Check if user has cards
   */

  haveCards() {
    this.apiService.get("User_Card/HasCards/" + this.cardForm.get('mail')?.value).subscribe((data)=>{
      if (data) {
        this.router.navigate(['/playerview/start'])
        // route to home;
      } else {
        this.router.navigate(['/cards-selection'])
      }
    });
  }

  /**
   * Redirect to register page
   */

  redirecToRegister() {
    this.router.navigate(['/register']);
  }


}
