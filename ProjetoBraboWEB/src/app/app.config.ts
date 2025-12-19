import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { errorInterceptor } from './core/interceptors/error.interceptor';
import { environment } from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers:
  [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient( // Ativa o HttpClient para fazer chamadas HTTP (GET, POST, PUT, DELETE)
      withInterceptors([authInterceptor, errorInterceptor]) // São "middlewares" que interceptam TODAS as requisições
      // authInterceptor: Adiciona automaticamente Authorization: Bearer TOKEN em toda requisição
      // errorInterceptor: Se a API retornar erro 401, redireciona para login automaticamente
    ),
    provideStore(), // É como ter um "banco de dados" no frontend que qualquer componente pode ler/escrever
    provideEffects(), // Ativa os Effects do NgRx (para operações assíncronas)
    provideStoreDevtools({
      maxAge: 25,
      logOnly: !environment.logging.enableReduxDevtools // Habilitado apenas em development
    }),
    provideAnimationsAsync()
  ]
};
