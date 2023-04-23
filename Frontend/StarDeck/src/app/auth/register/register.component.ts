import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

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

  onSubmit() {

    let decission = this.verification();

    if(decission == 0) {
      alert("Por favor, rellene todos los campos");
    } else if (decission == 1) {
      alert("Las contraseñas no coinciden");
    } else if (decission == 2) {
      alert("La contraseña debe tener al menos una letra y un número");
    } else {
      alert("Registro completado");
    }

  }

  verification() {

    //verificar que la contraseña tenga numeros y letras

    let numbers = "0123456789";
    let letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    let hasNumbers = false;
    let hasLetters = false;

    for(let i = 0; i < this.password.length; i++) {
      if(numbers.indexOf(this.password[i]) != -1) {
        hasNumbers = true;
      }
      if(letters.indexOf(this.password[i]) != -1) {
        hasLetters = true;
      }
    }

    if(this.mail == null || this.name == null || this.nickname == null || this.country == null || this.birthdate == null || this.password == null || this.passwordConfirm == null || this.checkboxButton == null) {
      return 0;
    } else if (this.password != this.passwordConfirm) {
      return 1;
    } else if(!hasNumbers || !hasLetters) {
      return 2;
    }

    return -1;

  }

}
