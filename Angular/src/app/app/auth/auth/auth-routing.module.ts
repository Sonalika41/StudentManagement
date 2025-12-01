import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthComponent } from './auth.component';
import { LoginComponent } from 'src/app/app/auth/login/login.component';
import { RegisterComponent } from 'src/app/app/auth/register/register.component';
import { UnauthorizedComponent } from 'src/app/app/auth/unauthorized/unauthorized.component';

const routes: Routes = [
  { path: '', component: AuthComponent, children: [
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'unauthorized', component: UnauthorizedComponent }
  ]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule {}
