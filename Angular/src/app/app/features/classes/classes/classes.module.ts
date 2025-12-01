import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { ClassesRoutingModule } from './classes-routing.module';
import { ClassesComponent } from './classes.component';
import { ClassesListComponent } from '../list/classes-list/classes-list.component';
import { ClassFormComponent } from '../form/class-form/class-form.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TeacherClassStudentsComponent } from '../list/teacher-class-students/teacher-class-students.component';
import { ClassStudentsByIdComponent } from '../list/class-students-by-id/class-students-by-id.component';


@NgModule({
  declarations: [
    ClassesComponent,
    ClassesListComponent,
    ClassFormComponent,
    TeacherClassStudentsComponent,
    ClassStudentsByIdComponent
  ],
  imports: [SharedModule, FormsModule, ReactiveFormsModule, ClassesRoutingModule]
})
export class ClassesModule {}
