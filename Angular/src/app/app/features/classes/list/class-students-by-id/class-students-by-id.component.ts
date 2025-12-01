import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClassService } from '../../data/class.service';
import { StudentService } from '../../../students/data/student.service';
import { AuthService } from 'src/app/app/auth/auth.service';

@Component({
  selector: 'app-class-students-by-id',
  templateUrl: './class-students-by-id.component.html',
  styleUrls: ['./class-students-by-id.component.css']
})
export class ClassStudentsByIdComponent implements OnInit {
  data: any | null = null;
  error: string | null = null;
  message: string | null = null;

  editingStudent: any | null = null;
  form: any = {};

  isIT = false;
  isPrincipal = false;

  constructor(
    private route: ActivatedRoute,
    private svc: ClassService,
    private studentSvc: StudentService,
    private auth: AuthService
  ) {}

  ngOnInit() {
    const roles = this.auth.getRoles();
    this.isIT = roles.includes('IT');
    this.isPrincipal = roles.includes('Principal');

    const id = this.route.snapshot.paramMap.get('classId')!;
    this.loadStudents(id);
  }

  loadStudents(classId: string) {
    this.svc.getStudentsByClassId(classId).subscribe({
      next: res => this.data = res,
      error: err => this.error = err.message
    });
  }

  editStudent(s: any) {
    this.editingStudent = s;
    this.form = {
      fullName: s.fullName,
      age: s.age,
      email: s.email,
      course: s.course,
      enrollmentDate: s.enrollmentDate,
      marks: s.marks,
      attendancePercentage: s.attendancePercentage,
      guardianContact: s.guardianContact,
      classId: s.classId
    };
  }

  saveStudent() {
    if (!this.editingStudent) return;

    const payload = {
      fullName: this.form.fullName ?? this.editingStudent.fullName,
      age: this.form.age ?? this.editingStudent.age,
      email: this.form.email ?? this.editingStudent.email,
      course: this.form.course ?? this.editingStudent.course,
      enrollmentDate: this.form.enrollmentDate ?? this.editingStudent.enrollmentDate,
      marks: this.form.marks ?? this.editingStudent.marks,
      attendancePercentage: this.form.attendancePercentage ?? this.editingStudent.attendancePercentage,
      guardianContact: this.form.guardianContact ?? this.editingStudent.guardianContact,
      classId: this.editingStudent.classId
    };

    this.studentSvc.updateByPrincipalOrIT(this.editingStudent.studentID, payload).subscribe({
      next: _ => {
        this.message = 'Student updated.';
        this.editingStudent = null;
        this.form = {};
        const id = this.route.snapshot.paramMap.get('classId')!;
        this.loadStudents(id);
      },
      error: err => this.error = err.message
    });
  }

  cancelEdit() {
    this.editingStudent = null;
    this.form = {};
  }
}
