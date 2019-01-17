import {
    AfterViewInit,
    ChangeDetectorRef,
    Component,
    Input,
    OnInit,
    ViewEncapsulation
} from '@angular/core';
import { GlobalEventsManager } from '../../../shared/utilities/global-events-manager';

@Component({
    selector: 'app-loader',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./loader.scss'],
    templateUrl: './loader.html',
})
export class LoaderComponent implements AfterViewInit, OnInit {
    public label: string;
    public showLoader: boolean = false;
    constructor(private cdr: ChangeDetectorRef, private _globalEventsManager: GlobalEventsManager) {
    }

    public ngAfterViewInit() {
        this.cdr.detectChanges();
    }

    public ngOnInit(): void {
        this.label = 'PearUp';
        // Show or Hide Loader
        this._globalEventsManager.loaderEmitter.subscribe(show => {
            setTimeout(() => this.showLoader = show || false, 0);
        },
            (err) => { console.error('LoaderEmitter-', err); }
        );
    }
}
