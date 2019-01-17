import { Component, OnInit } from '@angular/core';

import { GlobalState } from '../../../global.state';

@Component({
    selector: 'app-content-top',
    styleUrls: ['./content-top.scss'],
    templateUrl: './content-top.html',
})
export class ContentTopComponent implements OnInit {


    public activePageTitle: string = '';

    constructor(private _state: GlobalState) {
    }

    public ngOnInit(): void {
        this._state.subscribe('menu.activeLink', (activeLink) => {
            if (activeLink) {
                this.activePageTitle = activeLink.title;
            }
        });
    }
}
