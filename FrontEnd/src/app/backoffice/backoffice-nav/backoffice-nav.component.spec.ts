import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BackofficeNavComponent } from './backoffice-nav.component';

describe('BackofficeNavComponent', () => {
  let component: BackofficeNavComponent;
  let fixture: ComponentFixture<BackofficeNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BackofficeNavComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BackofficeNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
