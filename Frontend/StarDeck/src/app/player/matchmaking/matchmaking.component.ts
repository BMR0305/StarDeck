import { Component } from '@angular/core';
import { Deck } from 'src/app/shared/models/models-cards';

@Component({
  selector: 'app-matchmaking',
  templateUrl: './matchmaking.component.html',
  styleUrls: ['./matchmaking.component.css']
})
export class MatchmakingComponent {

  img_src = 'https://previews.123rf.com/images/ylivdesign/ylivdesign1609/ylivdesign160903327/62577801-icono-de-alien-en-estilo-monocromo-negro-sobre-una-ilustraci%C3%B3n-de-vector-de-fondo-blanco.jpg';
  user = 'Usuario';
  
  deck: Deck = {
    name: 'Deck1',
    code: undefined,
    name_user: undefined,
    cards: undefined
  }

  deckList: Deck[] = [];

  matchClick() {
    console.log('Matchmaking');

  }

  deckClick(item:any) {
    console.log('Deck: '+ item);
  }

  constructor() { 
    for(let i = 0; i < 4; i++){
      this.deckList.push(this.deck);
    }
  }

}

