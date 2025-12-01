// import { Component } from '@angular/core';
// import { FormBuilder, Validators } from '@angular/forms';
// import { StudentService } from '../../data/student.service';

// @Component({
//   selector: 'app-student-form',
//   templateUrl: './student-form.component.html',
//   styleUrls: ['./student-form.component.css']
// })
// export class StudentFormComponent {
//   message: string | null = null;
//   error: string | null = null;

//   form = this.fb.group({
//     fullName: ['', Validators.required],
//     age: [18, Validators.required],
//     email: ['', [Validators.required, Validators.email]],
//     course: ['', Validators.required],
//     enrollmentDate: ['', Validators.required],
//     marks: [0, Validators.required],
//     attendancePercentage: [0, Validators.required],
//     guardianContact: [''],
//     classId: ['']
//   });

//   constructor(private fb: FormBuilder, private svc: StudentService) {}

//   submit() {
//     if (this.form.invalid) return;
//     this.message = this.error = null;
//     this.svc.createByIT(this.form.value as any).subscribe({
//       next: res => this.message = `Student created. StudentID: ${res.studentID}, TempPassword: ${res.tempPassword}`,
//       error: err => this.error = err.message
//     });
//   }
// }


import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { StudentService } from '../../data/student.service';
import { ClassService } from 'src/app/app/features/classes/data/class.service';
import { ClassDto } from 'src/app/app/features/classes/data/class.model';

@Component({
  selector: 'app-student-form',
  templateUrl: './student-form.component.html',
  styleUrls: ['./student-form.component.css']
})
export class StudentFormComponent implements OnInit {
  message: string | null = null;
  error: string | null = null;
  classes: ClassDto[] = [];

  form = this.fb.group({
    fullName: ['', Validators.required],
    age: [18, Validators.required],
    email: ['', [Validators.required, Validators.email]],
    course: ['', Validators.required],
    enrollmentDate: ['', Validators.required],
    marks: [0, Validators.required],
    attendancePercentage: [0, Validators.required],
    guardianContact: [''],
    classId: [''] // will be bound to dropdown
  });

  constructor(
    private fb: FormBuilder,
    private svc: StudentService,
    private classSvc: ClassService
  ) {}

  ngOnInit() {
    this.classSvc.getAll().subscribe({
      next: res => this.classes = res,
      error: err => this.error = err.message
    });
  }

  submit() {
    if (this.form.invalid) return;
    this.message = this.error = null;

    this.svc.createByIT(this.form.value as any).subscribe({
      next: res => {
        this.message = `Student created. StudentID: ${res.studentID}, TempPassword: ${res.tempPassword}`;
        this.form.reset({ age: 18, marks: 0, attendancePercentage: 0 });
      },
      error: err => this.error = err.message
    });
  }
}
