import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginComponenteComponent } from './login-componente.component';

describe('LoginComponenteComponent', () => {
  let component: LoginComponenteComponent;
  let fixture: ComponentFixture<LoginComponenteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoginComponenteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginComponenteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
