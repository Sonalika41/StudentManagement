import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/app/features/admin/users/data/admin.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: any[] = [];
  message: string | null = null;
  error: string | null = null;

  constructor(private admin: AdminService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.admin.getUsers().subscribe({
      next: res => this.users = res,
      error: err => this.error = err.message
    });
  }

  assign(userId: string, role: string) {
    this.message = this.error = null;
    this.admin.assignRole(userId, role).subscribe({
      next: _ => { this.message = 'Role assigned.'; this.load(); },
      error: err => this.error = err.message
    });
  }

  remove(userId: string, role: string) {
    this.message = this.error = null;
    this.admin.removeRole(userId, role).subscribe({
      next: _ => { this.message = 'Role removed.'; this.load(); },
      error: err => this.error = err.message
    });
  }

  delete(userId: string) {
    if (!confirm('Delete user?')) return;
    this.admin.deleteUser(userId).subscribe({
      next: _ => this.load(),
      error: err => this.error = err.message
    });
  }
}
