import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCardComponent } from './create-card.component';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { MatGridListModule } from '@angular/material/grid-list';
import { ReactiveFormsModule } from '@angular/forms';

describe('CreateCardComponent', () => {
  let component: CreateCardComponent;
  let fixture: ComponentFixture<CreateCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateCardComponent ],
      imports: [HttpClientTestingModule, MatGridListModule, ReactiveFormsModule],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  //create unit test for from validation
  it('form invalid when empty', () => {
    expect(component.cardForm.valid).toBeFalsy();
  });

  //create unit test for name validation
  it('name field validity', () => {
    let name = component.cardForm.controls['name'];
    expect(name.valid).toBeFalsy();

    name.setValue("");
    expect(name.hasError('required')).toBeTruthy();

    name.setValue("dasdasdasdasdasdasdadadadadadadasdasdadadadadadasdasdadasdsadasdasdasdsada");
    expect(name.hasError('maxlength')).toBeTruthy();
  });


});
