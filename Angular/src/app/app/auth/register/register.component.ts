import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  message: string | null = null;
  error: string | null = null;

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
    role: ['', [Validators.required]],
    fullName: ['']
  });

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) {}

  submit() {
    if (this.form.invalid) return;
    this.message = this.error = null;

    const payload = {
      email: this.form.value.email ?? '',
      password: this.form.value.password ?? '',
      role: this.form.value.role ?? '',
      fullName: this.form.value.fullName ?? undefined
    };

    this.auth.register(payload).subscribe({
      next: res => {
        this.message = `Registered as ${res.role}. Redirecting to login...`;
        setTimeout(() => this.router.navigate(['/auth/login']), 1500); // ✅ redirect after 1.5s
      },
      error: err => this.error = err.message
    });
  }
}
