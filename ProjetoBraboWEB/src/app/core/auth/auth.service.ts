import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap, catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { ApiService } from '../services/api.service';
import { User, LoginRequest, LoginResponse } from '../models/user.model';

/**
 * Serviço de Autenticação
 *
 * Gerencia login, logout e estado do usuário autenticado
 * Usa localStorage para persistir token JWT
 */
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  /**
   * Chave para armazenar token no localStorage
   */
  private readonly TOKEN_KEY = environment.auth.tokenKey;

  /**
   * Subject que mantém o usuário atual
   * BehaviorSubject sempre tem um valor (null quando não logado)
   */
  private currentUserSubject = new BehaviorSubject<User | null>(null);

  /**
   * Observable do usuário atual
   * Componentes podem se inscrever para reagir a mudanças
   */
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private apiService: ApiService,
    private router: Router
  ) {
    // Verifica se já está autenticado ao inicializar o serviço
    this.checkAuth();
  }

  /**
   * Realiza login
   * @param email - Email do usuário
   * @param password - Senha do usuário
   * @returns Observable com a resposta do login
   */
  login(email: string, password: string): Observable<LoginResponse> {
    const loginRequest: LoginRequest = { email, password };

    if (environment.logging.enableConsoleLog) {
      console.log('[AuthService] Attempting login for:', email);
    }

    return this.apiService.post<LoginResponse>('auth/login', loginRequest).pipe(
      tap(response => {
        // Salva token no localStorage
        this.saveToken(response.token);

        // Atualiza usuário atual
        this.currentUserSubject.next(response.user);

        if (environment.logging.enableConsoleLog) {
          console.log('[AuthService] Login successful:', response.user);
        }
      }),
      catchError(error => {
        if (environment.logging.enableConsoleLog) {
          console.error('[AuthService] Login failed:', error);
        }
        return throwError(() => error);
      })
    );
  }

  /**
   * Realiza logout
   * Remove token e limpa estado do usuário
   */
  logout(): void {
    if (environment.logging.enableConsoleLog) {
      console.log('[AuthService] Logging out');
    }

    // Remove token do localStorage
    localStorage.removeItem(this.TOKEN_KEY);

    // Limpa usuário atual
    this.currentUserSubject.next(null);

    // Redireciona para home
    this.router.navigate(['/']);
  }

  /**
   * Verifica se usuário está autenticado
   * @returns true se estiver autenticado e token válido
   */
  isAuthenticated(): boolean {
    const token = this.getToken();

    if (!token) {
      return false;
    }

    // Verifica se token está expirado
    if (this.isTokenExpired(token)) {
      // Token expirado, faz logout automático
      this.logout();
      return false;
    }

    return true;
  }

  /**
   * Retorna o token JWT do localStorage
   * @returns Token JWT ou null se não existir
   */
  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  /**
   * Retorna o usuário atual
   * @returns Usuário atual ou null se não estiver logado
   */
  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  /**
   * Salva token no localStorage
   * @param token - Token JWT
   */
  private saveToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  /**
   * Verifica se token JWT está expirado
   * @param token - Token JWT
   * @returns true se expirado
   */
  private isTokenExpired(token: string): boolean {
    try {
      // Decodifica payload do JWT (parte do meio entre os '.')
      const payload = JSON.parse(atob(token.split('.')[1]));

      // Verifica se tem campo 'exp' (expiration)
      if (!payload.exp) {
        return true;
      }

      // Converte para milissegundos e compara com agora
      const expirationDate = payload.exp * 1000;
      const now = Date.now();

      return now > expirationDate;
    } catch (error) {
      if (environment.logging.enableConsoleLog) {
        console.error('[AuthService] Error decoding token:', error);
      }
      return true; // Se não conseguir decodificar, considera expirado
    }
  }

  /**
   * Verifica autenticação ao inicializar o serviço
   * Se houver token válido, busca dados do usuário
   */
  private checkAuth(): void {
    if (!this.isAuthenticated()) {
      return;
    }

    // Busca dados atualizados do usuário
    this.apiService.get<User>('auth/me').subscribe({
      next: (user) => {
        this.currentUserSubject.next(user);

        if (environment.logging.enableConsoleLog) {
          console.log('[AuthService] User authenticated:', user);
        }
      },
      error: (error) => {
        if (environment.logging.enableConsoleLog) {
          console.error('[AuthService] Failed to get user data:', error);
        }
        // Se falhar ao buscar dados, faz logout
        this.logout();
      }
    });
  }

  /**
   * Extrai papel (role) do token
   * @returns UserType do usuário ou null
   */
  getUserRole(): string | null {
    const token = this.getToken();

    if (!token) {
      return null;
    }

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.role || payload.userType || null;
    } catch {
      return null;
    }
  }
}
