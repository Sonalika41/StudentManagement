import { Component, OnInit } from '@angular/core';
import { ClassService } from '../../data/class.service';
import { AdminService } from 'src/app/app/features/admin/users/data/admin.service';
import { AuthService } from 'src/app/app/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-classes-list',
  templateUrl: './classes-list.component.html',
  styleUrls: ['./classes-list.component.css']
})
export class ClassesListComponent implements OnInit {
  classes: any[] = [];
  teachers: any[] = [];
  error: string | null = null;

  isIT = false;
  isPrincipal = false;
  isTeacher = false;

  constructor(
    private svc: ClassService,
    private adminSvc: AdminService,
    private auth: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
  const roles = this.auth.getRoles();
console.log('Decoded roles:', roles); // ✅ should show ["Principal"]
this.isIT = roles.includes('IT');
this.isPrincipal = roles.includes('Principal');
this.isTeacher = roles.includes('Teacher');



    this.load();

    if (this.isIT || this.isPrincipal) {
      this.loadTeachers();
    }
  }

  load() {
    this.svc.getAll().subscribe({
      next: res => this.classes = res,
      error: err => this.error = err?.error?.message || err.message || 'Failed to load classes'
    });
  }

 loadTeachers() {
  this.adminSvc.getTeachers().subscribe({
    next: res => {
      console.log('Teachers:', res); // ✅ should show teacher list
      this.teachers = res;
    },
    error: err => this.error = err.message
  });
}




  delete(id: string) {
    if (!this.isIT) return;
    if (!confirm('Delete class?')) return;
    this.svc.delete(id).subscribe({
      next: _ => this.load(),
      error: err => this.error = err?.error?.message || err.message || 'Failed to delete class'
    });
  }

  assign(classId: string, teacherUserId: string) {
    if (!(this.isIT || this.isPrincipal)) return;
    this.svc.assignTeacher({ classId, teacherUserId }).subscribe({
      next: res => {
        const cls = this.classes.find(c => c.classId === res.classId);
        if (cls) {
          cls.classTeacherUserId = res.teacherUserId;
          cls.classTeacherEmail = res.teacherEmail;
        }
      },
      error: err => this.error = err?.error?.message || err.message || 'Failed to assign teacher'
    });
  }

  viewStudents(classId: string) {
    if (!(this.isIT || this.isPrincipal)) return;
    this.router.navigate(['/classes', classId, 'students']);
  }

  editClass(classId: string) {
    if (!(this.isIT || this.isPrincipal)) return;
    this.router.navigate(['/classes/edit', classId]);
  }
}
