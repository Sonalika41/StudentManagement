import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleGuard } from 'src/app/core/auth/role.guard';
import { LeaveComponent } from './leave.component';
import { LeaveApplyComponent } from 'src/app/app/features/leave/apply/leave-apply/leave-apply.component';
import { LeaveMineComponent } from 'src/app/app/features/leave/mine/leave-mine/leave-mine.component';

const routes: Routes = [
  { path: '', component: LeaveComponent, children: [
    { path: 'apply', component: LeaveApplyComponent, canActivate: [RoleGuard], data: { roles: ['Student'] } },
    { path: 'mine', component: LeaveMineComponent, canActivate: [RoleGuard], data: { roles: ['Student'] } }
  ]}
];

@NgModule({ imports: [RouterModule.forChild(routes)], exports: [RouterModule] })
export class LeaveRoutingModule {}
