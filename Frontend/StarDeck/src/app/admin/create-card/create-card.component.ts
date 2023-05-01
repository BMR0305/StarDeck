import { Component, NgModule, OnInit} from '@angular/core';
import { FormGroup, FormControl, Validators, FormsModule, ReactiveFormsModule, NgModel} from '@angular/forms';
import { AdminModule } from '../admin.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { ApiService } from '../../shared/api-module/api.service';
import { Cards } from '../../shared/models/models-cards';

@Component({
  selector: 'app-create-card',
  templateUrl: './create-card.component.html',
  styleUrls: ['./create-card.component.css'],
})

export class CreateCardComponent implements OnInit {
  constructor(private apiService: ApiService) {
  }

  cards: Cards[] = [];
  temp: any;
  imageUrl: any;
  base64Image: any;

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
    image: new FormControl('', Validators.required),
    image64: new FormControl('')
});
typeoptions = ['Ultra-Rara', 'Muy Rara', 'Rara', 'Normal', 'Básica'];
raceoptions = ['Opción A', 'Opción B', 'Opción C'];

  /**
   * Convert image to base64
   * @param event
   */

onFileSelected(event: any) {
  if (event.target.files.length > 0) {
    const file = event.target.files[0];
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => {
      this.imageUrl = reader.result;
      this.base64Image = reader.result;
      this.cardForm.get('image64')?.setValue(this.base64Image);
    };
  }
}

  /**
   * Submit form to the API
   */

onSubmit() {
  if (this.cardForm.valid) {
    console.log(this.cardForm.value);
    console.log(this.cardForm.get('name')?.value)
    this.apiService.post("Card/post",{

      id: "id",
      c_name: this.cardForm.get('name')?.value,
      battle_pts: this.cardForm.get('battle')?.value,
      energy: this.cardForm.get('energy')?.value,
      c_image: this.base64Image,
      c_type: this.cardForm.get('type')?.value,
      race: this.cardForm.get('race')?.value,
      c_status: "a",
      c_description: this.cardForm.get('description')?.value


    }).subscribe((data)=>{
      console.log("Carta agregada");


      const card : Cards = {
        id: "id",
        c_name: this.cardForm.get('name')?.value,
        battle_pts: this.cardForm.get('battle')?.value,
        energy: this.cardForm.get('energy')?.value,
        c_image: this.base64Image,
        c_type: this.cardForm.get('type')?.value,
        race: this.cardForm.get('race')?.value,
        c_status: "a",
        c_description: this.cardForm.get('description')?.value
      }

      this.cards.push(card);
    });

  } else {
    this.cardForm.markAllAsTouched();
  }
}


  /**
   * Get all cards from the API
   */
  ngOnInit(): void {

    this.getCards()

  }

  /**
   * Get all cards from the API and push them to the cards array
   */

  getCards(){

    this.apiService.get("Card/getAll").subscribe((data)=>{
      this.temp = data;
      for (let i = 0; i < this.temp.length; i++) {
        const card : Cards = {
          id : this.temp[i]["ID"],
          c_name: this.temp[i]["c_name"],
          battle_pts: this.temp[i]["battle_pts"],
          energy : this.temp[i]["energy"],
          c_image: this.temp[i]["c_image"],
          c_type: this.temp[i]["c_type"],
          race : this.temp[i]["race"],
          c_status: this.temp[i]["c_status"],
          c_description: this.temp[i]["c_description"]
        }
        this.cards.push(card);
      }

    });

  }

}
