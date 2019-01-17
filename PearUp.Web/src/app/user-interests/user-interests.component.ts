import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { ConfirmationService } from 'primeng/api';
import * as _ from 'lodash';

import { Interest } from '../shared/models/interest';
import { UserInterestsService } from './user-interests.service'
import { AppConstants } from '../app.constants';
import { PearupGridConfig } from '../shared/components/pearup-grid/pearup-grid-Config';

@Component({
  selector: 'app-user-interests',
  templateUrl: './user-interests.component.html',
  styleUrls: ['./user-interests.component.css']
})
export class UserInterestsComponent implements OnInit {
  public interests: Array<Interest>;
  public constants = { ...AppConstants.userInterests };
  public gridConfig: PearupGridConfig;
  constructor(private _userInterestsService: UserInterestsService,
    private _confirmationService: ConfirmationService) {

  }
  ngOnInit() {
    this.GetUserInterests();
    this.gridConfig = {
      editable: true,
      filter: true,
      lazy: false,
      pagination: true,
      pageLinks: this.constants.pageLinks,
      rowsPerPage: this.constants.maxRows,
      totalRecords: null,
      filterPlaceholderText:this.constants.filterPlaceholderText,
      cellConfig: [
        {
          field: "id",
          editable: false,
          header: "Id",
          center:true,
        },
        {
          field: "interestName",
          editable: true,
          header: "Interest Name",
          center:false,
        },
        {
          field: "interestDescription",
          editable: true,
          header: "Interest Description",
          center:false,
        },
      ]
    }
  }
  private GetUserInterests() {
    this._userInterestsService.GetUserInterests().subscribe(result => {
      this.interests = result as Array<Interest>
    })
  }
  public onDeleteRow(event, item) {
    this._confirmationService.confirm({
      message: AppConstants.commonErrors.delete_confirmation, accept: () => {
        this.interests = _.reject(this.interests, { id: item.id });
      }
    })
  }
}
