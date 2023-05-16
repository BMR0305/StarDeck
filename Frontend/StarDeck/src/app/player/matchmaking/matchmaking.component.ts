import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/shared/api-module/api.service';
import { Deck } from 'src/app/shared/models/models-cards';
import {Router} from "@angular/router";

@Component({
  selector: 'app-matchmaking',
  templateUrl: './matchmaking.component.html',
  styleUrls: ['./matchmaking.component.css',
]
})
export class MatchmakingComponent implements OnInit{

  constructor( private router: Router, private apiService: ApiService) {  }

  img_src = 'https://previews.123rf.com/images/ylivdesign/ylivdesign1609/ylivdesign160903327/62577801-icono-de-alien-en-estilo-monocromo-negro-sobre-una-ilustraci%C3%B3n-de-vector-de-fondo-blanco.jpg';
  temp: any; //temporal variable to save the cards from the api
  user = 'Usuario';
  msg = "";
  myDeck!: Deck;



  deckList: Deck[] = [];


  matchClick() {

    this.msg = 'Buscando oponente ...';

    const mail = localStorage.getItem("email");
    mail == null ? "" : mail

    let url = "Matchmaking/lookForGame/" + mail;
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      console.log(data);
      this.temp = data;
       if (this.temp["message"] = 'Timeout reached'){
        this.msg = "";
       }
       else{
        this.router.navigate(['/game']);
       }


    });

  }

  deckClick(item:any) {
    console.log('Deck: '+ item);
    this.myDeck = item;
  }

  cancelClick(){
    this.msg = "";
    const mail = localStorage.getItem("email");
    mail == null ? "" : mail

    let url = "Matchmaking/cancelMM/" + mail;
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      console.log(data);
    });

  }


  

  ngOnInit(): void {

    this.user = localStorage.getItem('email')+"";
    this.user = this.user.replace(/"/g, "");

    this.getDecks();

  }



  getDecks() {

    const mail = localStorage.getItem("email");
    mail == null ? "" : mail

    let url = "Deck/getPlayerDecks/" + mail;
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      for (let i = 0; i < this.temp.length; i++) {
        const deck: Deck = {
          name: this.temp[i]["name"],
          code: this.temp[i]["code"],
          email_user: this.temp[i]["email_user"],
          cards: this.temp[i]["cards"]
        }
        this.deckList.push(deck);
      }

    });



  }


}




