import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleGuard } from 'src/app/core/auth/role.guard';
import { AdminComponent } from './admin.component';
import { UsersComponent } from 'src/app/app/features/admin/users/users/users.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [RoleGuard],
    data: { roles: ['IT'] },
    children: [
      { path: '', component: UsersComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {}
