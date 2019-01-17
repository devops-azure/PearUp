import { GlobalEventsManager } from '../../../shared/utilities/global-events-manager';
import { Component, OnInit } from '@angular/core';

@Component({
    templateUrl: './error-page.html',
    styleUrls: ['./error-page.scss']
})

export class ErrorPageComponent implements OnInit {
    public statusCode: string
    constructor(private globalEventsManager: GlobalEventsManager) {
        console.log('ErrorPageComponent');
    }
    ngOnInit(): void {
        console.log('ErrorPageComponent-Init');
        this.globalEventsManager.errorStateHandler.subscribe(statuscode => {
            if (statuscode != null) {
                console.log('ErrorPageComponent-', statuscode);
                this.statusCode = statuscode
            } else {
                this.statusCode = null;
            }
        })
    }
}