# 🛠️ Tech Stack - Marketplace Platform

> Sistema de marketplace com Machine Learning, múltiplas lojas e gestão de vendedores

---

## 📊 Visão Geral

```
┌─────────────────────────────────────────────────────────┐
│                      FRONTEND                            │
│              Angular 18 + NgRx + Material                │
└─────────────────────────────────────────────────────────┘
                            ↕️ REST API (JSON)
┌─────────────────────────────────────────────────────────┐
│                      BACKEND                             │
│              .NET 9 + EF Core + ML.NET                   │
└─────────────────────────────────────────────────────────┘
                            ↕️
┌─────────────────────────────────────────────────────────┐
│                    INFRAESTRUTURA                        │
│         PostgreSQL + Redis + Storage + Docker            │
└─────────────────────────────────────────────────────────┘
```

---

## 🎯 BACKEND (.NET 9)

### **Core Framework**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **.NET 9** | Latest | Performance melhorada, Native AOT, novos recursos de C# 13 |
| **ASP.NET Core Web API** | 9.0 | Framework robusto para APIs RESTful |
| **C# 13** | Latest | Últimas features de linguagem |

### **Banco de Dados & ORM**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **Entity Framework Core** | 9.0 | ORM maduro, migrations, change tracking |
| **PostgreSQL** | 16+ | Banco relacional robusto, suporta JSON, performático |
| **Npgsql.EntityFrameworkCore.PostgreSQL** | 9.0 | Provider do EF Core para PostgreSQL |
| **Redis** | Latest | Cache de métricas e sessões para performance |
| **StackExchange.Redis** | Latest | Cliente Redis para .NET |

### **Arquitetura & Patterns**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **MediatR** | 12+ | Implementação de CQRS, desacoplamento |
| **AutoMapper** | 13+ | Mapeamento entre DTOs e Entities |
| **FluentValidation** | 11+ | Validações fluentes e testáveis |

### **Autenticação & Autorização**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **Microsoft.AspNetCore.Authentication.JwtBearer** | 9.0 | Autenticação JWT stateless |
| **BCrypt.Net-Next** | Latest | Hash seguro de senhas |

### **Machine Learning**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **ML.NET** | 3+ | Framework ML da Microsoft, integração nativa com .NET |
| **Microsoft.ML** | 3+ | Core do ML.NET |
| **Microsoft.ML.Recommender** | 0.21+ | Algoritmos de recomendação (Matrix Factorization) |
| **Microsoft.ML.TimeSeries** | 3+ | Previsão de demanda (forecasting) |
| **Microsoft.ML.TensorFlow** | 3+ | Análise de sentimento com TensorFlow |

### **Logging & Monitoring**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **Serilog** | 4+ | Logging estruturado e flexível |
| **Serilog.Sinks.Console** | Latest | Output para console (desenvolvimento) |
| **Serilog.Sinks.File** | Latest | Logs em arquivo |
| **Serilog.Sinks.PostgreSQL** | Latest | Logs persistentes no banco |

### **Testing**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **xUnit** | 2.6+ | Framework de testes moderno |
| **Moq** | 4.20+ | Mocking de dependências |
| **FluentAssertions** | 6+ | Assertions legíveis |
| **Bogus** | 35+ | Geração de dados fake para testes |

### **Documentação API**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **Swashbuckle.AspNetCore** | 6.5+ | Geração de documentação Swagger/OpenAPI |
| **Swagger UI** | - | Interface interativa para testar endpoints |

### **Storage & File Upload**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **Azure.Storage.Blobs** | Latest | Storage de fotos (reviews, produtos) - Opção cloud |
| **Local FileSystem** | - | Alternativa para desenvolvimento local |

---

## 🎨 FRONTEND (Angular 18)

### **Core Framework**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **Angular** | 18+ | Framework completo, TypeScript first, signals |
| **TypeScript** | 5.4+ | Type safety, melhor DX |
| **RxJS** | 7.8+ | Programação reativa, gerenciamento de streams |

### **State Management**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **NgRx Store** | 18+ | Redux pattern, state management previsível |
| **NgRx Effects** | 18+ | Side effects (HTTP calls, async operations) |
| **NgRx Entity** | 18+ | Gerenciamento de coleções normalizadas |
| **NgRx Router Store** | 18+ | Sincronização do router com store |
| **NgRx DevTools** | 18+ | Debug e time-travel debugging |

