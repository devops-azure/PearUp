import { DashBoardService } from './dashboard.service';
import { Component, ViewEncapsulation } from '@angular/core';
import { GlobalEventsManager } from '../shared/utilities/global-events-manager';

@Component({
  selector: 'app-dashboard',
  encapsulation: ViewEncapsulation.Emulated,
  styleUrls: ['./dashboard.scss'],
  templateUrl: './dashboard.html'
})
export class DashboardComponent {
  public dashBoards: Array<any>;
  public isDisabled: boolean;
public testText:string;
  constructor(
    private globalEventsManager: GlobalEventsManager,
    private dashBoardService: DashBoardService
  ) {
    this.isDisabled = true;
  }
  public showLoader() {
    this.globalEventsManager.toggleLoader(true);
    // setTimeout(() => this.globalEventsManager.toggleLoader(false), 5000);

  }
}
