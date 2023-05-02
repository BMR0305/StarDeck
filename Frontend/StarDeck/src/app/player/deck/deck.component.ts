
import { Component, NgModule, OnInit} from '@angular/core';
import {ApiService} from "../../shared/api-module/api.service";
import { Cards } from '../../shared/models/models-cards';


@Component({
  selector: 'app-deck',
  templateUrl: './deck.component.html',
  styleUrls: ['./deck.component.css']
})
export class DeckComponent implements OnInit {

  cardsDeck: Cards[] = [];
  cardsAvailable: Cards[] = [];
  temp: any;
  nameDeck: string = "";

  constructor(private apiService: ApiService) {
  }

  ngOnInit(): void {

    this.getCards();

  }

  getCards() {

    this.apiService.get("Card/getAll").subscribe((data) => {
      this.temp = data;
      for (let i = 0; i < this.temp.length; i++) {
        const card: Cards = {
          id: this.temp[i]["ID"],
          c_name: this.temp[i]["c_name"],
          battle_pts: this.temp[i]["battle_pts"],
          energy: this.temp[i]["energy"],
          c_image: this.temp[i]["c_image"],
          c_type: this.temp[i]["c_type"],
          race: this.temp[i]["race"],
          c_status: this.temp[i]["c_status"],
          c_description: this.temp[i]["c_description"]
        }
        this.cardsAvailable.push(card);
      }

    });

  }

  addCard(card: Cards) {

    if(this.cardsDeck.length == 18){
      alert("You can't add more than 18 cards");
    } else {
      this.cardsDeck.push(card);
      this.cardsAvailable.splice(this.cardsAvailable.indexOf(card), 1);
    }
  }

  deleteCard(card: Cards) {
    this.cardsAvailable.push(card);
    this.cardsDeck.splice(this.cardsDeck.indexOf(card), 1);
  }

  saveDeck() {

    if(this.nameDeck != ""){
      this.apiService.post("Deck/create", {
        name: this.nameDeck,
        cards: this.cardsDeck
      }).subscribe((data) => {
        alert("Deck created");
        this.cardsDeck = [];
        this.cardsAvailable = [];
        this.getCards();
      });
    } else {
      alert("You must enter a name for the deck");
    }
  }
}
