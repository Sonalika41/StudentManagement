import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleGuard } from 'src/app/core/auth/role.guard';
import { ClassesComponent } from './classes.component';
import { ClassesListComponent } from '../list/classes-list/classes-list.component';
import { ClassFormComponent } from '../form/class-form/class-form.component';
import { TeacherClassStudentsComponent } from '../list/teacher-class-students/teacher-class-students.component';
import { ClassStudentsByIdComponent } from '../list/class-students-by-id/class-students-by-id.component';


const routes: Routes = [
  { path: '', component: ClassesComponent, children: [
    { path: '', component: ClassesListComponent, canActivate: [RoleGuard], data: { roles: ['Teacher','Principal','IT'] } },
    { path: 'new', component: ClassFormComponent, canActivate: [RoleGuard], data: { roles: ['IT'] } },
    { path: 'edit/:id', component: ClassFormComponent, canActivate: [RoleGuard], data: { roles: ['IT','Principal'] } },
    { path: 'my-class/students', component: TeacherClassStudentsComponent, canActivate: [RoleGuard], data: { roles: ['Teacher'] } },
    { path: ':classId/students', component: ClassStudentsByIdComponent, canActivate: [RoleGuard], data: { roles: ['IT','Principal'] } }
  ]}
];

@NgModule({ imports: [RouterModule.forChild(routes)], exports: [RouterModule] })
export class ClassesRoutingModule {}
