import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataTableModule,Column } from 'primeng/primeng';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';

import { UserInterestsRoutingModule } from './user-interests-routing.module';
import { AuthenticationService } from '../shared/services/authentication.service'
import { UserInterestsComponent } from './user-interests.component'
import { UserInterestsService } from './user-interests.service'
import {PearupGridComponent  } from '../shared/components/pearup-grid/pearup-grid.component'

@NgModule({
  imports: [
    CommonModule,
    UserInterestsRoutingModule,
    DataTableModule,
    ConfirmDialogModule
  ],
  declarations: [
    UserInterestsComponent,
    PearupGridComponent
  ],
  providers: [
    AuthenticationService,
    UserInterestsService,
    ConfirmationService
  ]
})
export class UserInterestsModule { }