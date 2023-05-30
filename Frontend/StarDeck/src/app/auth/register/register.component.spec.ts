import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterComponent } from './register.component';
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {MatGridListModule} from "@angular/material/grid-list";
import {ReactiveFormsModule} from "@angular/forms";
import {CUSTOM_ELEMENTS_SCHEMA} from "@angular/core";

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegisterComponent ],
      imports: [HttpClientTestingModule, MatGridListModule, ReactiveFormsModule],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('register form', () => {

    component.cardForm.controls['mail'].setValue("asdf@gmail.com");
    component.cardForm.controls['password'].setValue("asdf1234");
    component.cardForm.controls['passwordConfirm'].setValue("asdf1234");
    component.cardForm.controls['name'].setValue("asdf");
    component.cardForm.controls['nickname'].setValue("asdassd");
    component.cardForm.controls['birthdate'].setValue("11-11-1111");
    component.cardForm.controls['country'].setValue("Costa Rica");
    component.cardForm.controls['checkboxButton'].setValue(true);

    expect(component.cardForm.valid).toBeTruthy();

  });
});
