import { Component } from '@angular/core';
import { Cards } from 'src/app/shared/models/models-cards';
import { Observable, interval } from 'rxjs';
import { takeWhile } from 'rxjs/operators';


@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})



export class GameComponent {

  img_src = 'https://previews.123rf.com/images/ylivdesign/ylivdesign1609/ylivdesign160903327/62577801-icono-de-alien-en-estilo-monocromo-negro-sobre-una-ilustraci%C3%B3n-de-vector-de-fondo-blanco.jpg';
  planet1 = 'https://cdn-icons-png.flaticon.com/512/44/44748.png?w=740&t=st=1683840954~exp=1683841554~hmac=b93dbb5232b5f572e934deee8ecc7d308f322f70d4a3d2fd2bd34f71bc17361d';
  planet2 = this.planet1;
  planet3 = this.planet1;
  deck = 'https://cdn-icons-png.flaticon.com/512/1178/1178933.png?w=740&t=st=1683845874~exp=1683846474~hmac=64a15b662b3545f116f90292fe99ab6f0f3c3036692aa285b3fe8114b3f4d4c3';
  seconds = 20;


  card: Cards = {
    c_image: 'https://cdn-icons-png.flaticon.com/512/391/391155.png?w=740&t=st=1683843338~exp=1683843938~hmac=a5abda84fa8dab44b6c2e5fc8cd83eb5ea862ce59d31bc1e8bdcbaf581a598d2',
    id: undefined,
    c_name: undefined,
    battle_pts: undefined,
    energy: undefined,
    c_type: undefined,
    race: undefined,
    c_status: undefined,
    c_description: undefined
  }

  hand_cards: Cards[] = [];
  deck_cards: Cards[] = [];
  lenghtdeck = this.deck_cards.length;
  energy = 1;
  


  constructor() { 
    // Utilizamos un bucle for para recorrer el arreglo de números y agregar los números pares al arreglo numerosPares
    for(let i = 0; i < 18; i++){
      this.deck_cards.push(this.card);
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
  }


  ngOnInit(): void {
    const source = interval(1000);
    const timer = source.pipe(takeWhile(() => this.seconds > 0));

    timer.subscribe(() => {
      this.seconds--;
    });
  }

}

function Shuffle() {
  return Math.random() - 0.5;
}
