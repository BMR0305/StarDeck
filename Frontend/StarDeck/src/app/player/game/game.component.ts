import { Component } from '@angular/core';
import {Cards, Planet, Player, CardPlayed, CardPlayed_DTO} from 'src/app/shared/models/models-cards';
import { Observable, interval } from 'rxjs';
import { takeWhile } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/shared/api-module/api.service';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})

export class GameComponent {

  temp: any; //temporal variable to save the cards from the api
  cardToPlay : any;

  //Profile player image default
  player_img = 'https://previews.123rf.com/images/ylivdesign/ylivdesign1609/ylivdesign160903327/62577801-icono-de-alien-en-estilo-monocromo-negro-sobre-una-ilustraci%C3%B3n-de-vector-de-fondo-blanco.jpg';

  //Image for the deck
  deck = 'https://cdn-icons-png.flaticon.com/512/1178/1178933.png?w=740&t=st=1683845874~exp=1683846474~hmac=64a15b662b3545f116f90292fe99ab6f0f3c3036692aa285b3fe8114b3f4d4c3';

  //Image for hidden planet
  planet0 = 'https://cdn-icons-png.flaticon.com/512/16/16268.png?w=740&t=st=1684308443~exp=1684309043~hmac=a9aeb0e725b0764136935741b1d73d2c0d70f29d1e1cd5741c88f45efd447747';
  planet3Img : any;

  planet1 : any;
  planet2 : any;
  planet3 : any;
  seconds = 20;
  oponente?: Player;

  card?: Cards;

  hand_cards: Cards[] = [];
  lenghtdeck = 18;
  energy = 0;
  cardPerPlanet = 5;
  idMatch : any;
  idPlayer : any;
  idTurn : any;
  inTurn : boolean = true;
  numberTurn : number = 1;
  loadData = false;
  canGetCard = true;
  gameEnd = false;
  gameResult = 0;

  cardsPlayed: CardPlayed[] = [];
  planet1TopCards: Cards[] = [];
  planet1BottomCards: Cards[] = [];
  planet2TopCards: Cards[] = [];
  planet2BottomCards: Cards[] = [];
  planet3TopCards: Cards[] = [];
  planet3BottomCards: Cards[] = [];
  pointsPlanets : number[] = [0,0,0,0,0,0];

  constructor(private router: Router, private apiService: ApiService) {  }

  /**
   * This method is used to call to get the initial data from the api
   */
  ngOnInit(): void {

    this.idMatch = localStorage.getItem('IdMatch');
    this.idPlayer = localStorage.getItem('playerID');

    console.log(this.idMatch);
    console.log(localStorage.getItem('email'));

    this.getOponente(localStorage.getItem('oponent')+"");

    this.getFirstPlanet();

    const source = interval(1000);
    const timer = source.pipe(takeWhile(() => this.seconds > 0));

    timer.subscribe(() => {
      this.seconds--;

      if(this.seconds == 0){
        //console.log("Termino el turno");
        //this.endTurn();
      }

    });

  }

  //Methods for get the planets from the api

