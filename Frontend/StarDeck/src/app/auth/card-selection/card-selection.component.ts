import { Component, OnInit} from '@angular/core';
import { ApiserviceService} from "../../shared/apiservice.service";
import { MatGridList } from '@angular/material/grid-list';

interface Cards {
  image: string;
  title: string;
}

@Component({
  selector: 'app-card-selection',
  templateUrl: './card-selection.component.html',
  styleUrls: ['./card-selection.component.css']
})

export class CardSelectionComponent {

  constructor(private apiService: ApiserviceService) { }

  firstCard: boolean = true;
  secondCard: boolean = false;
  thirdCard: boolean = false;
  finish: boolean = false;

  cards: Cards[] = [];
  cardsSelected: Cards[] = [];
  cardsPosible: Cards[] = [];

  /**
   * show 3 cards to select and 3 turns for all cartas, then show the finish button. In addition, the cards are removed from the array of possible cards and added to the array of selected cards.
   * @param int position of the card selected
   */

  cardSelection(int : number){

    if(this.firstCard){
      this.firstCard = false;
      this.secondCard = true;
    } else if(this.secondCard){
      this.secondCard = false;
      this.thirdCard = true;
    } else {
      this.thirdCard = false;
      this.finish = true;
    }

    this.cards.push(this.cardsPosible[int]);

    const titleSelected = this.cardsPosible[int].title;

    for (let i = 0; i < this.cards.length; i++) {
      if(this.cardsPosible[i].title == titleSelected){
        this.cardsPosible.splice(i, 1);
      }
    }

    for (let i = 0; i < 3; i++) {
      this.cardsSelected.push(this.cardsPosible[i]);
    }

  }

}
