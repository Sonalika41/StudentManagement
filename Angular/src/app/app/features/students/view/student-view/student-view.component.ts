import { Component, OnInit } from '@angular/core';
import { StudentService } from '../../data/student.service';
import { StudentDto } from '../../data/student.model';

@Component({
  selector: 'app-student-view',
  templateUrl: './student-view.component.html',
  styleUrls: ['./student-view.component.css']
})
export class StudentViewComponent implements OnInit {
  student: StudentDto | null = null;
  error: string | null = null;

  constructor(private svc: StudentService) {}

  ngOnInit() {
    this.svc.me().subscribe({
      next: res => this.student = res,
      error: err => this.error = err.message
    });
  }
}
