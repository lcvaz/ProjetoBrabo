import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConexoesComponent } from './conexoes.component';

describe('ConexoesComponent', () => {
  let component: ConexoesComponent;
  let fixture: ComponentFixture<ConexoesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConexoesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConexoesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
