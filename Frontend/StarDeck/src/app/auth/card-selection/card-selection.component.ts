import { Component, OnInit} from '@angular/core';
import { MatGridList } from '@angular/material/grid-list';
import { ApiService } from '../../shared/api-module/api.service';
import { Cards } from '../../shared/models/models-cards';
import {Router} from "@angular/router";

@Component({
  selector: 'app-card-selection',
  templateUrl: './card-selection.component.html',
  styleUrls: ['./card-selection.component.css']
})

export class CardSelectionComponent implements OnInit {

  temp: any;
  finish: boolean = false;
  loading: boolean = false;
  message: string = "Selecciona 3 cartas para tu mazo";
  cardPosition: number = 0;


  cards: Cards[] = [];
  cardsPosible: Cards[] = [];

  constructor(private router: Router,private apiService: ApiService) { }

  /**
   * Get cards from API and save them in cards array
   */

  ngOnInit(): void {

    this.getCards()

  }

  /**
   * Get cards from API and save them in cards array
   */

  getCards(){

    this.apiService.get("Card/getRandom/15?types=Basica").subscribe((data)=>{
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

      this.requestPosibleCards();

    });

  }

  /**
   * Get cards from API and save them in cards array
   */

  requestPosibleCards(){
    this.apiService.get("Card/getRandom/9?types=Rara&types=Normal").subscribe((data)=>{

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

        this.cardsPosible.push(card);

      }

      this.loading = true;

    });
  }

  /**
   * show 3 cards to select and 3 turns for all cartas, then show the finish button. In addition, the cards are removed from the array of possible cards and added to the array of selected cards.
   * @param int position of the card selected
   */

  cardSelection(int : number){

    this.cards.push(this.cardsPosible[int]);

    if(this.cardPosition != 6){
      this.cardPosition += 3;
    }else{
      this.finish = true;
    }

  }

  /**
   * send the selected cards to the API
   */

  onSubmit(){

    const mail = localStorage.getItem("email");
    mail == null ? "" : mail

    let url = "User_Card/post/" + mail;
    url = url.replace(/"/g, "");

    console.log(url);

    this.apiService.post(url, this.cards).subscribe((data)=>{
      console.log(data);
      this.router.navigate(['/playerview/start'])
    });

  }

}
