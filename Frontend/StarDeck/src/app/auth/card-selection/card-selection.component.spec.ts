import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardSelectionComponent } from './card-selection.component';
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {MatGridListModule} from "@angular/material/grid-list";
import {ReactiveFormsModule} from "@angular/forms";
import {CUSTOM_ELEMENTS_SCHEMA} from "@angular/core";
import { Cards } from '../../shared/models/models-cards';

describe('CardSelectionComponent', () => {
  let component: CardSelectionComponent;
  let fixture: ComponentFixture<CardSelectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CardSelectionComponent ],
      imports: [HttpClientTestingModule, MatGridListModule, ReactiveFormsModule],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardSelectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('card added', () => {

    // generate a list cards empty
    let cards_test: Cards[] = [];

    for(let i = 0; i < 9; i++){

      let card: Cards = {
        id: i,
        c_name: "test" + i.toString(),
        battle_pts: 0,
        energy: 0,
        c_image: "test" + i.toString(),
        c_type: "test" + i.toString(),
        race: "test",
        c_status: "test",
        c_description: "test"
      }

      cards_test.push(card);

    }

    component.cardsPosible = cards_test;

    component.cardSelection(0);

    expect(component.cardPosition).toEqual(3);
    expect(component.cards.length).toEqual(1);


  });

});
