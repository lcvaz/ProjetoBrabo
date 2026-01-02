import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';

/**
 * Guard de Papel (Role)
 *
 * Verifica se usuário tem o papel necessário para acessar a rota
 * Usa route.data['role'] ou route.data['roles']
 */
export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Pega role(s) requerida(s) da configuração da rota
  const requiredRole = route.data['role'] as string | undefined;
  const requiredRoles = route.data['roles'] as string[] | undefined;

  // Se não especificou role, permite acesso
  if (!requiredRole && !requiredRoles) {
    return true;
  }

  // Pega role do usuário atual
  const userRole = authService.getUserRole();

  if (!userRole) {
    // Usuário não tem role (provavelmente não está autenticado)
    router.navigate(['/auth/login']);
    return false;
  }

  // Verifica se usuário tem a role necessária
  const hasRequiredRole =
    (requiredRole && userRole === requiredRole) ||
    (requiredRoles && requiredRoles.includes(userRole));

  if (hasRequiredRole) {
    return true; // Usuário tem permissão
  }

  // Usuário não tem permissão - redireciona para home
  router.navigate(['/']);
  return false;
};
