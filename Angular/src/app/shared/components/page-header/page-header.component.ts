import { Component } from '@angular/core';
import { TokenStorageService } from 'src/app/core/auth/token-storage.service';

@Component({
  selector: 'app-page-header',
  templateUrl: './page-header.component.html',
  styleUrls: ['./page-header.component.css']
})
export class PageHeaderComponent {
  constructor(public token: TokenStorageService) {}

  logout() {
    this.token.clear();
    location.href = '/auth/login';
  }
}
