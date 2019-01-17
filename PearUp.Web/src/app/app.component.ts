import { CommonUtility } from './shared/utilities/common-utility.service';
import {
  Component,
  NgZone,
  OnInit,
  ViewContainerRef,
  ViewEncapsulation,
} from '@angular/core';
import { ThemeConfigService } from './layout';
import { GlobalEventsManager } from './shared/utilities/global-events-manager';
import { GlobalState } from './global.state';
import { layoutPaths } from './layout/theme.constants';
import { User } from './shared/models';
import { UserService } from './shared/services/user.service';


/*
 * App Component
 * Top Level Component
 */
@Component({
  selector: 'app',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['./app.component.scss'],
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {

  public currentUser: User;
  public isMenuCollapsed: boolean = false;
  public isMenuExpandedonHover: boolean = true;
  constructor(private _state: GlobalState,
    private _config: ThemeConfigService,
    private _userService: UserService,
    private _globalEventsManager: GlobalEventsManager,
    private viewContainerRef: ViewContainerRef,
    private zone: NgZone,
  ) {

  
    this._state.subscribe('menu.isCollapsed', (isCollapsed) => {
      this.isMenuCollapsed = isCollapsed;
    });
    this._state.subscribe('menu.isExpandedOnHover', (isExpandedOnHover) => {
      this.isMenuExpandedonHover = isExpandedOnHover;
    });
  }

  public ngOnInit(): void {
    this.currentUser = this._userService.getCurrentUser() || null;
    // After Loggin Success fire this event
    this._globalEventsManager.userLoggedInEmitter.subscribe(
      (isLoggedIn) => this.zone.run(() => {
        // mode will be null the first time it is created,
        if (isLoggedIn !== null) {
          if (isLoggedIn) {
            this.currentUser = this._userService.getCurrentUser();
          } else {
            this.currentUser = null;
          }
        }
      }));
  }

}
