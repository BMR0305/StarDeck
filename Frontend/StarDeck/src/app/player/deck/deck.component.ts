
import { Component, NgModule, OnInit} from '@angular/core';
import {ApiService} from "../../shared/api-module/api.service";
import { Cards, Deck } from '../../shared/models/models-cards';


@Component({
  selector: 'app-deck',
  templateUrl: './deck.component.html',
  styleUrls: ['./deck.component.css']
})
export class DeckComponent implements OnInit {

  decks : Deck[] = []; //all decks
  cardsDeck: Cards[] = []; //deck to save
  cardsAvailable: Cards[] = []; //all cards per user
  cardsEdit: Cards[] = []; //deck to edit
  cardsCreate: Cards[] = []; //deck to create

  temp: any; //temporal variable to save the cards from the api
  nameDeck: string = ""; //name of the deck
  mode: number = 0; //0 = view, 1 = edit, 2 = create

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

      // create deck
      const deck: Deck = {
        name: this.nameDeck,
        code: "12345678",
        email_user: "test",
        cards: this.cardsDeck,
      }

      this.decks.push(deck);

      // save decks in localstorage
      localStorage.setItem("decks", JSON.stringify(this.decks));


    } else {
      alert("You must enter a name for the deck");
    }
  }

  createDeck() {

    this.mode = 1;
    this.cardsDeck = [];
    this.cardsCreate = this.cardsAvailable;

  }

  deleteDeck(item: Deck) {}

  editDeck(item: Deck) {

    this.mode = 2;
    this.cardsDeck = item.cards;
    this.cardsEdit = this.cardsAvailable;

    for(let i = 0; i < this.cardsDeck.length; i++){
      for (let j = 0; j < this.cardsEdit.length; j++) {
        if(this.cardsDeck[i].id == this.cardsEdit[j].id){
          this.cardsEdit.splice(j, 1);
        }
      }
    }

  }


  backToView() {

    this.mode = 0;
    this.cardsDeck = [];
    this.cardsEdit = [];
    this.nameDeck = "";

  }
}
