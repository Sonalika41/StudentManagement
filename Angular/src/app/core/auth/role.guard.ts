import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { TokenStorageService } from './token-storage.service';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
  constructor(private token: TokenStorageService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const allowedRoles = route.data['roles'] as string[] | undefined;
    const roles = this.token.getRoles();
    if (!allowedRoles || allowedRoles.some(r => roles.includes(r))) return true;
    this.router.navigate(['/auth/unauthorized']);
    return false;
  }
}
