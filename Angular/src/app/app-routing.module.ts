import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  { path: 'auth', loadChildren: () => import('src/app/app/auth/auth/auth.module').then(m => m.AuthModule) },
  { path: 'admin', loadChildren: () => import('src/app/app/features/admin/admin/admin.module').then(m => m.AdminModule) },
  { path: 'classes', loadChildren: () => import('src/app/app/features/classes/classes/classes.module').then(m => m.ClassesModule) },
  { path: 'students', loadChildren: () => import('src/app/app/features/students/students/students.module').then(m => m.StudentsModule) },
  { path: 'leave', loadChildren: () => import('src/app/app/features/leave/leave/leave.module').then(m => m.LeaveModule) },
  { path: '**', redirectTo: 'auth/unauthorized' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}






