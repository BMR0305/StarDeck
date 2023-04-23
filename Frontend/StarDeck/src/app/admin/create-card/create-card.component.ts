import { Component, NgModule } from '@angular/core';
import { FormGroup, FormControl, Validators, FormsModule, ReactiveFormsModule, NgModel} from '@angular/forms';
import { AdminModule } from '../admin.module';
import { NgSelectModule } from '@ng-select/ng-select';



@Component({
  selector: 'app-create-card',
  templateUrl: './create-card.component.html',
  styleUrls: ['./create-card.component.css'],
  
})

export class CreateCardComponent {

  imageUrl: any;

  maxCharsName = 30
  maxCharsDesccrip = 1000
  isNumeric(value: string | number): boolean {
    return !isNaN(Number(value));
  }
  cardForm = new FormGroup({
    name: new FormControl('',[Validators.required, Validators.minLength(5), Validators.maxLength(30)]),
    energy: new FormControl(0, [Validators.required, Validators.min(-100), Validators.max(100), ]),
    battle: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(100), ]),
    type: new FormControl('',[Validators.required]),
    race: new FormControl('',[Validators.required]),
    description: new FormControl('', [Validators.required, Validators.maxLength(1000)]),
    image: new FormControl('', Validators.required)
  
});
typeoptions = ['Ultra-Rara', 'Muy Rara', 'Rara', 'Normal', 'Básica'];
raceoptions = ['Opción A', 'Opción B', 'Opción C'];

onFileSelected(event: any) {
  if (event.target.files.length > 0) {
    const file = event.target.files[0];
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => {
      this.imageUrl = reader.result;
    };
  }
}


onSubmit() {
  if (this.cardForm.valid) {
    console.log(this.cardForm.value);
  } else {
    this.cardForm.markAllAsTouched();
  }
}

}
