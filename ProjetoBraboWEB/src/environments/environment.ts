/**
 * Environment - DEVELOPMENT
 *
 * Configurações para ambiente de desenvolvimento local
 * Usado quando roda: ng serve ou npm start
 */
export const environment = {
  /**
   * Flag de produção - sempre false em development
   */
  production: false,

  /**
   * Nome da aplicação
   */
  appName: 'Marketplace Platform',

  /**
   * Versão da aplicação
   */
  version: '0.1.0-dev',

  /**
   * URL base da API backend
   * Development: API rodando localmente
   */
  apiUrl: 'http://localhost:5000/api',

  /**
   * URL base para uploads/arquivos estáticos
   */
  storageUrl: 'http://localhost:5000/uploads',

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
     */
    productListCacheDuration: 5,

    /**
     * Tempo de cache para dados do usuário (em minutos)
     */
    userCacheDuration: 30,
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
    recommendationsRefreshInterval: 15,
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
     * Habilitar logs detalhados no console
     */
    enableConsoleLog: true,

    /**
     * Habilitar debug de requisições HTTP
     */
    enableHttpLogging: true,

    /**
     * Habilitar Redux DevTools (NgRx)
     */
    enableReduxDevtools: true,
  },

  /**
   * URLs externas (analytics, etc)
   */
  external: {
    /**
     * Google Analytics ID (opcional)
     */
    googleAnalyticsId: '',

    /**
     * Sentry DSN para error tracking (opcional)
     */
    sentryDsn: '',
  },
};
