import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class TokenStorageService {
  private tokenKey = 'jwt_token';
  private rolesKey = 'roles';
  private studentIdKey = 'studentId';
  private teacherClassIdKey = 'teacherClassId';
  private emailKey = 'email';

  setLogin(payload: { token: string; roles: string[]; studentId?: string; teacherClassId?: string; email: string }) {
    localStorage.setItem(this.tokenKey, payload.token);
    localStorage.setItem(this.rolesKey, JSON.stringify(payload.roles || []));
    if (payload.studentId) localStorage.setItem(this.studentIdKey, payload.studentId);
    if (payload.teacherClassId) localStorage.setItem(this.teacherClassIdKey, payload.teacherClassId);
    localStorage.setItem(this.emailKey, payload.email);
  }

  getToken(): string | null { return localStorage.getItem(this.tokenKey); }
  getRoles(): string[] { return JSON.parse(localStorage.getItem(this.rolesKey) || '[]'); }
  getStudentId(): string | null { return localStorage.getItem(this.studentIdKey); }
  getTeacherClassId(): string | null { return localStorage.getItem(this.teacherClassIdKey); }
  getEmail(): string | null { return localStorage.getItem(this.emailKey); }

  clear() {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.rolesKey);
    localStorage.removeItem(this.studentIdKey);
    localStorage.removeItem(this.teacherClassIdKey);
    localStorage.removeItem(this.emailKey);
  }

  isLoggedIn(): boolean { return !!this.getToken(); }
}
