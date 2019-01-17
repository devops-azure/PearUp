import { Routes, RouterModule } from '@angular/router';
import { TermsConditionsComponent } from './terms-conditions.component';

const routes: Routes = [
  {
    path: '',
    component: TermsConditionsComponent
  }
];

export const TermsConditionsRoutingModule = RouterModule.forChild(routes);