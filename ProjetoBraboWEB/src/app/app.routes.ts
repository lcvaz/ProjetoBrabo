import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';


export const routes: Routes = [
    {
        path: '',  // localhost:4200/
        loadComponent: () => import('./layouts/public-layout/public-layout.component')
        .then(m => m.PublicLayoutComponent),
        children: [
        {
            path: '',  // localhost:4200/ (home page)
            loadComponent: () => import('./features/public/home/home.component')
            .then(m => m.HomeComponent)
        },
        {
            path: 'lojas',  // localhost:4200/lojas
            loadComponent: () => import('./features/public/lojas/lojas.component')
            .then(m => m.LojasComponent)
        },
        {
            path: 'produtos/:id',  // localhost:4200/produtos/123
            loadComponent: () => import('./features/public/produto-detalhes/produto-detalhes.component')
            .then(m => m.ProdutoDetalhesComponent)
        }
        ]
    },

    // ==========================================
    // AUTENTICAÇÃO (PÚBLICO)
    // ==========================================
    {
        path: 'auth',
        loadComponent: () => import('./layouts/auth-layout/auth-layout.component')
        .then(m => m.AuthLayoutComponent),
        children: [
        {
            path: 'login',
            loadComponent: () => import('./features/auth/login/login.component')
            .then(m => m.LoginComponent)
        },
        {
            path: 'register',
            loadComponent: () => import('./features/auth/register/register.component')
            .then(m => m.RegisterComponent)
        }
        ]
    },

    // ==========================================
    // ÁREA DO CLIENTE (PROTEGIDA)
    // ==========================================
    {
        path: 'minha-conta',  // 👈 Mudei de 'cliente' para 'minha-conta'
        loadComponent: () => import('./layouts/cliente-layout/cliente-layout.component')
        .then(m => m.ClienteLayoutComponent),
        canActivate: [authGuard, roleGuard],
        data: { role: 'Cliente' },
        children: [
        {
            path: 'pedidos',
            loadComponent: () => import('./features/cliente/pedidos/pedidos.component')
            .then(m => m.PedidosComponent)
        },
        {
            path: 'perfil',
            loadComponent: () => import('./features/cliente/perfil/perfil.component')
            .then(m => m.PerfilComponent)
        },
        {
            path: 'avaliacoes',
            loadComponent: () => import('./features/cliente/avaliacoes/avaliacoes.component')
            .then(m => m.AvaliacoesComponent)
        }
        ]
    },

    // ==========================================
  // ÁREA DO VENDEDOR (AUTENTICADO)
  // ==========================================
  {
    path: 'vendedor',  // localhost:4200/vendedor
    
    // Lazy loading do layout do vendedor
    // Este layout tem: sidebar com menu, header com notificações
    loadComponent: () => import('./layouts/vendedor-layout/vendedor-layout.component')
      .then(m => m.VendedorLayoutComponent),
    
    // ✅ Guards: Só vendedores autenticados
    canActivate: [authGuard, roleGuard],
    data: { role: 'Vendedor' },
    
    children: [
      {
        path: '',  // localhost:4200/vendedor
        loadComponent: () => import('./features/vendedor/vendedor-home/vendedor-home.component')
          .then(m => m.VendedorHomeComponent)
      },
      {
        path: 'dashboard',  // localhost:4200/vendedor/dashboard
        loadComponent: () => import('./features/vendedor/dashboard/dashboard.component')
          .then(m => m.DashboardComponent)
      },
      {
        path: 'lojas',  // localhost:4200/vendedor/lojas
        loadComponent: () => import('./features/vendedor/lojas/lojas.component')
          .then(m => m.LojasComponent)
      },
      {
        path: 'feed',  // localhost:4200/vendedor/feed
        loadComponent: () => import('./features/vendedor/feed/feed.component')
          .then(m => m.FeedComponent)
      },
      {
        path: 'conexoes',  // localhost:4200/vendedor/conexoes
        loadComponent: () => import('./features/vendedor/conexoes/conexoes.component')
          .then(m => m.ConexoesComponent)
      }
    ]
  },

  // ==========================================
  // ÁREA DO ADMIN (AUTENTICADO)
  // ==========================================
  {
    path: 'admin',  // localhost:4200/admin
    
    // Lazy loading do layout do admin
    // Este layout tem: sidebar com menu admin, tabelas, etc
    loadComponent: () => import('./layouts/admin-layout/admin-layout.component')
      .then(m => m.AdminLayoutComponent),
    
    // ✅ Guards: Só admins autenticados
    canActivate: [authGuard, roleGuard],
    data: { role: 'Admin' },
    
    children: [
      {
        path: '',  // localhost:4200/admin
        loadComponent: () => import('./features/admin/admin-home/admin-home.component')
          .then(m => m.AdminHomeComponent)
      },
      {
        path: 'usuarios',  // localhost:4200/admin/usuarios
        loadComponent: () => import('./features/admin/usuarios/usuarios.component')
          .then(m => m.UsuariosComponent)
      },
      {
        path: 'lojas',  // localhost:4200/admin/lojas
        loadComponent: () => import('./features/admin/lojas/lojas.component')
          .then(m => m.LojasComponent)
      },
      {
        path: 'relatorios',  // localhost:4200/admin/relatorios
        loadComponent: () => import('./features/admin/relatorios/relatorios.component')
          .then(m => m.RelatoriosComponent)
      }
    ]
  },

  // ==========================================
  // 404 - ROTA NÃO ENCONTRADA
  // ==========================================
  {
    path: '**',  // Qualquer URL que não foi definida acima
    redirectTo: '/'  // Redireciona para login
  }



];
