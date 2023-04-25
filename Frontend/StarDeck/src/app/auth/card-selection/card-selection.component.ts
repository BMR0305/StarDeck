import { Component, OnInit} from '@angular/core';
import { MatGridList } from '@angular/material/grid-list';
import { ApiService } from '../../shared/api-module/api.service';

interface Cards {
  id : string;
  c_name: string;
  battle_pts: string;
  energy : string;
  c_image: string;
  c_type: string;
  race : string;
  c_status: string;
  c_description: string;
}

@Component({
  selector: 'app-card-selection',
  templateUrl: './card-selection.component.html',
  styleUrls: ['./card-selection.component.css']
})

export class CardSelectionComponent implements OnInit {

  temp: any;
  firstCard: boolean = true;
  secondCard: boolean = false;
  thirdCard: boolean = false;
  finish: boolean = false;

  cards: Cards[] = [];
  cardsSelected: Cards[] = [];
  cardsPosible: Cards[] = [];

  constructor(private apiService: ApiService) { }

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

      this.apiService.get("Card/getRandom/5?types=Rara&types=Normal").subscribe((data)=>{

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

      });

    });


  }

  /**
   * show 3 cards to select and 3 turns for all cartas, then show the finish button. In addition, the cards are removed from the array of possible cards and added to the array of selected cards.
   * @param int position of the card selected
   */

  cardSelection(int : number){

    if(this.firstCard){
      this.firstCard = false;
      this.secondCard = true;
    } else if(this.secondCard){
      this.secondCard = false;
      this.thirdCard = true;
    } else {
      this.thirdCard = false;
      this.finish = true;
    }

    this.cards.push(this.cardsPosible[int]);

    const titleSelected = this.cardsPosible[int].c_name;

    for (let i = 0; i < this.cardsPosible.length; i++) {
      if(this.cardsPosible[i].c_name == titleSelected){
        this.cardsPosible.splice(i, 1);
      }
    }

    for (let i = 0; i < 3; i++) {
      this.cardsSelected.push(this.cardsPosible[i]);
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
    });

  }

}
