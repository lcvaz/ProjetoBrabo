import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../auth/auth.service';
import { environment } from '../../../environments/environment';

/**
 * Interceptor de Autenticação
 *
 * Adiciona automaticamente o token JWT no header Authorization
 * de todas as requisições HTTP
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  // Pega o token do AuthService
  const token = authService.getToken();

  // Se não tem token, passa a requisição sem modificar
  if (!token) {
    return next(req);
  }

  // Clona a requisição e adiciona header Authorization
  const authReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });

  // Log apenas em development
  if (environment.logging.enableHttpLogging) {
    console.log('[authInterceptor] Added token to request:', req.url);
  }

  // Passa a requisição modificada adiante
  return next(authReq);
};
