import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import {
  AssignClassTeacherDto,
  ClassDto,
  CreateClassRequestDto,
  UpdateClassRequestDto
} from './class.model';

@Injectable({ providedIn: 'root' })
export class ClassService {
  private base = `${environment.apiBase}/api/class`;

  constructor(private http: HttpClient) {}

  // IT, Principal, Teacher — view all classes
  getAll() {
    return this.http.get<ClassDto[]>(`${this.base}`);
  }

  // IT only — create new class
  create(body: CreateClassRequestDto) {
    return this.http.post<any>(`${this.base}`, body);
  }

  // IT + Principal — update name, section, teacherId
  update(id: string, body: UpdateClassRequestDto) {
    return this.http.put<any>(`${this.base}/${id}`, body);
  }

  // IT only — delete class
  delete(id: string) {
    return this.http.delete<any>(`${this.base}/${id}`);
  }

  // IT + Principal — assign/reassign class teacher
  assignTeacher(body: AssignClassTeacherDto) {
    return this.http.post<any>(`${this.base}/assign-teacher`, body);
  }

  // Teacher only — get students of their own class
  getMyClassStudents() {
    return this.http.get<any>(`${this.base}/my-class/students`);
  }

  // IT + Principal — get students of any class by classId
  getStudentsByClassId(classId: string) {
    return this.http.get<any>(`${this.base}/${classId}/students`);
  }
}
