import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { StudentsRoutingModule } from './students-routing.module';
import { StudentsComponent } from './students.component';
import { StudentsListComponent } from '../list/students-list/students-list.component';
import { StudentFormComponent } from '../form/student-form/student-form.component';
import { StudentViewComponent } from '../view/student-view/student-view.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [StudentsComponent, StudentsListComponent, StudentFormComponent, StudentViewComponent],
  imports: [SharedModule, FormsModule, ReactiveFormsModule, StudentsRoutingModule]
})
export class StudentsModule {}
