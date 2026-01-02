import { inject } from '@angular/core';
import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { environment } from '../../../environments/environment';

/**
 * Interceptor de Erros HTTP
 *
 * Trata erros globalmente:
 * - 401 (Unauthorized): Faz logout e redireciona para login
 * - 403 (Forbidden): Redireciona para home
 * - Outros erros: Loga no console
 */
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // Log do erro em development
      if (environment.logging.enableHttpLogging) {
        console.error('[errorInterceptor] HTTP Error:', {
          status: error.status,
          message: error.message,
          url: req.url
        });
      }

      // Trata diferentes tipos de erro
      switch (error.status) {
        case 401:
          // Token inválido ou expirado - faz logout
          if (environment.logging.enableConsoleLog) {
            console.warn('[errorInterceptor] 401 Unauthorized - Logging out');
          }
          authService.logout();
          router.navigate(['/auth/login']);
          break;

        case 403:
          // Forbidden - usuário não tem permissão
          if (environment.logging.enableConsoleLog) {
            console.warn('[errorInterceptor] 403 Forbidden - Redirecting to home');
          }
          router.navigate(['/']);
          break;

        case 404:
          // Not Found
          if (environment.logging.enableConsoleLog) {
            console.warn('[errorInterceptor] 404 Not Found:', req.url);
          }
          break;

        case 500:
        case 502:
        case 503:
          // Server errors
          if (environment.logging.enableConsoleLog) {
            console.error('[errorInterceptor] Server Error:', error.status);
          }
          break;

        case 0:
          // Network error (servidor offline, CORS, etc)
          if (environment.logging.enableConsoleLog) {
            console.error('[errorInterceptor] Network Error - Server may be offline');
          }
          break;

        default:
          // Outros erros
          if (environment.logging.enableConsoleLog) {
            console.error('[errorInterceptor] Unhandled error:', error);
          }
      }

      // Re-lança o erro para que o componente possa tratá-lo
      return throwError(() => error);
    })
  );
};
