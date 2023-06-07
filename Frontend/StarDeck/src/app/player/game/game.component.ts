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

  temp: any; //temporal variable to save cada from API

  //images for the screen
  player_img = 'https://previews.123rf.com/images/ylivdesign/ylivdesign1609/ylivdesign160903327/62577801-icono-de-alien-en-estilo-monocromo-negro-sobre-una-ilustraci%C3%B3n-de-vector-de-fondo-blanco.jpg';
  deck = 'https://cdn-icons-png.flaticon.com/512/1178/1178933.png?w=740&t=st=1683845874~exp=1683846474~hmac=64a15b662b3545f116f90292fe99ab6f0f3c3036692aa285b3fe8114b3f4d4c3';
  planet0 = 'https://cdn-icons-png.flaticon.com/512/16/16268.png?w=740&t=st=1684308443~exp=1684309043~hmac=a9aeb0e725b0764136935741b1d73d2c0d70f29d1e1cd5741c88f45efd447747';
  planet3Img : any;

  //Variables get from API
  planet1 : any;
  planet2 : any;
  planet3 : any;
  oponente?: Player;
  card?: Cards;

  //Variable for send the API
  hand_cards: Cards[] = [];
  idMatch : any;
  idPlayer : any;
  idTurn : any;

  //variable for control the actions
  cardToPlay : any;
  canGetCard = true;
  inTurn : boolean = true;
  numberTurn : number = 1;
  cardPerPlanet = 5;
  energy = 0;
  lenghtdeck = 18;
  seconds = 20;
  resetSeconds = 20;
  maxTurns = 7;
  showThirdPlaner = 4;

  //Variable for change the screen
  loadData = false;
  gameEnd = false;
  gameResult = 0;

  cardsPlayed: CardPlayed[] = []; //list for select the card to play

  //list for cards per planet
  planet1TopCards: Cards[] = [];
  planet1BottomCards: Cards[] = [];
  planet2TopCards: Cards[] = [];
  planet2BottomCards: Cards[] = [];
  planet3TopCards: Cards[] = [];
  planet3BottomCards: Cards[] = [];
  pointsPlanets : number[] = [0,0,0,0,0,0]; //list that have the points for each planet.
  // 0 : planet 1 player. 1 : planet 1 opponent. 2 : planet 2 player. 3 : planet 2 opponent. 4 : planet 3 player. 5 : planet 3 opponent

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
    const timer = source.pipe(takeWhile(() => this.seconds > -2));

    timer.subscribe(() => {
      this.seconds--;

      if(this.seconds == 0 && this.inTurn){
        this.seconds = this.resetSeconds;
        console.log("Termino el turno por tiempo");
        this.endTurn();
      } else if(this.seconds <= 0 || !this.inTurn){
        this.seconds = 0;
      }

    });

  }


  /**
   * This method is used to get the name and avatar from the opponent. This method is called when the player start the game.
   * @param opp string with the email from the opponen
   */
  getOponente(opp: string) {

    let url = "Users/get/" + opp;
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      this.oponente = this.temp[0]["nickname"];
      this.player_img = this.temp[0]["avatar"]
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
   * Method for get first hand from the api. This method is called when the player start the game. This method get 5 cards from the api. This method only get the ID card. Needs the method setHandCards for get the cards.
   */
  setHand(){

    let url = "Match/GetHand/" + this.idMatch + "/" + localStorage.getItem('email');
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;

      let listID : string[] = [];

      const numElements = Object.keys(this.temp).length;

      for (let i = 0; i < numElements - 2; i++){
        listID.push(this.temp["Card" + (i+1) + "_ID"]);
      }

      console.log(listID);

      this.setHandCards(listID);

    });

  }

  /**
   * This method is used to get the cards from the api. Use the listID generated in the method setHand for call each card from the api.
   * @param listId string[] with the ID cards
   */
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

  /**
   * This method is used to end turn and send the cards played to the api. First it verifies if the player is in turn, next verifies if the turn is 7,
   * if it is 7, the method call the api to end the game, if not, the method call the api to end the turn and call the method getNextTurn.
   */

  endTurnOrGame() {

    console.log("terminar turno");

    if(!this.inTurn){
      alert("No es su turno");
      return;
    }

    this.inTurn = false;

    this.endTurn();

  }

  /**
   * This method is used to end the turn. This method call the api to end the turn and call the method getNextTurn for change the turn.
   */
  endTurn(){
    // @ts-ignore
    let mail = localStorage.getItem("email").toString();
    mail = mail.replace(/"/g, "");

    this.apiService.update("Match/EndTurn/" + this.idMatch + "/" + mail, this.cardsPlayed).subscribe((data) => {
      this.temp = data;
      this.cardsPlayed = [];
      console.log(this.temp);
      this.getNextTurn(this.idTurn);
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

        if (this.numberTurn == this.showThirdPlaner){
          this.planet3.p_image = this.planet3Img;
        } else if(this.numberTurn == this.maxTurns){
          this.endParty();
        }

        this.getCardsFromOponent(lastTurn);
      }else{
        this.getNextTurn(actualTurn);
      }

    });


  }

  /**
   * This method is used to get the cards played from the oponent. This method is called the api using the idTurn that is the last turn of the player.\
   * In addition reset the values of inTurn, seconds and canGetCardd.
   * @param lastTurn string with the last turn of the player.
   */
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
      this.seconds = this.resetSeconds;

    });
  }

  /**
   * This method is used to end party or game. This methos call to API to get the information of the winner, lose or tie and change the variable gameEnd to true and
   * the game result to 0 if the player win, 1 if the player lose and 0 is the draw.
   */
  endParty(){
    this.apiService.get("Match/EndGame/" + this.idMatch).subscribe((data) => {
      this.temp = data;
      console.log("Terminada partida");
      console.log(this.temp);

      if(this.temp["ID"] == this.idPlayer){
        this.gameResult = 0;
      } else if(this.temp["ID"] == "Tie"){
        this.gameResult = 2;
      } else{
        this.gameResult = 1;
      }
      this.gameEnd = true;
      this.loadData = false;

      console.log(this.gameResult);
      console.log(this.gameEnd);

    });
  }


  /**
   * This method select the card to play from the hand cards
   * @param cardSelected Card selected from the hand cards
   */
  selectionCard(cardSelected: Cards){
    let index = this.hand_cards.indexOf(cardSelected);
    this.cardToPlay = this.hand_cards[index];
  }

  /**
   * This method is used to put the card selected in the planet selected, plus the points of the card in the planet and added the card to the list of cards played.
   * @param planet string with the planet id selected
   * @param numberOfCards number of cards in the planet selected
   */
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
        this.pointsPlanets[4] += this.cardToPlay.battle_pts;
      }
      this.energy -= this.cardToPlay.energy;
      this.deleteToHand();
    }

  }

  /**
   * This method is used to verify if the card selected can be played.
   * @param numberOfCards
   */
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

  /**
   * After the player select the card to play, this method is used to delete the card from the hand cards and reset the variable cardToPlay.
   */
  deleteToHand(){
    this.hand_cards.splice(this.hand_cards.indexOf(this.cardToPlay), 1);
    this.cardToPlay = undefined;
  }

  /**
   * This method generate a cardPlayed model from the card selected and add it to the list of cards played. This list is used to send the cards played to the api when the player end the turn.
   * @param planetId
   */
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


  /**
   * This method is used to get a card from deck. The deck is in the Backend, so we need to call the api to get a card from the deck.
   * The method verify if the player can get a card and verify if the turn is in turn.
   */
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


  /**
   * This method get the cards left in the deck. The deck is in the Backend, so we need to call the api to get the cards left in the deck.
   */
  getCardsLeft(){

    console.log("get cards left");

    // @ts-ignore
    let mail = localStorage.getItem("email").toString();
    mail = mail.replace(/"/g, "");

    this.apiService.get("Match/GetCardsLeft/" + mail).subscribe((data) => {
      this.temp = data;
      this.lenghtdeck = this.temp["message"];
    });

  }

  /**
   * This method return to the main menu
   */
  withdraw(){
    this.router.navigate(['/playerview/start'])
  }


}
