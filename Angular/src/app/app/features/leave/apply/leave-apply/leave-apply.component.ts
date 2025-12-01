import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { LeaveService } from '../../data/leave.service';

@Component({
  selector: 'app-leave-apply',
  templateUrl: './leave-apply.component.html',
  styleUrls: ['./leave-apply.component.css']
})
export class LeaveApplyComponent {
  message: string | null = null;
  error: string | null = null;

  form = this.fb.group({
    fromDate: ['', Validators.required],
    toDate: ['', Validators.required],
    reason: ['', [Validators.required, Validators.maxLength(500)]]
  });

  constructor(private fb: FormBuilder, private svc: LeaveService) {}

  submit() {
    if (this.form.invalid) return;
    this.message = this.error = null;
    this.svc.apply(this.form.value as any).subscribe({
      next: res => this.message = `Leave submitted. Status: ${res.status}`,
      error: err => this.error = err.message
    });
  }
}
