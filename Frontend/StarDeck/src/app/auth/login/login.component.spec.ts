import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginComponent } from './login.component';
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {MatGridListModule} from "@angular/material/grid-list";
import {ReactiveFormsModule} from "@angular/forms";
import {CUSTOM_ELEMENTS_SCHEMA} from "@angular/core";

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoginComponent ],
      imports: [HttpClientTestingModule, MatGridListModule, ReactiveFormsModule],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  //verify if the form is valid
  it('form should be valid', () => {

      component.cardForm.controls['mail'].setValue("asdf@gmai.com");
      component.cardForm.controls['password'].setValue("asdf1234");

      expect(component.cardForm.valid).toBeTruthy();

  });

});
