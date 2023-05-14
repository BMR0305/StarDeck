import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/shared/api-module/api.service';
import { Deck } from 'src/app/shared/models/models-cards';

@Component({
  selector: 'app-matchmaking',
  templateUrl: './matchmaking.component.html',
  styleUrls: ['./matchmaking.component.css']
})
export class MatchmakingComponent implements OnInit{

  img_src = 'https://previews.123rf.com/images/ylivdesign/ylivdesign1609/ylivdesign160903327/62577801-icono-de-alien-en-estilo-monocromo-negro-sobre-una-ilustraci%C3%B3n-de-vector-de-fondo-blanco.jpg';
  temp: any; //temporal variable to save the cards from the api
  user = 'Usuario';
  myDeck!: Deck;
  
  deck: Deck = {
    name: 'Deck1',
    code: undefined,
    email_user: undefined,
    cards: undefined
  }

  

  deckList: Deck[] = [];
  

  matchClick() {
    console.log('Matchmaking');

  }

  deckClick(item:any) {
    console.log('Deck: '+ item);
  }


  constructor(private apiService: ApiService) {
  }

  ngOnInit(): void {

    this.user = localStorage.getItem('email')+"";
    this.user = this.user.replace(/"/g, "");

    this.getDecks();
    

  }



  getDecks() {

    const mail = localStorage.getItem("email");
    mail == null ? "" : mail

    let url = "Deck/get/" + mail;
    url = url.replace(/"/g, "");

    this.apiService.get(url).subscribe((data) => {
      console.log(data);
      this.temp = data;
      for (let i = 0; i < this.temp.length; i++) {
        const deck: Deck = {
          name: undefined,
          code: undefined,
          email_user: undefined,
          cards: undefined
        }
        this.deckList.push(deck);
      }

    });

    
  }
  

}




