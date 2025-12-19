# 🔧 Environments - Configuração

Esta pasta contém os arquivos de configuração de ambiente do projeto.

## 📁 Arquivos

- **environment.ts** - Configurações de DESENVOLVIMENTO (local)
- **environment.production.ts** - Configurações de PRODUÇÃO (servidor)

## 🚀 Como Usar

### Importar no código

```typescript
import { environment } from '../environments/environment';

// Usar as configurações
console.log(environment.apiUrl);        // http://localhost:5000/api (dev)
console.log(environment.production);     // false (dev)
```

### Exemplos de Uso

#### 1. Em um Service (API calls)

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private apiUrl = environment.apiUrl; // http://localhost:5000/api

  constructor(private http: HttpClient) {}

  getProducts() {
    // GET http://localhost:5000/api/products
    return this.http.get(`${this.apiUrl}/products`);
  }
}
```

#### 2. No AuthService

```typescript
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private tokenKey = environment.auth.tokenKey; // 'auth_token'

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  saveToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }
}
```

#### 3. Logs condicionais (apenas em dev)

```typescript
import { environment } from '../../environments/environment';

if (environment.logging.enableConsoleLog) {
  console.log('Debug info:', data);
}
```

#### 4. No app.config.ts (NgRx DevTools)

```typescript
import { isDevMode } from '@angular/core';
import { environment } from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    // ...
    provideStoreDevtools({
      maxAge: 25,
      logOnly: !environment.logging.enableReduxDevtools
    }),
  ]
};
```

## 🔄 Comandos

### Rodar em desenvolvimento (usa environment.ts)
```bash
npm start
# ou
ng serve
```

### Build para produção (usa environment.production.ts)
```bash
npm run build
# ou
ng build --configuration production
```

### Rodar em modo produção localmente
```bash
ng serve --configuration production
```

## ⚙️ Como Funciona

O Angular **automaticamente** troca o arquivo baseado no comando:

- **ng serve** → usa `environment.ts`
- **ng build --configuration production** → substitui `environment.ts` por `environment.production.ts`

Isso é configurado no `angular.json` através da propriedade `fileReplacements`.

## 🔐 Configurações Sensíveis

### ⚠️ NUNCA commitar:
- API keys secretas
- Tokens de serviços externos
- Credenciais de banco de dados

### ✅ Boas práticas:

1. **Para desenvolvimento local com configurações diferentes:**
   - Criar `environment.local.ts` (adicionar ao .gitignore)
   - Copiar de `environment.ts` e modificar

2. **Para produção:**
   - Usar variáveis de ambiente do servidor
   - Configurar no CI/CD (GitHub Actions, etc)

## 📝 Variáveis Disponíveis

### Principais:
- `production`: boolean - Se está em produção
- `apiUrl`: string - URL base da API
- `storageUrl`: string - URL de arquivos estáticos

### Autenticação:
- `auth.tokenKey`: Chave do localStorage
- `auth.tokenExpirationHours`: Validade do token
- `auth.autoRefreshToken`: Auto-renovar token

### Paginação:
- `pagination.defaultPageSize`: Tamanho padrão
- `pagination.pageSizeOptions`: Opções disponíveis

### Cache:
- `cache.productListCacheDuration`: Cache de produtos (min)
- `cache.userCacheDuration`: Cache de usuário (min)

### Machine Learning:
- `ml.recommendationsLimit`: Qtd. de recomendações
- `ml.recommendationsRefreshInterval`: Intervalo de atualização

### Upload:
- `upload.maxFileSizeMB`: Tamanho máximo de arquivo
- `upload.allowedImageFormats`: Formatos permitidos

### Logging:
- `logging.enableConsoleLog`: Logs no console
- `logging.enableHttpLogging`: Debug de HTTP
- `logging.enableReduxDevtools`: Redux DevTools

### Externos:
- `external.googleAnalyticsId`: ID do Google Analytics
- `external.sentryDsn`: DSN do Sentry (error tracking)

## 🎯 Próximos Passos

1. **Atualizar `apiUrl` em `environment.production.ts`** quando tiver a URL do servidor
2. **Adicionar Google Analytics ID** se for usar analytics
3. **Configurar Sentry** se for usar error tracking
4. **Criar `environment.local.ts`** se precisar de configs locais diferentes

## 📚 Documentação Angular

[Angular Environments Guide](https://angular.io/guide/build#configuring-application-environments)
