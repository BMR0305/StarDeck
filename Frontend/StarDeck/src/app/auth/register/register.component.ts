import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApiService } from '../../shared/api-module/api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  constructor( private router: Router, private apiService: ApiService) { }

  maxCharsName = 30;
  maxCharsPassword = 8;

  mail : any;
  name : any;
  nickname : any;
  country : any;
  birthdate : any;
  password : any;
  passwordConfirm : any;
  checkboxButton : any;

  /**
   * Show a alert
   */

  onSubmit() {

    let decission = this.verification();

    if(decission == 0) {
      alert("Por favor, rellene todos los campos");
    } else if (decission == 1) {
      alert("Las contraseñas no coinciden");
    } else if (decission == 2) {
      alert("La contraseña debe tener al menos una letra y un número");
    } else if(decission == 3) {
      alert("Por favor, acepte los términos y condiciones");
    }

    this.apiService.get("User/emailVerification/" + this.mail).subscribe((data)=>{
      if (data) {
        this.sendData()
      } else {
        alert("El email ya está registrado")
      }
    });

  }

  /**
   * Check if all the fields are filled, if the passwords match, if the password has at least one letter and one number and if the checkbox is checked.
   */

  verification() {

    let numbers = "0123456789";
    let letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    let hasNumbers = false;
    let hasLetters = false;
    let passwordString = this.password.toString();

    for(let i = 0; i < this.maxCharsPassword; i++) {
      if(numbers.indexOf(passwordString[i]) != -1) {
        hasNumbers = true;
      }
      if(letters.indexOf(passwordString[i]) != -1) {
        hasLetters = true;
      }
    }

    if(this.mail == null || this.name == null || this.nickname == null || this.country == null || this.birthdate == null || this.password == null || this.passwordConfirm == null) {
      return 0;
    } else if (this.password != this.passwordConfirm) {
      return 1;
    } else if(!hasNumbers || !hasLetters) {
      return 2;
    } else if(!this.checkboxButton) {
      return 3;
    }

    return -1;

  }

  sendData() {

    this.apiService.post("Users/post",{

      id: "string",
      email: this.mail,
      nickname: this.nickname,
      u_name: this.name,
      birthday: this.birthdate + "T20:33:24.106Z",
      nationality: this.country,
      u_password: this.password,
      u_status: "active",
      avatar: "string",
      ranking: 0,
      coins: 0,
      u_type: "admin"

    }).subscribe(data =>{
      localStorage.setItem("email", JSON.stringify(this.mail));
      this.router.navigate(['/card-selection']);
    });

    alert("Registro completado");

  }

}
