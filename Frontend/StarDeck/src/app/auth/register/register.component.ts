import { Component } from '@angular/core';
import { ApiService } from '../../shared/api-module/api.service';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators, FormsModule, ReactiveFormsModule, NgModel} from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  constructor( private router: Router, private apiService: ApiService) { }

  maxCharsName = 30;
  maxCharsPassword = 8;


  cardForm = new FormGroup({
    mail: new FormControl('',[Validators.required, Validators.email]),
    name: new FormControl('', [Validators.required, Validators.minLength(0), Validators.maxLength(30),]),
    nickname: new FormControl('', [Validators.required, Validators.minLength(0), Validators.maxLength(30)]),
    country: new FormControl('',[Validators.required]),
    birthdate: new FormControl('',[Validators.required]),
    password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(8), Validators.pattern('^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$')]),
    passwordConfirm: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(8), Validators.pattern('^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$')]),
    checkboxButton: new FormControl(false, Validators.requiredTrue)
  });

  /**
   * Show a alert and send data to api
   */

  onSubmit() {

    let condition = true;

    if (this.cardForm.get('password')?.value != this.cardForm.get('passwordConfirm')?.value) {
      alert("Las contraseñas no coinciden");
      condition = false;
    } else {
      this.apiService.get("Users/mail/" + this.cardForm.get('mail')?.value).subscribe((data)=>{
        if (data) {
          this.sendData()
        } else if (condition) {
          alert("El email ya está registrado")
        }
      });
    }

  }


  /**
   * Send data to api
   */

  sendData() {

    this.apiService.post("Users/post",{

      id: "id",
      email: this.cardForm.get('mail')?.value,
      nickname: this.cardForm.get('nickname')?.value,
      u_name: this.cardForm.get('name')?.value,
      birthday: this.cardForm.get('birthdate')?.value + "T20:33:24.106Z",
      nationality: this.cardForm.get('country')?.value,
      u_password: this.cardForm.get('password')?.value,
      u_status: "active",
      avatar: "string",
      ranking: 0,
      coins: 0,
      u_type: "user"

    }).subscribe(data =>{
      localStorage.setItem("email", JSON.stringify(this.cardForm.get('mail')?.value));
      this.router.navigate(['/card-selection']);
    });

    alert("Registro completado");

  }

}
