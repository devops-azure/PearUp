import { Component, ViewEncapsulation, Input, Output, EventEmitter } from '@angular/core';
import { GlobalState } from '../../../../../global.state';
@Component({
    selector: 'app-menu-item',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./menu-item.scss'],
    templateUrl: './menu-item.html'
})
export class MenuItemComponent {

    @Input() menuItem: any;
    @Input() child: boolean = false;

    @Output() itemHover = new EventEmitter<any>();
    @Output() toggleSubMenu = new EventEmitter<any>();
    constructor(private _state: GlobalState) { }
    public onHoverItem($event): void {
        this.itemHover.emit($event);
    }
    public onToggleSubMenu($event, item): boolean {
        if (!$event.item) {
            $event.item = item;
        }
        this.toggleSubMenu.emit($event);
        return false;
    }
    public toggleMenu(collapse: boolean): boolean {
        this._state.collapseButton = true;
        this._state.notifyDataChanged('menu.isCollapsed', collapse);
        return false;
    }
}
