import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeckComponent } from './deck.component';
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {MatGridListModule} from "@angular/material/grid-list";
import {ReactiveFormsModule} from "@angular/forms";
import {CUSTOM_ELEMENTS_SCHEMA} from "@angular/core";
import { Cards, Deck } from '../../shared/models/models-cards';

describe('DeckComponent', () => {
  let component: DeckComponent;
  let fixture: ComponentFixture<DeckComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeckComponent ],
      imports: [HttpClientTestingModule, MatGridListModule, ReactiveFormsModule],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('crete deck', () => {

    let cards_test: Cards[] = [];

    for (let i = 0; i < 35; i++) {

      let card : Cards = {
        id : i,
        c_name: "test" + i.toString(),
        battle_pts: 1,
        energy : 1,
        c_image: "test",
        c_type: "test",
        race : "race_test",
        c_status: "status_test",
        c_description: "description_test"
      }
      cards_test.push(card);
    }

    component.createDeck();

    component.cardsAvailable = cards_test;

    expect(component.cardsDeck.length).toBe(0);
    expect(component.mode).toBe(1);
    expect(component.cardsAvailable).toBe(cards_test);

  });

  it('crete deck', () => {

    let cards_test: Cards[] = [];

    for (let i = 0; i < 35; i++) {

      let card : Cards = {
        id : i,
        c_name: "test" + i.toString(),
        battle_pts: 1,
        energy : 1,
        c_image: "test",
        c_type: "test",
        race : "race_test",
        c_status: "status_test",
        c_description: "description_test"
      }
      cards_test.push(card);
    }

    component.createDeck();

    component.cardsAvailable = cards_test;

    expect(component.cardsDeck.length).toBe(0);
    expect(component.mode).toBe(1);
    expect(component.cardsAvailable).toBe(cards_test);

  });

  it('back to view', () => {

    component.backToView();

    expect(component.mode).toBe(0);
    expect(component.nameDeck).toBe("");
    expect(component.cardsDeck.length).toBe(0);
    expect(component.cardsEdit.length).toBe(0);

  });

  it('edit deck', () => {

    let cards_test: Cards[] = [];
    let cards_deck : Cards[] = [];

    for (let i = 0; i < 35; i++) {

      let card : Cards = {
        id : i,
        c_name: "test" + i.toString(),
        battle_pts: 1,
        energy : 1,
        c_image: "test",
        c_type: "test",
        race : "race_test",
        c_status: "status_test",
        c_description: "description_test"
      }
      cards_test.push(card);
    }

    for (let i = 0; i < component.cardsLimit; i++) {
      cards_deck.push(cards_test[i]);
    }

    let deck_test: Deck = {
      name : "test",
      code: "test_code",
      email_user : "test_email",
      cards: cards_deck
    }

    component.cardsAvailable = cards_test;
    component.editDeck(deck_test);

    expect(component.mode).toBe(2);
    expect(component.cardsDeck).toBe(deck_test.cards);
    expect(component.cardsEdit).toBe(cards_test);
    expect(component.cardsEdit).toBe(cards_test);

  });

});
