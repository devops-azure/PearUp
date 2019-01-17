import { AuthenticationService } from '../shared/services/authentication.service';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { DashBoardService } from './dashboard.service';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgaModule } from '../layout/nga.module';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { routing } from './dashboard.routing';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    routing
  ],
  declarations: [
    DashboardComponent
  ],
  providers: [
    AuthenticationService,
    DashBoardService
  ]
})
export class DashboardModule {
}
