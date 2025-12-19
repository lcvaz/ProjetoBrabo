import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';


export const routes: Routes = [
    // ==========================================
    // ÁREA PRINCIPAL (HOME = CLIENTE)
    // Pública quando não logado
    // Personalizada quando logado
    // ==========================================
    {
        path: '',
        loadComponent: () => import('./layouts/cliente-layout/cliente-layout.component')
        .then(m => m.ClienteLayoutComponent),
        children: [
        {
            path: '',  // localhost:4200/ (home)
            loadComponent: () => import('./features/cliente/cliente-home/cliente-home.component')
            .then(m => m.ClienteHomeComponent)
        },
        {
            path: 'lojas',  // localhost:4200/lojas
            loadComponent: () => import('./features/cliente/lojas/lojas.component')
            .then(m => m.LojasComponent)
        },
        {
            path: 'produtos/:id',  // localhost:4200/produtos/123
            loadComponent: () => import('./features/cliente/produto-detalhes/produto-detalhes.component')
            .then(m => m.ProdutoDetalhesComponent)
        },
        
        // Rotas protegidas (precisam de login)
        {
            path: 'carrinho',
            loadComponent: () => import('./features/cliente/carrinho/carrinho.component')
            .then(m => m.CarrinhoComponent),
            canActivate: [authGuard, roleGuard],
            data: { role: 'Cliente' }
        },
        {
            path: 'meus-pedidos',
            loadComponent: () => import('./features/cliente/pedidos/pedidos.component')
            .then(m => m.PedidosComponent),
            canActivate: [authGuard, roleGuard],
            data: { role: 'Cliente' }
        },
        {
            path: 'perfil',
            loadComponent: () => import('./features/cliente/perfil/perfil.component')
            .then(m => m.PerfilComponent),
            canActivate: [authGuard, roleGuard],
            data: { role: 'Cliente' }
        },
        {
            path: 'avaliacoes',
            loadComponent: () => import('./features/cliente/avaliacoes/avaliacoes.component')
            .then(m => m.AvaliacoesComponent),
            canActivate: [authGuard, roleGuard],
            data: { role: 'Cliente' }
        }
        ]
    },

    // ==========================================
    // AUTENTICAÇÃO
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
    // ÁREA DO VENDEDOR (PROTEGIDA)
    // ==========================================
    {
        path: 'vendedor',
        loadComponent: () => import('./layouts/vendedor-layout/vendedor-layout.component')
        .then(m => m.VendedorLayoutComponent),
        canActivate: [authGuard, roleGuard],
        data: { role: 'Vendedor' },
        children: [
        {
            path: '',
            loadComponent: () => import('./features/vendedor/vendedor-home/vendedor-home.component')
            .then(m => m.VendedorHomeComponent)
        },
        {
            path: 'dashboard',
            loadComponent: () => import('./features/vendedor/dashboard/dashboard.component')
            .then(m => m.DashboardComponent)
        },
        {
            path: 'lojas',
            loadComponent: () => import('./features/vendedor/lojas/lojas.component')
            .then(m => m.LojasComponent)
        },
        {
            path: 'feed',
            loadComponent: () => import('./features/vendedor/feed/feed.component')
            .then(m => m.FeedComponent)
        },
        {
            path: 'conexoes',
            loadComponent: () => import('./features/vendedor/conexoes/conexoes.component')
            .then(m => m.ConexoesComponent)
        }
        ]
    },

    // ==========================================
    // ÁREA DO ADMIN (PROTEGIDA)
    // ==========================================
    {
        path: 'admin',
        loadComponent: () => import('./layouts/admin-layout/admin-layout.component')
        .then(m => m.AdminLayoutComponent),
        canActivate: [authGuard, roleGuard],
        data: { role: 'Admin' },
        children: [
        {
            path: '',
            loadComponent: () => import('./features/admin/admin-home/admin-home.component')
            .then(m => m.AdminHomeComponent)
        },
        {
            path: 'usuarios',
            loadComponent: () => import('./features/admin/usuarios/usuarios.component')
            .then(m => m.UsuariosComponent)
        },
        {
            path: 'lojas',
            loadComponent: () => import('./features/admin/lojas/lojas.component')
            .then(m => m.LojasComponent)
        },
        {
            path: 'relatorios',
            loadComponent: () => import('./features/admin/relatorios/relatorios.component')
            .then(m => m.RelatoriosComponent)
        }
        ]
    },

    // ==========================================
    // 404
    // ==========================================
    {
        path: '**',
        redirectTo: '/'
    }
];
