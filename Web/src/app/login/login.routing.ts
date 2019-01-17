import { LoginComponent } from './login.component';
import { ModuleWithProviders } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Routes
export const routes: Routes = [
    {
        path: '',
        component: LoginComponent
    }
];
export const routing = RouterModule.forChild(routes);
