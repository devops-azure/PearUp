import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PearupGridComponent } from './pearup-grid.component';

describe('PearupGridComponent', () => {
  let component: PearupGridComponent;
  let fixture: ComponentFixture<PearupGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PearupGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PearupGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
