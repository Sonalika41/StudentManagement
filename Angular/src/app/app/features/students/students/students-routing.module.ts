import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleGuard } from 'src/app/core/auth/role.guard';
import { StudentsComponent } from './students.component';
import { StudentsListComponent } from '../list/students-list/students-list.component';
import { StudentFormComponent } from '../form/student-form/student-form.component';
import { StudentViewComponent } from '../view/student-view/student-view.component';

const routes: Routes = [
  { path: '', component: StudentsComponent, children: [
    { path: '', component: StudentsListComponent, canActivate: [RoleGuard], data: { roles: ['IT'] } },
    { path: 'new', component: StudentFormComponent, canActivate: [RoleGuard], data: { roles: ['IT'] } },
    { path: 'view', component: StudentViewComponent, canActivate: [RoleGuard], data: { roles: ['Student'] } },
  ]}
];

@NgModule({ imports: [RouterModule.forChild(routes)], exports: [RouterModule] })
export class StudentsRoutingModule {}
