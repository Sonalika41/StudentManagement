import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class AdminService {
  private base = `${environment.apiBase}/api/auth`;

  constructor(private http: HttpClient) {}

  // IT-only full users list
  getUsers() {
    return this.http.get<any[]>(`${this.base}/users`);
  }

  // IT + Principal: only teachers
  getTeachers() {
    return this.http.get<any[]>(`${this.base}/teachers`);
  }

  assignRole(userId: string, role: string) {
    return this.http.post(`${this.base}/assign-role`, { userId, role });
  }

  removeRole(userId: string, role: string) {
    return this.http.post(`${this.base}/remove-role`, { userId, role });
  }

  deleteUser(userId: string) {
    return this.http.delete(`${this.base}/user/${userId}`);
  }
}
