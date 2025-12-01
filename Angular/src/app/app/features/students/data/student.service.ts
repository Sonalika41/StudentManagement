import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { 
  CreateStudentRequestDto, 
  StudentDto, 
  TeacherUpdateStudentDto, 
  UpdateStudentRequestDto 
} from './student.model';

@Injectable({ providedIn: 'root' })
export class StudentService {
  private base = `${environment.apiBase}/api/student`;

  constructor(private http: HttpClient) {}

  // ✅ IT/Admin: fetch all students
  getAll() {
    return this.http.get<StudentDto[]>(`${this.base}`);
  }

  // ✅ IT: create student account
  createByIT(body: CreateStudentRequestDto) {
    return this.http.post<any>(`${this.base}/create-by-it`, body);
  }

  // ✅ IT/Principal: update student details
  updateByPrincipalOrIT(id: string, body: UpdateStudentRequestDto) {
    return this.http.put<any>(`${this.base}/${id}`, body);
  }

  // ✅ Teacher: limited update
  teacherUpdate(id: string, body: TeacherUpdateStudentDto) {
    return this.http.put<any>(`${this.base}/${id}/teacher-update`, body);
  }

  // ✅ IT: delete student
  deleteByIT(id: string) {
    return this.http.delete<any>(`${this.base}/${id}`);
  }

  // ✅ Student: view own profile
  me() {
    return this.http.get<StudentDto>(`${this.base}/me`);
  }

  getById(id: string) {
  return this.http.get<StudentDto>(`${this.base}/${id}`);
}

}
