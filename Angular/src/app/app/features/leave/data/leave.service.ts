import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { CreateLeaveRequestDto, StudentLeaveRequestDto, UpdateLeaveStatusDto } from './leave.model';

@Injectable({ providedIn: 'root' })
export class LeaveService {
  private base = `${environment.apiBase}/api/studentleave`;

  constructor(private http: HttpClient) {}

  apply(body: CreateLeaveRequestDto) {
    return this.http.post<StudentLeaveRequestDto>(`${this.base}/apply`, body);
  }

  mine() {
    return this.http.get<StudentLeaveRequestDto[]>(`${this.base}/mine`);
  }

  updateStatus(body: UpdateLeaveStatusDto) {
    return this.http.put<StudentLeaveRequestDto>(`${this.base}/status`, body);
  }

  myClass() {
    return this.http.get<StudentLeaveRequestDto[]>(`${this.base}/my-class`);
  }
}
