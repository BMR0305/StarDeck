import { Component, OnInit } from '@angular/core';
import {Cards, Planet} from "../../shared/models/models-cards";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ApiService} from "../../shared/api-module/api.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-create-planet',
  templateUrl: './create-planet.component.html',
  styleUrls: ['./create-planet.component.css']
})
export class CreatePlanetComponent implements OnInit{


  constructor( private router: Router, private apiService: ApiService) { }

  typeoptions: string[] = [];
  efectoptions = ['Efecto A', 'Efecto B', 'Efecto C'];

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

    this.getAllPlanets();
    this.getPlanetsType();

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

  /**
   * Send planet information to api to create a new planet on database when submit button is clicked
   */
  onSubmit() {
    
    this.apiService.post("Planet/post", {

      id: "id",
      p_name: this.cardForm.value.name,
      p_image: this.cardForm.value.image64,
      p_description: this.cardForm.value.description,
      p_effect: this.cardForm.value.efects,
      p_type: this.cardForm.value.type,
      p_status: "a"

    }).subscribe((data) => {
      console.log(data);
      this.cardForm.reset();
    }); 
  }

  /**
   * Get all planets from database
   */
  getAllPlanets() {
    this.apiService.get("Planet/getAll").subscribe((data) => {
      this.temp = data;
      for (let i = 0; i < this.temp.length; i++) {
        const planet : Planet = {
          id: this.temp[i].id,
          p_name: this.temp[i].p_name,
          p_image: this.temp[i].p_image,
          p_description: this.temp[i].p_description,
          p_effect: this.temp[i].p_effect,
          p_type: this.temp[i].p_type,
          p_status: this.temp[i].p_status
        }
        this.planets.push(planet);
      }
    });
  }

  /**
   * Get all planets from database
   */
  getPlanetsType() {
    this.apiService.get("Planet/getTypes").subscribe((data) => {
      this.temp = data;
      for (let i = 0; i < this.temp.length; i++) {
        this.typeoptions.push(this.temp[i]);
      }
    });
  }

}
