import { Inject, Injectable } from '@angular/core';
import { User } from '../models/user';
import { CookieService } from 'ngx-cookie-service';
import {AppConstants} from '../../app.constants'

@Injectable()
export class UserService {
    constructor(
        private cookieService: CookieService
    ) { }

    getCurrentUser(): User {
        let user = this.cookieService.get(AppConstants.user.currentUser);
        if (user === "")
            return null;

        return JSON.parse(this.cookieService.get(AppConstants.user.currentUser));
    }

    setCurrentUser(user: User) {
        this.cookieService.set(AppConstants.user.currentUser, JSON.stringify(user));
    }

    removeCurrentUser() {
        this.cookieService.delete(AppConstants.user.currentUser);
    }
}
