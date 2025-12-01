import { Component, OnInit } from '@angular/core';
import { ClassService } from '../../data/class.service';
import { StudentService } from '../../../students/data/student.service';
import { LeaveService } from '../../../leave/data/leave.service';

@Component({
  selector: 'app-teacher-class-students',
  templateUrl: './teacher-class-students.component.html',
  styleUrls: ['./teacher-class-students.component.css']
})
export class TeacherClassStudentsComponent implements OnInit {
  data: any | null = null;
  error: string | null = null;

  // student editing
  editingStudent: any | null = null;
  updatedEmail: string = '';
  updatedMarks: number | null = null;

  // leave requests
  leaveRequests: any[] = [];

  constructor(
    private classSvc: ClassService,
    private studentSvc: StudentService,
    private leaveSvc: LeaveService
  ) {}

  ngOnInit() {
    this.loadStudents();
    this.loadLeaves();
  }

  loadStudents() {
    this.classSvc.getMyClassStudents().subscribe({
      next: res => this.data = res,
      error: err => this.error = err.message
    });
  }

  loadLeaves() {
    this.leaveSvc.myClass().subscribe({
      next: res => this.leaveRequests = res,
      error: err => this.error = err.message
    });
  }

  editStudent(student: any) {
    this.editingStudent = student;
    this.updatedEmail = student.email ?? '';
    this.updatedMarks = student.marks ?? 0;
  }

  saveStudent() {
    if (!this.editingStudent) return;
    this.studentSvc.teacherUpdate(this.editingStudent.studentID, {
      email: this.updatedEmail,
      marks: this.updatedMarks ?? 0
    }).subscribe({
      next: _ => {
        this.editingStudent = null;
        this.updatedEmail = '';
        this.updatedMarks = null;
        this.loadStudents();
      },
      error: err => this.error = err.message
    });
  }

  cancelEdit() {
    this.editingStudent = null;
    this.updatedEmail = '';
    this.updatedMarks = null;
  }

  updateLeaveStatus(id: string, status: string) {
    this.leaveSvc.updateStatus({ leaveId: id, status }).subscribe({
      next: res => {
        const req = this.leaveRequests.find(r => r.leaveId === id);
        if (req) req.status = res.status;
      },
      error: err => this.error = err.message
    });
  }
}
