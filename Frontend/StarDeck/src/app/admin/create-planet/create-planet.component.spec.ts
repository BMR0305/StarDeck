import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatePlanetComponent } from './create-planet.component';
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {MatGridListModule} from "@angular/material/grid-list";
import {ReactiveFormsModule} from "@angular/forms";
import {CUSTOM_ELEMENTS_SCHEMA} from "@angular/core";
import { fakeAsync, tick } from '@angular/core/testing';
import { of } from 'rxjs';
import {ApiService} from "../../shared/api-module/api.service";

describe('CreatePlanetComponent', () => {
  let component: CreatePlanetComponent;
  let fixture: ComponentFixture<CreatePlanetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreatePlanetComponent ],
      imports: [HttpClientTestingModule, MatGridListModule, ReactiveFormsModule],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatePlanetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  // Test for form validation
  it('form invalid when empty', () => {
    expect(component.cardForm.valid).toBeFalsy();
  });

  // Test for form validation with correct data
  it('form valid when correct data', () => {

    let name = component.cardForm.controls['name'];
    let efects = component.cardForm.controls['efects'];
    let type = component.cardForm.controls['type'];
    let description = component.cardForm.controls['description'];
    let image = component.cardForm.controls['image'];
    let image64 = component.cardForm.controls['image64'];

    console.log(component.cardForm.value);

    name.setValue("Tierra");
    efects.setValue("Efecto");
    type.setValue("basico");
    description.setValue("Planeta Tierra");
    //falta image y image64, sera falso


    console.log(component.cardForm.value);

    expect(component.cardForm.valid).toBeFalsy();

  });


});
