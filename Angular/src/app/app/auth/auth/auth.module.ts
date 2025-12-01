import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthRoutingModule } from './auth-routing.module';
import { AuthComponent } from './auth.component';
import { LoginComponent } from 'src/app/app/auth/login/login.component';
import { RegisterComponent } from 'src/app/app/auth/register/register.component';
import { UnauthorizedComponent } from 'src/app/app/auth/unauthorized/unauthorized.component';

@NgModule({
  declarations: [AuthComponent, LoginComponent, RegisterComponent, UnauthorizedComponent],
  imports: [SharedModule, FormsModule, ReactiveFormsModule, AuthRoutingModule]
})
export class AuthModule {}
