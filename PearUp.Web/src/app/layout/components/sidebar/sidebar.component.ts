import { Component, ElementRef, HostListener, ViewEncapsulation } from '@angular/core';
import { GlobalState } from '../../../global.state';
import { layoutSizes } from '../../../layout';
import * as _ from 'lodash';
import { MENU } from "../../../app.menu";

@Component({
    selector: 'app-sidebar',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./sidebar.scss'],
    templateUrl: './sidebar.html'
})
export class SidebarComponent {

    // we're creating a deep copy since we are going to change that object
    public menuHeight: number;
    public isMenuCollapsed: boolean = false;
    public isMenuShouldCollapsed: boolean = false;
    public isExpandedOnHover: boolean = false;


    constructor(private _elementRef: ElementRef, private _state: GlobalState) {

        this._state.subscribe('menu.isCollapsed', (isCollapsed) => {
            this.isMenuCollapsed = isCollapsed;
        });

        this._state.subscribe('menu.isExpandedOnHover', (isExpandedOnHover) => {
            this.isExpandedOnHover = isExpandedOnHover;
        });

    }

    public ngOnInit(): void {
        // Initially, If you want to close side bar,call below function directly
        // this.menuCollapse();
        // If you want collpase sidebar based on window width use this
        if (this._shouldMenuCollapse()) {
            this.menuCollapse();
        }

    }

    public ngAfterViewInit(): void {
        setTimeout(() => this.updateSidebarHeight());
    }

    @HostListener('window:resize')
    public onWindowResize(): void {

        let isMenuShouldCollapsed = this._shouldMenuCollapse();

        if (this.isMenuShouldCollapsed !== isMenuShouldCollapsed) {
            this.menuCollapseStateChange(isMenuShouldCollapsed);
        }
        this.isMenuShouldCollapsed = isMenuShouldCollapsed;
        this.updateSidebarHeight();
    }

    public menuExpand(): void {
        this.menuCollapseStateChange(false);
    }

    public menuCollapse(): void {
        this.menuCollapseStateChange(true);
    }

    public menuCollapseStateChange(isCollapsed: boolean): void {
        this.isMenuCollapsed = isCollapsed;
        this._state.notifyDataChanged('menu.isCollapsed', this.isMenuCollapsed);
    }

    public updateSidebarHeight(): void {
        // TODO: get rid of magic 84 constant
        this.menuHeight = this._elementRef.nativeElement.childNodes[0].clientHeight - 84;
    }

    private _shouldMenuCollapse(): boolean {
        return window.innerWidth <= layoutSizes.resWidthCollapseSidebar;
    }
}
