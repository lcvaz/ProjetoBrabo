import { ApplicationConfig, provideZoneChangeDetection, isDevMode } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { errorInterceptor } from './core/interceptors/error.interceptor';

export const appConfig: ApplicationConfig = {
  providers: 
  [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes), 
    provideHttpClient( // tiva o HttpClient para fazer chamadas HTTP (GET, POST, PUT, DELETE)
      withInterceptors([authInterceptor, errorInterceptor]) // São "middlewares" que interceptam TODAS as requisições
      // authInterceptor: Adiciona automaticamente Authorization: Bearer TOKEN em toda requisição
      // errorInterceptor: Se a API retornar erro 401, redireciona para login automaticamente
    ),
    provideStore(), // É como ter um "banco de dados" no frontend que qualquer componente pode ler/escrever
    provideEffects(), // Ativa os Effects do NgRx (para operações assíncronas)
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() }), // logOnly: environment.production
    provideAnimationsAsync()
  ]
};
