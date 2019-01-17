import { ErrorPageComponent } from './layout/components/error-page/error-page.component';
import { Routes, RouterModule } from '@angular/router';
import {PearUpAuthGuardService} from './shared/services/pearUpAuthGuard.service';

export const routes: Routes = [
   
    { 
        path: '', 
        redirectTo: 'login', 
        pathMatch: 'full' 
    },
    { 
        path: 'login', 
        loadChildren: 'app/login/login.module#LoginModule' 
    },
    {
        path: 'error',
        component: ErrorPageComponent
    },
    {
        path: 'dashboard',
        canActivate: [PearUpAuthGuardService],
        loadChildren: 'app/dashboard/dashboard.module#DashboardModule'
    },
    {
        path: 'terms',        
        loadChildren: 'app/terms-conditions/terms-conditions.module#TermsConditionsModule'
    },
    {
        path: 'privacypolicy',        
        loadChildren: 'app/privacy-policy/privacy-policy.module#PrivacyPolicyModule'
    },
    {
        path: 'interests',        
        loadChildren: 'app/user-interests/user-interests.module#UserInterestsModule'
    },
    { path: '**', redirectTo: 'error' }
];

export const routing = RouterModule.forRoot(routes, { useHash: false });
