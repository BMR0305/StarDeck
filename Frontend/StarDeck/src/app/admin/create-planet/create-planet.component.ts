import { Component, OnInit } from '@angular/core';
import {Cards, Planet} from "../../shared/models/models-cards";
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-create-planet',
  templateUrl: './create-planet.component.html',
  styleUrls: ['./create-planet.component.css']
})
export class CreatePlanetComponent implements OnInit{

  typeoptions = ['Popular', 'Raro'];
  raceoptions = ['Opción A', 'Opción B', 'Opción C'];

  planets: Planet[] = [];
  temp: any;
  imageUrl: any;
  base64Image: any;

  maxCharsName = 30
  maxCharsDesccrip = 1000

  cardForm = new FormGroup({
    name: new FormControl('',[Validators.required, Validators.minLength(5), Validators.maxLength(30)]),
    efects: new FormControl('',[Validators.required]),
    type: new FormControl('',[Validators.required]),
    description: new FormControl('', [Validators.required, Validators.maxLength(1000)]),
    image: new FormControl('', Validators.required),
    image64: new FormControl('')
  });

  ngOnInit() {



  }

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

  onSubmit() {

    console.log(this.cardForm.value);

    const planet : Planet = {
      id: "id",
      p_name: this.cardForm.value.name,
      p_effect: this.cardForm.value.efects,
      p_type: this.cardForm.value.type,
      p_description: this.cardForm.value.description,
      p_image: this.cardForm.value.image64
    }

    this.planets.push(planet);


  }
}
