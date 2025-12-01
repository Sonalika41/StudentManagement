import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeaveMineComponent } from './leave-mine.component';

describe('LeaveMineComponent', () => {
  let component: LeaveMineComponent;
  let fixture: ComponentFixture<LeaveMineComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LeaveMineComponent]
    });
    fixture = TestBed.createComponent(LeaveMineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
