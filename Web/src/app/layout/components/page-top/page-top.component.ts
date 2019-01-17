import {
    AfterViewInit,
    Component,
    OnDestroy,
    ViewEncapsulation
} from '@angular/core';
import { GlobalEventsManager } from '../../../shared/utilities/global-events-manager';
import { GlobalState } from '../../../global.state';
import { Observable } from 'rxjs/Rx';
import { Router } from '@angular/router';
import { UserService } from '../../../shared/services';
import { User } from '../../../shared/models/user';
@Component({
    selector: 'app-page-top',
    styleUrls: ['./page-top.scss'],
    templateUrl: './page-top.html',
    encapsulation: ViewEncapsulation.None
})
export class PageTopComponent implements AfterViewInit, OnDestroy {


    public isScrolled: boolean = false;
    public isMenuCollapsed: boolean = false;
    public name: string;
    public lastName: string;
    public menuClickItemObservable: any;
    constructor(private _state: GlobalState,
        private router: Router,
        private _globalEventsManager: GlobalEventsManager,
        private _userService : UserService) {
        this._state.subscribe('menu.isCollapsed', (isCollapsed) => {
            this.isMenuCollapsed = isCollapsed;
        });
        let user = this._userService.getCurrentUser();
        this.name = user !== null ? user.fullName : 'PearUp User';
    }
    public ngAfterViewInit(): void {
        let menuClickItem = document.querySelectorAll('.al-sidebar-list-link > .hidewhencollapsed');
        this.menuClickItemObservable = Observable.fromEvent(menuClickItem, 'click')
            .subscribe(() => {
                this.toggleSidebar();
            });
    }
    public toggleSidebar() {
        this.isMenuCollapsed = !this.isMenuCollapsed;
        if (this.isMenuCollapsed) {
            this._state.collapseButton = true;
        }
        else {
            this._state.collapseButton = false;
        }
        this._state.notifyDataChanged('menu.isCollapsed', this.isMenuCollapsed);
        this._state.notifyDataChanged('menu.isExpandedOnHover', false);
        return false;
    }
    public signOut() {
        localStorage.clear();
        sessionStorage.clear();
        this._userService.removeCurrentUser();
        // Load layout after logged in
        this._globalEventsManager.isUserLoggedIn(false);
        this.router.navigate(['/login']);
    }

    public scrolledChanged(isScrolled) {
        this.isScrolled = isScrolled;
    }
    public ngOnDestroy(): void {
        this.menuClickItemObservable.unsubscribe();
    }
}
