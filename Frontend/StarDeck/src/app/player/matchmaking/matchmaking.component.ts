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
  user = "";
  userEmail = "";
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

      this.temp = data;
       if (this.temp["message"] == 'Timeout reached'){
        this.msg = "";
       }
       else if (this.temp["message"] == 'Matchmaking canceled'){
        this.msg = "";
       }
       else{

        this.userEmail = localStorage.getItem("email")+"";
        this.userEmail = this.userEmail.replace(/"/g, "");


        if (this.temp["Players"][0]["email"] === this.userEmail){
          localStorage.setItem('oponent', this.temp["Players"][1]["email"]);
        }
        else{
          localStorage.setItem('oponent', this.temp["Players"][0]["email"]);
        }

        localStorage.setItem('planet1',this.temp["Planets"][0]['p_name']);
        localStorage.setItem('planet2',this.temp["Planets"][1]['p_name']);
        localStorage.setItem('planet3',this.temp["Planets"][2]['p_name']);

        localStorage.setItem('game', this.temp);
        localStorage.setItem('IdMatch',this.temp["ID"]);

        console.log(this.temp);


        this.router.navigate(['/game']);
       }


    });

  }

  deckClick(item:any) {

    for (let i = 0; i < this.deckList.length; i++) {
      if (this.deckList[i].name === item) {
        const mail = localStorage.getItem("email");
        mail == null ? "" : mail

        let url = "Users/setDeck/"+this.deckList[i].code +"/" + mail;
        url = url.replace(/"/g, "");

        this.apiService.get(url).subscribe((data) => {
          console.log(data);
        });


        this.myDeck = item;
        localStorage.setItem("deck",this.deckList[i].code)

        break;
      }
    }


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

    const mail = localStorage.getItem("email");
    mail == null ? "" : mail

    let url = "Users/get/" + mail;
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      this.temp = data;
      this.img_src = this.temp[0]["avatar"]
      this.user = this.temp[0]["nickname"];
    });



    this.getDecks();


  }


  /**
   *  Get decks from API
   */

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




