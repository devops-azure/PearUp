import { Routes, RouterModule } from '@angular/router';
import { UserInterestsComponent } from './user-interests.component';

const routes: Routes = [
  {
    path: '',
    component: UserInterestsComponent
  }
];

export const UserInterestsRoutingModule = RouterModule.forChild(routes);
