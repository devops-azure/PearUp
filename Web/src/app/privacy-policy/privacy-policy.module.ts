import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthenticationService } from '../shared/services/authentication.service'
import { PrivacyPolicyComponent } from './privacy-policy.component';
import { RouterModule, Routes } from '@angular/router';
import { PrivacyPolicyRoutingModule } from './privacy-policy-routing.module';
@NgModule({
  imports: [
    CommonModule,
    PrivacyPolicyRoutingModule,
  ],
  declarations: [
    PrivacyPolicyComponent
  ],
  providers: [
    AuthenticationService
  ]
})
export class PrivacyPolicyModule { }