import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { jwtDecode } from 'jwt-decode';

export interface LoginResponseDto {
  email: string;
  roles: string[];
  token: string;
  studentID?: string;
  teacherClassId?: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private base = `${environment.apiBase}/api/auth`;

  constructor(private http: HttpClient) {}

  login(email: string, password: string) {
    return this.http.post<LoginResponseDto>(`${this.base}/login`, { email, password });
  }

  register(body: { email: string; password: string; role: string; fullName?: string }) {
    return this.http.post<any>(`${this.base}/register`, body);
  }

  // Prefer stored roles; fallback to decode if missing
  getRoles(): string[] {
    const stored = JSON.parse(localStorage.getItem('roles') || '[]');
    if (stored?.length) return stored;

    const token = localStorage.getItem('jwt_token') || localStorage.getItem('token'); // handle both keys
    if (!token) return [];
    try {
      const decoded: any = jwtDecode(token);
      const claim =
        decoded.role ||
        decoded.roles ||
        decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return Array.isArray(claim) ? claim : claim ? [claim] : [];
    } catch {
      return [];
    }
  }

  hasRole(role: string): boolean {
    return this.getRoles().includes(role);
  }
}
