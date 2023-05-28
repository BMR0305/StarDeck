import { Component } from '@angular/core';
import { Cards, Planet, Player } from 'src/app/shared/models/models-cards';
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

  //Profile player image default
  player_img = 'https://previews.123rf.com/images/ylivdesign/ylivdesign1609/ylivdesign160903327/62577801-icono-de-alien-en-estilo-monocromo-negro-sobre-una-ilustraci%C3%B3n-de-vector-de-fondo-blanco.jpg';
  
  //Image for the deck
  deck = 'https://cdn-icons-png.flaticon.com/512/1178/1178933.png?w=740&t=st=1683845874~exp=1683846474~hmac=64a15b662b3545f116f90292fe99ab6f0f3c3036692aa285b3fe8114b3f4d4c3';
  
  //Image for hidden planet
  planet0 = 'https://cdn-icons-png.flaticon.com/512/16/16268.png?w=740&t=st=1684308443~exp=1684309043~hmac=a9aeb0e725b0764136935741b1d73d2c0d70f29d1e1cd5741c88f45efd447747';
  
  planet1? : Planet;
  planet2? : Planet;
  planet3? : string;
  seconds = 20;
  oponente?: Player;


  card?: Cards;

  hand_cards: Cards[] = [];
  deck_cards: Cards[] = [];
  lenghtdeck = this.deck_cards.length;
  energy = 1;
  


  constructor(private router: Router, private apiService: ApiService) {  }


  ngOnInit(): void {


    this.getOponente(localStorage.getItem('oponent')+"");

    let url = "Planet/get/" + localStorage.getItem('planet1');
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      this.planet1 = this.temp[0]["p_image"];
    });

    url = "Planet/get/" + localStorage.getItem('planet2');
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      this.planet2 = this.temp[0]["p_image"];
      
    });

    url = "Planet/get/" + localStorage.getItem('planet3');
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      //this.planet3 = this.temp[0]["p_image"];
      this.planet3 = this.planet0;
    });

    this.setDeck();

   
    
    const source = interval(1000);
    const timer = source.pipe(takeWhile(() => this.seconds > 0));

    timer.subscribe(() => {
      this.seconds--;
    });

  }

  setDeck(){

  

    let url = "Deck/get/" + localStorage.getItem("deck");
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      this.temp = this.temp["cards"]
      for(let i = 0; i < this.temp.length; i++){
        console.log()
        const card: Cards = {
          c_image: this.temp[i]["c_image"],
          id: this.temp[i]["ID"],
          c_name: this.temp[i]["c_name"],
          battle_pts: this.temp[i]["battle_pts"],
          energy: this.temp[i]["energy"],
          c_type: this.temp[i]["c_type"],
          race: this.temp[i]["race"],
          c_status: this.temp[i]["c_status"],
          c_description: this.temp[i]["c_description"]
        }
        //console.log(card);
        this.deck_cards.push(card);
        this.lenghtdeck = this.deck_cards.length;
      }
      this.deck_cards.sort(Shuffle);
  
      for(let i = 0; i < 7; i++){
        let mycard = this.deck_cards.pop();
        if(mycard) {
          this.hand_cards.push(mycard);
        }
        this.lenghtdeck = this.deck_cards.length;
      }
      
    });




    
  }

  getOponente(opp: string) {
    let url = "Users/get/" + opp;
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      console.log(this.temp);
      this.oponente = this.temp[0]["nickname"];
      this.player_img = this.temp[0]["avatar"]
    });
  }

  withdraw(){
    this.router.navigate(['/playerview/start'])
  }

}

function Shuffle() {
  return Math.random() - 0.5;
}


