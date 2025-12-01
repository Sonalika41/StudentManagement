import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { LeaveRoutingModule } from './leave-routing.module';
import { LeaveComponent } from './leave.component';
import { LeaveApplyComponent } from 'src/app/app/features/leave/apply/leave-apply/leave-apply.component';
import { LeaveMineComponent } from 'src/app/app/features/leave/mine/leave-mine/leave-mine.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [LeaveComponent, LeaveApplyComponent, LeaveMineComponent],
  imports: [SharedModule, FormsModule, ReactiveFormsModule, LeaveRoutingModule]
})
export class LeaveModule {}
