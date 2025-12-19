/**
 * Environment - PRODUCTION
 *
 * Configurações para ambiente de produção
 * Usado quando roda: ng build ou ng build --configuration production
 */
export const environment = {
  /**
   * Flag de produção - sempre true em production
   */
  production: true,

  /**
   * Nome da aplicação
   */
  appName: 'Marketplace Platform',

  /**
   * Versão da aplicação
   */
  version: '0.1.0',

  /**
   * URL base da API backend
   * Production: API em servidor de produção
   *
   * ⚠️ IMPORTANTE: Trocar pela URL real do servidor de produção
   */
  apiUrl: 'https://api.marketplace.com/api',

  /**
   * URL base para uploads/arquivos estáticos
   */
  storageUrl: 'https://storage.marketplace.com/uploads',

  /**
   * Configurações de autenticação
   */
  auth: {
    /**
     * Chave para armazenar token no localStorage
     */
    tokenKey: 'auth_token',

    /**
     * Tempo de expiração do token (em horas)
     * Deve coincidir com o backend
     */
    tokenExpirationHours: 24,

    /**
     * Refresh token automático?
     */
    autoRefreshToken: true,
  },

  /**
   * Configurações de paginação
   */
  pagination: {
    defaultPageSize: 20,
    pageSizeOptions: [10, 20, 50, 100],
  },

  /**
   * Configurações de cache
   */
  cache: {
    /**
     * Tempo de cache para listas de produtos (em minutos)
     * Em produção, cache mais agressivo para economizar banda
     */
    productListCacheDuration: 15,

    /**
     * Tempo de cache para dados do usuário (em minutos)
     */
    userCacheDuration: 60,
  },

  /**
   * Configurações de Machine Learning
   */
  ml: {
    /**
     * Número de produtos recomendados a mostrar
     */
    recommendationsLimit: 10,

    /**
     * Atualizar recomendações a cada X minutos
     */
    recommendationsRefreshInterval: 30,
  },

  /**
   * Configurações de upload de arquivos
   */
  upload: {
    /**
     * Tamanho máximo de upload (em MB)
     */
    maxFileSizeMB: 5,

    /**
     * Formatos de imagem permitidos
     */
    allowedImageFormats: ['image/jpeg', 'image/png', 'image/jpg', 'image/webp'],
  },

  /**
   * Configurações de logs e debugging
   */
  logging: {
    /**
     * Desabilitar logs detalhados em produção
     */
    enableConsoleLog: false,

    /**
     * Desabilitar debug de requisições HTTP em produção
     */
    enableHttpLogging: false,

    /**
     * Desabilitar Redux DevTools em produção
     */
    enableReduxDevtools: false,
  },

  /**
   * URLs externas (analytics, etc)
   */
  external: {
    /**
     * Google Analytics ID
     * ⚠️ Adicionar ID real quando configurar analytics
     */
    googleAnalyticsId: 'G-XXXXXXXXXX',

    /**
     * Sentry DSN para error tracking
     * ⚠️ Adicionar DSN real quando configurar Sentry
     */
    sentryDsn: 'https://xxxxx@sentry.io/xxxxx',
  },
};
