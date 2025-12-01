// import { Component, OnInit } from '@angular/core';
// import { FormBuilder, Validators } from '@angular/forms';
// import { ClassService } from '../../data/class.service';
// import { ActivatedRoute, Router } from '@angular/router';
// import { AdminService } from 'src/app/app/features/admin/users/data/admin.service';

// @Component({
//   selector: 'app-class-form',
//   templateUrl: './class-form.component.html',
//   styleUrls: ['./class-form.component.css']
// })
// export class ClassFormComponent implements OnInit {
//   id: string | null = null;
//   message: string | null = null;
//   error: string | null = null;

//   teachers: any[] = [];

//   form = this.fb.group({
//     name: ['', Validators.required],
//     section: ['', Validators.required],
//     classTeacherUserId: ['']
//   });

//   constructor(
//     private fb: FormBuilder,
//     private svc: ClassService,
//     private route: ActivatedRoute,
//     private router: Router,
//     private adminSvc: AdminService
//   ) {
//     this.id = this.route.snapshot.paramMap.get('id');
//   }

//   ngOnInit() {
//     this.adminSvc.getTeachers().subscribe({
//       next: res => this.teachers = res,
//       error: err => this.error = err.message
//     });

//     if (this.id) {
//       // Optional: prefill by class id
//       this.svc.getAll().subscribe({
//         next: res => {
//           const cls = res.find(c => c.classId === this.id);
//           if (cls) {
//             this.form.patchValue({
//               name: cls.name,
//               section: cls.section,
//               classTeacherUserId: cls.classTeacherUserId || ''
//             });
//           }
//         },
//         error: err => this.error = err.message
//       });
//     }
//   }

//   submit() {
//     if (this.form.invalid) return;
//     this.message = this.error = null;
//     const payload = this.form.value as any;

//     if (this.id) {
//       this.svc.update(this.id, payload).subscribe({
//         next: _ => { this.message = 'Class updated.'; this.router.navigate(['/classes']); },
//         error: err => this.error = err.message
//       });
//     } else {
//       this.svc.create(payload).subscribe({
//         next: _ => { this.message = 'Class created.'; this.router.navigate(['/classes']); },
//         error: err => this.error = err.message
//       });
//     }
//   }
// }


import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ClassService } from '../../data/class.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminService } from 'src/app/app/features/admin/users/data/admin.service';

@Component({
  selector: 'app-class-form',
  templateUrl: './class-form.component.html',
  styleUrls: ['./class-form.component.css']
})
export class ClassFormComponent implements OnInit {
  id: string | null = null;
  message: string | null = null;
  error: string | null = null;

  teachers: any[] = [];

  form = this.fb.group({
    name: ['', Validators.required],
    section: ['', Validators.required],
    classTeacherUserId: ['']
  });

  constructor(
    private fb: FormBuilder,
    private svc: ClassService,
    private route: ActivatedRoute,
    private router: Router,
    private adminSvc: AdminService
  ) {
    this.id = this.route.snapshot.paramMap.get('id');
  }

  ngOnInit() {
    // Load teachers for dropdown
    this.adminSvc.getTeachers().subscribe({
      next: res => this.teachers = res,
      error: err => this.error = err.message
    });

    // If editing, prefill form
    if (this.id) {
      this.svc.getAll().subscribe({
        next: res => {
          const cls = res.find(c => c.classId === this.id);
          if (cls) {
            this.form.patchValue({
              name: cls.name,
              section: cls.section,
              classTeacherUserId: cls.classTeacherUserId || ''
            });
          }
        },
        error: err => this.error = err.message
      });
    }
  }

  submit() {
    if (this.form.invalid) return;
    this.message = this.error = null;
    const payload = this.form.value as any;

    if (this.id) {
      this.svc.update(this.id, payload).subscribe({
        next: _ => {
          this.message = 'Class updated successfully.';
          this.router.navigate(['/classes']);
        },
        error: err => this.error = err.message
      });
    } else {
      this.svc.create(payload).subscribe({
        next: _ => {
          this.message = 'Class created successfully.';
          this.router.navigate(['/classes']);
        },
        error: err => this.error = err.message
      });
    }
  }
}
