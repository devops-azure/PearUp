import { Router } from '@angular/router';
import {
    HttpClient,
    HttpRequest,
    HttpErrorResponse,
    HttpEvent,
    HttpHandler,
    HttpHeaders,
    HttpInterceptor,
    HttpResponse
} from '@angular/common/http';
import { Inject, Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { User } from '../../models/index';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/retryWhen';
import * as _ from 'lodash';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/range';
import 'rxjs/add/operator/zip';
import 'rxjs/add/observable/throw';
import { AuthenticationService } from "../../services/authentication.service";
import { UserService } from '../../services/user.service'
import { AppConstants } from '../../../app.constants'
@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(
        private _injector: Injector,
        private _userService: UserService
    ) {
    }

    intercept(req: HttpRequest<any>,
        next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req.clone({
            headers: this.addHeader(req)
        }))
    }

    addHeader(req: HttpRequest<any>) {
        let headers: HttpHeaders = req.headers;
        if (req.url.search(AppConstants.common.loginUrl) === -1) {
            let token = this.getToken();
            // For each Request
            return headers.set('Authorization', `Bearer  ${token}`)
                .set('Content-Type', 'application/json');
        } else {
            // To get Api Token
            return headers.set('Content-Type', 'application/json');
        }

    }

    private getToken() {
        let user: User = this._userService.getCurrentUser();
        return user ? user.accessToken || '' : '';
    }
}