import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassStudentsByIdComponent } from './class-students-by-id.component';

describe('ClassStudentsByIdComponent', () => {
  let component: ClassStudentsByIdComponent;
  let fixture: ComponentFixture<ClassStudentsByIdComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ClassStudentsByIdComponent]
    });
    fixture = TestBed.createComponent(ClassStudentsByIdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
