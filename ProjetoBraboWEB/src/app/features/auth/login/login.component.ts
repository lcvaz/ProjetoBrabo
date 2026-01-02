import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../../core/auth/auth.service';
import { UserType } from '../../../core/models/user.model';

/**
 * Componente de Login
 *
 * Formulário de autenticação com validação
 * Redireciona para home apropriada após login bem-sucedido
 */
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  /**
   * Formulário reativo de login
   */
  loginForm!: FormGroup;

  /**
   * Flag de loading durante o login
   */
  isLoading = false;

  /**
   * Mensagem de erro
   */
  errorMessage = '';

  /**
   * Flag para mostrar/esconder senha
   */
  hidePassword = true;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Inicializa o formulário com validações
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  /**
   * Submete o formulário de login
   */
  onSubmit(): void {
    // Valida formulário
    if (this.loginForm.invalid) {
      this.markFormGroupTouched(this.loginForm);
      return;
    }

    // Reseta mensagem de erro
    this.errorMessage = '';
    this.isLoading = true;

    const { email, password } = this.loginForm.value;

    this.authService.login(email, password).subscribe({
      next: (response) => {
        this.isLoading = false;

        // Redireciona baseado no tipo de usuário
        this.redirectAfterLogin(response.user.userType);
      },
      error: (error) => {
        this.isLoading = false;

        // Trata erro de autenticação
        if (error.status === 401) {
          this.errorMessage = 'Email ou senha inválidos';
        } else if (error.status === 0) {
          this.errorMessage = 'Não foi possível conectar ao servidor. Verifique sua conexão.';
        } else {
          this.errorMessage = error.error?.message || 'Erro ao fazer login. Tente novamente.';
        }
      }
    });
  }

  /**
   * Redireciona usuário para home apropriada baseado no tipo
   * @param userType - Tipo do usuário
   */
  private redirectAfterLogin(userType: UserType): void {
    switch (userType) {
      case UserType.Customer:
        this.router.navigate(['/']);
        break;
      case UserType.Vendor:
        this.router.navigate(['/vendedor']);
        break;
      case UserType.Admin:
        this.router.navigate(['/admin']);
        break;
      default:
        this.router.navigate(['/']);
    }
  }

  /**
   * Marca todos os campos do formulário como touched
   * para mostrar mensagens de validação
   */
  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }

  /**
   * Retorna mensagem de erro para campo de email
   */
  getEmailErrorMessage(): string {
    const emailControl = this.loginForm.get('email');

    if (emailControl?.hasError('required')) {
      return 'Email é obrigatório';
    }

    if (emailControl?.hasError('email')) {
      return 'Email inválido';
    }

    return '';
  }

  /**
   * Retorna mensagem de erro para campo de senha
   */
  getPasswordErrorMessage(): string {
    const passwordControl = this.loginForm.get('password');

    if (passwordControl?.hasError('required')) {
      return 'Senha é obrigatória';
    }

    if (passwordControl?.hasError('minlength')) {
      return 'Senha deve ter no mínimo 6 caracteres';
    }

    return '';
  }
}
