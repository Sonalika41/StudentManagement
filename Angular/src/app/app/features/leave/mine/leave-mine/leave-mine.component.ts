import { Component, OnInit } from '@angular/core';
import { LeaveService } from '../../data/leave.service';

@Component({
  selector: 'app-leave-mine',
  templateUrl: './leave-mine.component.html',
  styleUrls: ['./leave-mine.component.css']
})
export class LeaveMineComponent implements OnInit {
  leaves: any[] = [];
  error: string | null = null;

  constructor(private svc: LeaveService) {}

  ngOnInit() {
    this.svc.mine().subscribe({
      next: res => this.leaves = res,
      error: err => this.error = err.message
    });
  }
}