### **UI Components & Styling**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **Angular Material** | 18+ | Componentes prontos, acessíveis, customizáveis |
| **Material Icons** | Latest | Ícones consistentes |
| **Tailwind CSS** | 3+ | Utility-first CSS, customização rápida |
| **SCSS** | - | Pré-processador CSS para componentes complexos |

### **Charts & Data Visualization**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **Chart.js** | 4+ | Gráficos interativos para dashboard vendedor |
| **ng2-charts** | 6+ | Wrapper Angular para Chart.js |

### **Form Handling**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **Reactive Forms** | - | Forms complexos, validações dinâmicas |
| **Angular Template-Driven Forms** | - | Forms simples (login, busca) |

### **HTTP & API**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **HttpClient** | Angular built-in | Requisições HTTP, interceptors |
| **RxJS Operators** | - | map, switchMap, catchError para manipular responses |

### **File Upload**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **ng2-file-upload** | Latest | Upload de fotos em reviews |
| **ngx-image-compress** | Latest | Compressão de imagens antes do upload |

### **Utilities**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **date-fns** | 3+ | Manipulação de datas (mais leve que Moment.js) |
| **lodash-es** | Latest | Utility functions (debounce, throttle, etc) |

### **Testing**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **Jasmine** | Latest | Framework de testes padrão Angular |
| **Karma** | Latest | Test runner |
| **Cypress** | 13+ | Testes E2E |
| **@testing-library/angular** | Latest | Testes focados em comportamento do usuário |

---

## 🐳 INFRAESTRUTURA & DEVOPS

### **Containerização**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **Docker** | Latest | Containerização da aplicação |
| **Docker Compose** | Latest | Orquestração local (backend + postgres + redis) |

### **Banco de Dados**
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **PostgreSQL** | 16+ | Banco relacional principal |
| **Redis** | 7+ | Cache e sessions |

### **Proxy Reverso** (Produção)
| Tecnologia | Versão | Justificativa |
|------------|--------|---------------|
| **Nginx** | Latest | Proxy reverso, servir Angular build |

---

## 🔧 FERRAMENTAS DE DESENVOLVIMENTO

### **IDE & Editores**
| Ferramenta | Justificativa |
|------------|---------------|
| **Visual Studio 2022** | IDE completa para .NET (debugging, profiling) |
| **VS Code** | Editor leve para Angular e edição geral |
| **Rider** | Alternativa JetBrains (opcional) |

### **Extensions Recomendadas (VS Code)**
- Angular Language Service
- ESLint
- Prettier
- Angular Snippets
- GitLens
- Thunder Client (testar APIs)

### **CLI Tools**
| Ferramenta | Versão | Justificativa |
|------------|--------|---------------|
| **.NET CLI** | 9.0 | Criação de projetos, migrations, publish |
| **Angular CLI** | 18+ | Geração de componentes, build, serve |
| **EF Core CLI** | 9.0 | Gerenciamento de migrations |

### **Controle de Versão**
| Ferramenta | Justificativa |
|------------|---------------|
| **Git** | Controle de versão |
| **GitHub/GitLab** | Hospedagem de código, CI/CD |

### **API Testing**
| Ferramenta | Justificativa |
|------------|---------------|
| **Postman** | Testar endpoints REST |
| **Swagger UI** | Documentação interativa da API |

---

## 📦 ESTRUTURA DE PACOTES

### **Backend NuGet Packages (resumo)**
```xml
<!-- Core -->
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.*" />

<!-- Database -->
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.*" />
<PackageReference Include="StackExchange.Redis" Version="2.7.*" />

<!-- CQRS & Patterns -->
<PackageReference Include="MediatR" Version="12.*" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="13.*" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.*" />

<!-- Machine Learning -->
<PackageReference Include="Microsoft.ML" Version="3.*" />
<PackageReference Include="Microsoft.ML.Recommender" Version="0.21.*" />
<PackageReference Include="Microsoft.ML.TimeSeries" Version="3.*" />

<!-- Auth -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.*" />
<PackageReference Include="BCrypt.Net-Next" Version="4.*" />

<!-- Logging -->
<PackageReference Include="Serilog.AspNetCore" Version="8.*" />

<!-- Testing -->
<PackageReference Include="xUnit" Version="2.6.*" />
<PackageReference Include="Moq" Version="4.20.*" />
```

