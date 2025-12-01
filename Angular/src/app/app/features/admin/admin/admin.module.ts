import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/shared/shared.module';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { UsersComponent } from 'src/app/app/features/admin/users/users/users.component';
import { DeleteConfirmDialogComponent } from 'src/app/app/features/admin/users/delete-dialog/delete-confirm-dialog/delete-confirm-dialog.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    AdminComponent,
    UsersComponent,
    DeleteConfirmDialogComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    AdminRoutingModule,
    RouterModule
  ]
})
export class AdminModule {}
