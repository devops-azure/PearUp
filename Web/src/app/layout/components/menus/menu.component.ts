import { ResponseModel } from '../../../shared/models/responseModel';
import { UserService } from '../../../shared/services/user.service';
import {
    Component,
    EventEmitter,
    Input,
    Output,
    ViewEncapsulation
} from '@angular/core';
import { MenuService } from './menu.service';
import { GlobalState } from '../../../global.state';
import { NavigationEnd, Router, Route } from '@angular/router';
import { Subscription } from 'rxjs/Rx';
import { MENU } from "../../../app.menu";


@Component({
    selector: 'app-menu',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./menu.scss'],
    templateUrl: './menu.html',
    providers: [MenuService]
})
export class MenuComponent {

    @Input() sidebarCollapsed: boolean = false;
    @Input() menuHeight: number;
    @Input() isExpandedOnHover: boolean = false;
    @Output() expandMenu = new EventEmitter<any>();

    public menuItems: any[];
    public showHoverElem: boolean;
    public hoverElemHeight: number;
    public hoverElemTop: number;
    protected _onRouteChange: Subscription;
    public outOfArea: number = -200;

    constructor(private _router: Router,
        private _service: MenuService,
        private _state: GlobalState) {
        this._onRouteChange = this._router.events.subscribe((event) => {

            if (event instanceof NavigationEnd) {
                if (this.menuItems) {
                    this.selectMenuAndNotify();
                } else {
                    // on page load we have to wait as event is fired
                    // before menu elements are prepared
                    setTimeout(() => this.selectMenuAndNotify());
                }
            }
        });
    }

    public selectMenuAndNotify(): void {
        if (this.menuItems) {
            this.menuItems = this._service.selectMenuItem(this.menuItems);
            this._state.notifyDataChanged('menu.activeLink', this._service.getCurrentItem());
        }
    }

    public ngOnInit(): void {
         this.menuItems = this._service.convertRoutesToMenus(MENU);
                this.selectMenuAndNotify();
        // this._service.getMenus().subscribe((resp) => {
        //     let response : ResponseModel = resp as ResponseModel
        //     if(response.isSuccess){
        //         this.menuItems = this._service.convertRoutesToMenus(response.data);
        //         this.selectMenuAndNotify();
        //     }
        // })

    }

    public ngOnDestroy(): void {
        this._onRouteChange.unsubscribe();
    }

    public hoverItem($event): void {
        this.showHoverElem = true;
        this.hoverElemHeight = $event.currentTarget.clientHeight;
        // TODO: get rid of magic 66 constant
        this.hoverElemTop = $event.currentTarget.getBoundingClientRect().top - 56;
    }

    public toggleMenu(collapse: boolean, isExpandedOnHover: boolean = false) {
        if (this._state.collapseButton) {
            this._state.notifyDataChanged('menu.isCollapsed', collapse);
            this._state.notifyDataChanged('menu.isExpandedOnHover', isExpandedOnHover);
        }
        return false;
    }

    public toggleSubMenu($event): boolean {
        let submenu = jQuery($event.currentTarget).next();

        if (this.sidebarCollapsed) {
            this.expandMenu.emit(null);
            if (!$event.item.expanded) {
                $event.item.expanded = true;
            }
        }
        // else {
        //  $event.item.expanded = !$event.item.expanded;
        //  submenu.slideToggle();
        // }
        else {
            $event.item.children.forEach(function (child, index, childs) {
                child.expanded = false;
                jQuery(`[title='` + child.title + `']`).find(`ul.al-sidebar-sublist`)
                    .attr('style', 'display:none;');
            });

            // let menusToCollapse = this.menuItems.filter(menu => menu !== $event.item &&
            // (menu.children !== undefined && !menu.children.includes($event.item)
            // && menu.expanded));
            let menusToCollapse = this.getMenusToCollapse(this.menuItems, $event.item);
            menusToCollapse.forEach(function (menu, index, array) {
                menu.expanded = false;
                jQuery(`[title='` + menu.title + `']`).find(`ul.al-sidebar-sublist`).slideToggle();
            });

            $event.item.expanded = !$event.item.expanded;
            submenu.slideToggle();
        }

        return false;
    }

    private getMenusToCollapse(menuItems: any[], currentItem: any) {
        // let menusToCollapse: any[] = menuItems.filter(menu => menu !== currentItem
        //   && (menu.children !== undefined && !menu.children.includes(currentItem)
        //     && menu.expanded));
        let menusToCollapse: any[] = [];

        for (let index = 0; index < menuItems.length; index++) {
            let menu = menuItems[index];

            if (menu !== currentItem
                && (menu.children !== undefined && !menu.children.includes(currentItem)
                    && menu.expanded) && !this.isInHierarchy(menu.children, currentItem)) {
                menusToCollapse.push(menu);
            }

        }

        return menusToCollapse;
    }

    private isInHierarchy(menuItems: any[], currentItem: any): boolean {

        for (let index = 0; index < menuItems.length; index++) {
            let menu = menuItems[index];
            if (menu.expanded) {
                if (menu === currentItem || (
                    menu.children !== undefined && (menu.children.includes(currentItem)
                        || this.isInHierarchy(menu.children, currentItem))
                )) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }
        return true;
    }
}