  /**
   * This method is used to get the first planet from the api
   */
  getFirstPlanet(){

    let url = "Planet/get/" + localStorage.getItem('planet1');
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      this.planet1 = this.temp[0];
      this.getSecondPlanet();
    });
  }

  /**
   * This method is used to get the second planet from the api
   */
  getSecondPlanet(){
    let url = "Planet/get/" + localStorage.getItem('planet2');
    url = url.replace(/"/g, "");
    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      this.planet2 = this.temp[0];
      this.getThirdPlanet();
    });
  }

  /**
   * This method is used to get the Third planet from the api
   */
  getThirdPlanet(){

    let url = "Planet/get/" + localStorage.getItem('planet3');
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      this.planet3 = this.temp[0];
      this.planet3Img = this.planet3.p_image;
      this.planet3.p_image = this.planet0;
      this.getGameTurn();
    });

  }


  //Methods for get the turn from the api

  /**
   * This method is used to get the turn from the api. Only the first turn.
   */
  getGameTurn(){

    let url = "Match/GetGameTurn/" + this.idMatch;
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      console.log("turno inicial");
      console.log(this.temp);
      this.idTurn = this.temp["TurnID"];
      this.energy = this.temp["Energy"];
      this.numberTurn = this.temp["TurnNumber"];
      if (this.numberTurn == 4){
        this.planet3.p_image = this.planet3Img;
      }
      console.log(this.idTurn);
      this.setHand();
    });

  }

  /**
   * This method is used to get the next turn from the api. This methos is called when the player end his turn. Not the first turn.
   */
  getNextTurn(actualTurn: any){

    console.log("conseguir siguiente turno")

    let url = "Match/GetGameTurn/" + this.idMatch;
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      let turnFromAPI : string = this.temp["TurnID"];
      let lastTurn : string = this.idTurn;

      console.log("Turno actual: " + actualTurn);
      console.log("Turno de la API: " + turnFromAPI);

      if(turnFromAPI != actualTurn){

        this.numberTurn = this.temp["TurnNumber"];
        this.idTurn = turnFromAPI;
        this.energy = this.temp["Energy"];

        if (this.numberTurn == 4){
          this.planet3.p_image = this.planet3Img;
        }

        this.getCardsFromOponent(lastTurn);
      }else{
        this.getNextTurn(actualTurn);
      }

    });


  }

  getCardsFromOponent(lastTurn : string){

    let url = "Match/GetCardsPlayed/" + this.idMatch + "/" + lastTurn + "/" + localStorage.getItem('email');
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      let listCardOpponent : CardPlayed_DTO[] = [];
      listCardOpponent = this.temp;

      console.log("Cartas del oponente");
      console.log(listCardOpponent);

      for(let i = 0; i < listCardOpponent.length; i++){

        let cardPlayed : CardPlayed_DTO = listCardOpponent[i];

        if(cardPlayed.Planet == this.planet1.ID){
          this.pointsPlanets[1] += cardPlayed.Card.battle_pts;
          this.planet1TopCards.push(cardPlayed.Card);
        } else if(cardPlayed.Planet == this.planet2.ID){
          this.planet2TopCards.push(cardPlayed.Card);
          this.pointsPlanets[3] += cardPlayed.Card.battle_pts;
        } else if(cardPlayed.Planet == this.planet3.ID){
          this.planet3TopCards.push(cardPlayed.Card);
          this.pointsPlanets[5] += cardPlayed.Card.battle_pts;
        }

      }

      //This is the final request to the api. So the player can play again, we need to reset the variables.
      this.inTurn = true;
      this.canGetCard = true;
      this.seconds = 20;

    });
  }

  setHand(){

    let url = "Match/GetHand/" + this.idMatch + "/" + localStorage.getItem('email');
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;

      let listID : string[] = [];

      listID.push(this.temp["Card1_ID"]);
      listID.push(this.temp["Card2_ID"]);
      listID.push(this.temp["Card3_ID"]);
      listID.push(this.temp["Card4_ID"]);
      listID.push(this.temp["Card5_ID"]);

      this.setHandCards(listID);

    });

  }

  setHandCards(listId: string[]){

    this.apiService.get("Card/getCard/" + listId[0]).subscribe((data) => {
      this.temp = data;
      this.hand_cards.push(this.temp);
      listId.shift();

      if (listId.length > 0){
        this.setHandCards(listId);
      } else {
        this.getCardsLeft();
        this.loadData = true;
        this.seconds = 20;
      }

    });

  }

  getOponente(opp: string) {

    let url = "Users/get/" + opp;
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      this.oponente = this.temp[0]["nickname"];
      this.player_img = this.temp[0]["avatar"]
    });
  }

  withdraw(){
    this.router.navigate(['/playerview/start'])
  }

  selectionCard(cardSelected: Cards){
    let index = this.hand_cards.indexOf(cardSelected);
    this.cardToPlay = this.hand_cards[index];
  }

  addToPlanetDown(planet : string, numberOfCards: number){

    let condition = this.verificationCardSelected(numberOfCards);

    if(condition){
      if(planet == 'planet1'){
        this.addCardPlayed(this.planet1.ID);
        this.planet1BottomCards.push(this.cardToPlay);
        this.pointsPlanets[0] += this.cardToPlay.battle_pts;
      }else if(planet == 'planet2'){
        this.addCardPlayed(this.planet2.ID);
        this.planet2BottomCards.push(this.cardToPlay);
        this.pointsPlanets[2] += this.cardToPlay.battle_pts;
      }else if(planet == 'planet3'){
        this.addCardPlayed(this.planet3.ID);
        this.planet3BottomCards.push(this.cardToPlay);
        this.pointsPlanets[3] += this.cardToPlay.battle_pts;
      }
      this.energy -= this.cardToPlay.energy;
      this.deleteToHand();
    }

  }

  verificationCardSelected(numberOfCards: number) {

    let condition = false;

    if(this.cardToPlay == undefined){
      alert("No ha seleccionado una carta");
    } else if(numberOfCards >= this.cardPerPlanet){
      alert("No puede jugar mas cartas en este planeta");
    } else if(this.cardToPlay.energy > this.energy){
      alert("No tiene suficiente energia para jugar esta carta");
    } else if(this.seconds == 0){
      alert("No hay mas tiempo para jugar cartas");
    } else {
      condition = true;
    }

    return condition;

  }

  deleteToHand(){
    this.hand_cards.splice(this.hand_cards.indexOf(this.cardToPlay), 1);
    this.cardToPlay = undefined;
  }

  addCardPlayed(planetId: string){

    let CardPlayed: CardPlayed = {
      GameId: this.idMatch,
      CardId: this.cardToPlay.ID,
      PlayerId: this.idPlayer,
      Turn: this.idTurn,
      Planet : planetId
    }

    this.cardsPlayed.push(CardPlayed);

  }

  endTurn() {

    console.log("terminar turno");

    if(!this.inTurn){
      alert("No es su turno");
      return;
    }

    this.inTurn = false;

    // @ts-ignore
    let mail = localStorage.getItem("email").toString();
    mail = mail.replace(/"/g, "");

    if(this.numberTurn == 5){

      this.apiService.get("Match/EndGame/" + this.idMatch).subscribe((data) => {

        this.temp = data;

        console.log(this.temp);

      });

    } else {

      this.apiService.update("Match/EndTurn/" + this.idMatch + "/" + mail, this.cardsPlayed).subscribe((data) => {
        this.temp = data;
        this.cardsPlayed = [];
        console.log(this.temp);
        this.getNextTurn(this.idTurn);
      });


    }


  }










  getCardFromDeck() {

    if(!this.canGetCard){
      alert("Ya ha tomado una carta este turno");
      return;
    } else if(!this.inTurn){
      alert("El turno ha acabado");
      return;
    }

    //update the variable to not get more cards before to call the api.
    this.canGetCard = false;

    // @ts-ignore
    let mail = localStorage.getItem("email").toString();
    mail = mail.replace(/"/g, "");

    this.apiService.get("Match/TakeCard/" + mail).subscribe((data) => {
      this.temp = data;
      this.hand_cards.push(this.temp);
      this.getCardsLeft();
    });

  }

  getCardsLeft(){

    console.log("get cards left");

    // @ts-ignore
    let mail = localStorage.getItem("email").toString();
    mail = mail.replace(/"/g, "");

    this.apiService.get("Match/GetCardsLeft/" + mail).subscribe((data) => {
      this.temp = data;
      console.log(this.temp);
      this.lenghtdeck = this.temp["message"];
    });

  }


}