### **Frontend NPM Packages (resumo)**
```json
{
  "dependencies": {
    "@angular/animations": "^18.0.0",
    "@angular/common": "^18.0.0",
    "@angular/material": "^18.0.0",
    "@ngrx/store": "^18.0.0",
    "@ngrx/effects": "^18.0.0",
    "chart.js": "^4.0.0",
    "ng2-charts": "^6.0.0",
    "date-fns": "^3.0.0",
    "tailwindcss": "^3.0.0"
  },
  "devDependencies": {
    "@angular/cli": "^18.0.0",
    "cypress": "^13.0.0",
    "eslint": "^8.0.0",
    "prettier": "^3.0.0"
  }
}
```

---

## 🎯 FEATURES DE ML.NET POR SERVIÇO

### **1. Sistema de Recomendação (Collaborative Filtering)**
- **Algoritmo**: Matrix Factorization
- **Input**: UserId, ProductId, Rating/Purchase History
- **Output**: Top 10 produtos recomendados

### **2. Previsão de Demanda (Time Series)**
- **Algoritmo**: SSA (Singular Spectrum Analysis)
- **Input**: Histórico de vendas (data + quantidade)
- **Output**: Previsão de vendas próximos 30 dias

### **3. Análise de Sentimento (Text Classification)**
- **Algoritmo**: BERT ou SDCA (Fast Tree)
- **Input**: Texto da review
- **Output**: Score -1 (negativo) a 1 (positivo)

### **4. Precificação Dinâmica (Regression)**
- **Algoritmo**: FastTree Regression
- **Input**: Categoria, demanda, concorrência, sazonalidade
- **Output**: Preço sugerido otimizado

---

## 🔐 SEGURANÇA

| Prática | Implementação |
|---------|---------------|
| **Autenticação** | JWT com refresh tokens |
| **Hash de Senhas** | BCrypt (salt rounds: 12) |
| **HTTPS** | Obrigatório em produção |
| **CORS** | Configurado para domínios específicos |
| **Rate Limiting** | Proteção contra spam/DDoS |
| **SQL Injection** | EF Core (parametrized queries) |
| **XSS** | Angular sanitization automática |
| **CSRF** | Tokens anti-forgery |

---

## 📊 PERFORMANCE

| Técnica | Implementação |
|---------|---------------|
| **Caching** | Redis para métricas/dashboards |
| **Lazy Loading** | Módulos Angular carregados sob demanda |
| **Pagination** | Backend: Skip/Take, Frontend: Virtual Scroll |
| **CDN** | Imagens e assets estáticos |
| **Compression** | Gzip/Brotli no servidor |
| **DB Indexing** | Índices em foreign keys e campos de busca |

---

## 📝 CONVENÇÕES DE CÓDIGO

### **Backend (.NET)**
- **Naming**: PascalCase para classes/métodos, camelCase para variáveis
- **Async**: Todos os métodos I/O devem ser async
- **DI**: Injeção de dependência via constructor
- **Exception Handling**: Middleware global + try-catch específico

### **Frontend (Angular)**
- **Naming**: kebab-case para arquivos, PascalCase para classes
- **Components**: Dumb components (apresentação) e Smart components (lógica)
- **Services**: Singleton no root (providedIn: 'root')
- **Observables**: Sufixo $ (ex: products$)

---

## 🚀 PRÓXIMAS ETAPAS

1. ✅ **Tech Stack Definido**
2. ⏭️ Estrutura de Pastas Backend
3. ⏭️ Estrutura de Pastas Frontend
4. ⏭️ Configuração Docker Compose
5. ⏭️ Setup inicial dos projetos
6. ⏭️ Implementação de features (sprints)

---

**Documento criado para:** Marketplace Platform  
**Data:** Novembro 2025  
**Stack Principal:** .NET 9 + Angular 18 + ML.NET + PostgreSQL
