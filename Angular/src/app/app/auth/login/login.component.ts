import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService, LoginResponseDto } from '../auth.service';
import { TokenStorageService } from 'src/app/core/auth/token-storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  error: string | null = null;

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private token: TokenStorageService
  ) {}

  submit() {
    if (this.form.invalid) return;
    this.error = null;

    const { email, password } = this.form.value;

    this.auth.login(email ?? '', password ?? '').subscribe({
      next: (res: LoginResponseDto) => {
        // ✅ Safe casting: convert null → undefined
        this.token.setLogin({
          token: res.token,
          roles: res.roles,
          studentId: res.studentID ?? undefined,
          teacherClassId: res.teacherClassId ?? undefined,
          email: res.email
        });

        const roles = res.roles || [];
        if (roles.includes('IT')) location.href = '/admin';
        else if (roles.includes('Principal')) location.href = '/classes';
        else if (roles.includes('Teacher')) location.href = '/classes';
        else if (roles.includes('Student')) location.href = '/students/view';
        else location.href = '/auth/unauthorized';
      },
      error: err => this.error = err.message
    });
  }
}
