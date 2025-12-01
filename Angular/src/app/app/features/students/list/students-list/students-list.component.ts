// import { Component, OnInit } from '@angular/core';
// import { StudentService } from '../../data/student.service';
// import { StudentDto } from '../../data/student.model';

// @Component({
//   selector: 'app-students-list',
//   templateUrl: './students-list.component.html',
//   styleUrls: ['./students-list.component.css']
// })
// export class StudentsListComponent implements OnInit {
//   students: StudentDto[] = [];
//   error: string | null = null;

//   constructor(private svc: StudentService) {}

//   ngOnInit() {
//     this.svc.getAll().subscribe({
//       next: res => this.students = res,
//       error: err => this.error = err.message
//     });
//   }
// }

// import { Component, OnInit } from '@angular/core';
// import { StudentService } from '../../data/student.service';
// import { StudentDto, UpdateStudentRequestDto } from '../../data/student.model';
// import { ClassService } from 'src/app/app/features/classes/data/class.service';

// @Component({
//   selector: 'app-students-list',
//   templateUrl: './students-list.component.html',
//   styleUrls: ['./students-list.component.css']
// })
// export class StudentsListComponent implements OnInit {
//   students: StudentDto[] = [];
//   classes: any[] = [];
//   editing: StudentDto | null = null;
//   error: string | null = null;
//   message: string | null = null;

//   constructor(private svc: StudentService, private classSvc: ClassService) {}

//   ngOnInit() {
//     this.svc.getAll().subscribe({
//       next: res => this.students = res,
//       error: err => this.error = err.message
//     });

//     this.classSvc.getAll().subscribe({
//       next: res => this.classes = res,
//       error: err => this.error = err.message
//     });
//   }

//   startEdit(student: StudentDto) {
//     this.editing = { ...student }; // shallow copy
//     this.message = null;
//     this.error = null;
//   }

//   cancelEdit() {
//     this.editing = null;
//   }

//   saveEdit() {
//     if (!this.editing) return;
//     this.svc.updateByPrincipalOrIT(this.editing.studentID, this.editing as UpdateStudentRequestDto).subscribe({
//       next: _ => {
//         this.message = 'Student updated successfully.';
//         // refresh list
//         this.svc.getAll().subscribe({
//           next: res => this.students = res
//         });
//         this.editing = null;
//       },
//       error: err => this.error = err.message
//     });
//   }
// }


// import { Component, OnInit } from '@angular/core';
// import { StudentService } from '../../data/student.service';
// import { StudentDto, UpdateStudentRequestDto } from '../../data/student.model';
// import { ClassService } from 'src/app/app/features/classes/data/class.service';
// import { ClassDto } from 'src/app/app/features/classes/data/class.model';

// @Component({
//   selector: 'app-students-list',
//   templateUrl: './students-list.component.html',
//   styleUrls: ['./students-list.component.css']
// })
// export class StudentsListComponent implements OnInit {
//   students: StudentDto[] = [];
//   classes: ClassDto[] = [];
//   editing: StudentDto | null = null;
//   error: string | null = null;
//   message: string | null = null;

//   constructor(private svc: StudentService, private classSvc: ClassService) {}

//   ngOnInit() {
//     this.svc.getAll().subscribe({
//       next: res => this.students = res,
//       error: err => this.error = err.message
//     });

//     this.classSvc.getAll().subscribe({
//       next: res => this.classes = res,
//       error: err => this.error = err.message
//     });
//   }

//   startEdit(student: StudentDto) {
//     this.editing = { ...student };
//     this.message = null;
//     this.error = null;
//   }

//   cancelEdit() {
//     this.editing = null;
//   }

//   saveEdit() {
//     if (!this.editing) return;
//     this.svc.updateByPrincipalOrIT(this.editing.studentID, this.editing as UpdateStudentRequestDto).subscribe({
//       next: _ => {
//         this.message = 'Student updated successfully.';
//         this.svc.getAll().subscribe({
//           next: res => this.students = res
//         });
//         this.editing = null;
//       },
//       error: err => this.error = err.message
//     });
//   }
// }


import { Component, OnInit } from '@angular/core';
import { StudentService } from '../../data/student.service';
import { StudentDto, UpdateStudentRequestDto } from '../../data/student.model';
import { ClassService } from 'src/app/app/features/classes/data/class.service';
import { ClassDto } from 'src/app/app/features/classes/data/class.model';

@Component({
  selector: 'app-students-list',
  templateUrl: './students-list.component.html',
  styleUrls: ['./students-list.component.css']
})
export class StudentsListComponent implements OnInit {
  students: StudentDto[] = [];
  classes: ClassDto[] = [];
  editing: StudentDto | null = null;
  error: string | null = null;
  message: string | null = null;

  constructor(
    private svc: StudentService,
    private classSvc: ClassService
  ) {}

  ngOnInit() {
    this.loadStudents();
    this.loadClasses();
  }

  private loadStudents() {
    this.svc.getAll().subscribe({
      next: res => this.students = res,
      error: err => this.error = err.message
    });
  }

  private loadClasses() {
    this.classSvc.getAll().subscribe({
      next: res => this.classes = res,
      error: err => this.error = err.message
    });
  }

  startEdit(student: StudentDto) {
    this.editing = { ...student }; // shallow copy for editing
    this.message = null;
    this.error = null;
  }

  cancelEdit() {
    this.editing = null;
  }

  saveEdit() {
    if (!this.editing) return;
    this.svc.updateByPrincipalOrIT(this.editing.studentID, this.editing as UpdateStudentRequestDto).subscribe({
      next: _ => {
        this.message = 'Student updated successfully.';
        this.loadStudents(); // refresh list
        this.editing = null;
      },
      error: err => this.error = err.message
    });
  }

  deleteStudent(id: string) {
    this.svc.deleteByIT(id).subscribe({
      next: _ => {
        this.message = 'Student deleted successfully.';
        this.loadStudents(); // refresh list
      },
      error: err => this.error = err.message
    });
  }
}
