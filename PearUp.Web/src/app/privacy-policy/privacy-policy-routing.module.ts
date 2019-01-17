import { Routes, RouterModule } from '@angular/router';
import { PrivacyPolicyComponent } from './privacy-policy.component';

const routes: Routes = [
  {
    path: '',
    component: PrivacyPolicyComponent
  }
];

export const PrivacyPolicyRoutingModule = RouterModule.forChild(routes);