import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthenticationService } from '../shared/services/authentication.service'
import { TermsConditionsRoutingModule } from './terms-conditions-routing.module';
import { RouterModule, Routes } from '@angular/router';
import { TermsConditionsComponent } from './terms-conditions.component'
@NgModule({
  imports: [
    CommonModule,
    TermsConditionsRoutingModule,
  ],
  declarations: [
    TermsConditionsComponent
  ],
  providers: [
    AuthenticationService
  ]
})
export class TermsConditionsModule { }