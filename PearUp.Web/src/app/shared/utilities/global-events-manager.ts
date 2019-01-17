import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class GlobalEventsManager {


    protected _isUserLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(null);
    protected _isCurrentUserChanged: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(null);
    protected _isLoaderShown: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    public userLoggedInEmitter: Observable<boolean> = this._isUserLoggedIn.asObservable();
    public currentUserChangedEmitter: Observable<boolean> =
    this._isCurrentUserChanged.asObservable();
    public loaderEmitter: Observable<boolean> = this._isLoaderShown.asObservable();

    protected _errorType: BehaviorSubject<string> = new BehaviorSubject<string>(null);
    public errorStateHandler: Observable<string> = this._errorType.asObservable();
    
    constructor() { }

    public isUserLoggedIn(ifLoggedIn: boolean) {
        this._isUserLoggedIn.next(ifLoggedIn);
    }
    public updateCurrentUser(ifChanged: boolean) {
        this._isCurrentUserChanged.next(ifChanged);
    }

    public toggleLoader(show: boolean) {
        this._isLoaderShown.next(show);
    }
    public error(statusCode: string) {
        this._errorType.next(statusCode);
    }
}
