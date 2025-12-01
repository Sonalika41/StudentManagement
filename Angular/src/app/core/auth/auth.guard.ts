import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { TokenStorageService } from './token-storage.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private token: TokenStorageService, private router: Router) {}

  canActivate(): boolean {
    if (this.token.isLoggedIn()) return true;
    this.router.navigate(['/auth/unauthorized']);
    return false;
  }
}
