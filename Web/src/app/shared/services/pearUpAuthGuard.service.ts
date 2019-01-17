import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '../models/index';
import { UserService } from './user.service';

@Injectable()
export class PearUpAuthGuardService implements CanActivate {

    constructor(private router: Router,
    private _userService: UserService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        // check guards for stopping tester or project admin from
        // accessing user crud and user role crud pages
        let currentUser: User = this._userService.getCurrentUser();
        if (currentUser != null) {
            // logged in so return true
            return true;
        }
        // not logged in so redirect to login page with the return url
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}
