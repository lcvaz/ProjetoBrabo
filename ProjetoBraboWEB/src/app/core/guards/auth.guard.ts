import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';

/**
 * Guard de Autenticação
 *
 * Protege rotas que requerem autenticação
 * Redireciona para login se não autenticado
 */
export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Verifica se está autenticado
  if (authService.isAuthenticated()) {
    return true; // Permite acessar a rota
  }

  // Não autenticado - redireciona para login
  // Salva a URL que tentou acessar para redirecionar depois do login
  router.navigate(['/auth/login'], {
    queryParams: { returnUrl: state.url }
  });

  return false;
};
