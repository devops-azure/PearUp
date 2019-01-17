import { HttpUtility } from '../utilities/http/http-utility.service';
import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { User, ResponseModel } from '../models';
import { UserService } from './user.service';
import 'rxjs/add/operator/map';

@Injectable()
export class AuthenticationService {
    constructor(private http: HttpUtility,
        private _userService: UserService) { }
    
    getValues() {
        return this.http.get('values');
    }
    public verifyUser(user: User): Observable<any> {
        let bodySend = JSON.stringify({ emailId: user.email, password: user.password });
        return this.http.post('Authentication/AdminLogin', bodySend)
    }

    logout() {
        // remove user from local storage to log user out
        this._userService.removeCurrentUser();
    }  

    getTokenFromStorage() {
        return this._userService.getCurrentUser().accessToken;
    }
}
