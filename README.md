# 🛍️ Marketplace Platform

Sistema de marketplace multi-loja com Machine Learning, onde vendedores podem criar até 2 lojas, vender produtos, conectar-se com outros vendedores e ter acesso a dashboards com métricas inteligentes e previsões.

---

## 📋 Visão Geral do Projeto

### **Conceito**
Plataforma de e-commerce que permite:
- **Clientes**: Comprar produtos, fazer reviews com fotos, gerenciar carrinho
- **Vendedores**: Criar até 2 lojas, visualizar métricas/gráficos, conectar-se com outros vendedores (feed tipo Twitter), receber recomendações de ML
- **Admins**: Gestão completa de usuários, lojas, produtos e permissões

### **Diferenciais**
- 🤖 **Machine Learning** integrado para:
  - Recomendação de produtos (Collaborative Filtering)
  - Previsão de demanda (Time Series)
  - Análise de sentimento em reviews (Text Classification)
  - Precificação dinâmica (Regression)
- 👥 **Rede Social de Vendedores**: Feed para conexões e negociações B2B
- 📊 **Dashboards Inteligentes**: Métricas em tempo real com previsões

---

## 🏗️ Arquitetura

### **Backend - Clean Architecture (.NET 9)**

```
┌─────────────────────────────────────┐
│            API Layer                 │  ← Controllers REST
├─────────────────────────────────────┤
│      Infrastructure Layer            │  ← PostgreSQL, ML.NET, Redis
├─────────────────────────────────────┤
│      Application Layer               │  ← CQRS (Commands/Queries)
├─────────────────────────────────────┤
│         Domain Layer                 │  ← Entidades e Regras de Negócio
└─────────────────────────────────────┘
```

**Separação por Projetos:**
- **Domain**: Entidades, Enums, ValueObjects, Interfaces (núcleo puro)
- **Application**: Commands, Queries, DTOs, Validators (casos de uso)
- **Infrastructure**: Banco de dados, ML.NET, JWT, Storage (implementações técnicas)
- **API**: Controllers, Middleware (camada HTTP)

### **Frontend - Angular 18 + NgRx**
- Arquitetura modular com Lazy Loading
- State management com NgRx (Redux pattern)
- Angular Material para UI
- Chart.js para gráficos

---

## 🛠️ Tech Stack

### **Backend**
| Tecnologia | Versão | Uso |
|------------|--------|-----|
| .NET | 9.0 | Framework principal |
| ASP.NET Core | 9.0 | Web API |
| Entity Framework Core | 9.0 | ORM |
| PostgreSQL | 16+ | Banco de dados |
| ML.NET | 3+ | Machine Learning |
| MediatR | 12+ | CQRS pattern |
| AutoMapper | 13+ | Mapeamento DTO ↔ Entity |
| FluentValidation | 11+ | Validações |
| JWT Bearer | 9.0 | Autenticação |
| Serilog | 8+ | Logging |
| Redis | 7+ | Cache |

### **Frontend**
| Tecnologia | Versão | Uso |
|------------|--------|-----|
| Angular | 18+ | Framework SPA |
| NgRx | 18+ | State Management |
| Angular Material | 18+ | Componentes UI |
| Chart.js | 4+ | Gráficos |
| Tailwind CSS | 3+ | Styling |
| RxJS | 7.8+ | Programação reativa |

---

## 🚀 Setup Inicial do Backend

### **Pré-requisitos**
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL 16+](https://www.postgresql.org/download/)
- IDE: Visual Studio 2022, VS Code ou Rider

### **1. Criar Solution e Projetos**

```bash
# Criar pasta do projeto
mkdir MarketplaceAPI
cd MarketplaceAPI

# Criar solution
dotnet new sln -n MarketplaceAPI

# Criar os 4 projetos (camadas)
dotnet new classlib -n Domain
dotnet new classlib -n Application
dotnet new classlib -n Infrastructure
dotnet new webapi -n API

# Adicionar projetos à solution
dotnet sln add Domain/Domain.csproj
dotnet sln add Application/Application.csproj
dotnet sln add Infrastructure/Infrastructure.csproj
dotnet sln add API/API.csproj
```

### **2. Configurar Referências entre Projetos**

```bash
# API depende de Application e Infrastructure
dotnet add API/API.csproj reference Application/Application.csproj
dotnet add API/API.csproj reference Infrastructure/Infrastructure.csproj

# Infrastructure depende de Application e Domain
dotnet add Infrastructure/Infrastructure.csproj reference Application/Application.csproj
dotnet add Infrastructure/Infrastructure.csproj reference Domain/Domain.csproj

# Application depende de Domain
dotnet add Application/Application.csproj reference Domain/Domain.csproj
```

### **3. Verificar Build**

```bash
# Compilar toda a solution
dotnet build

# Se não houver erros, está tudo configurado!
```

---

## 📂 Estrutura do Projeto

```
MarketplaceAPI/
├── Domain/                          # Núcleo - Regras de Negócio
│   ├── Entities/                    # User, Product, Order, Store, etc
│   ├── Enums/                       # UserType, OrderStatus, PaymentStatus
│   ├── ValueObjects/                # Price, Address
│   ├── Interfaces/                  # IRepository, IUnitOfWork
│   ├── Exceptions/                  # Custom exceptions
│   └── Domain.csproj
│
├── Application/                     # Casos de Uso
│   ├── Commands/                    # CQRS - Write operations
│   │   ├── Products/
│   │   ├── Orders/
│   │   └── Reviews/
│   ├── Queries/                     # CQRS - Read operations
│   │   ├── Products/
│   │   ├── Dashboard/
│   │   └── Orders/
│   ├── DTOs/                        # Data Transfer Objects
│   ├── Validators/                  # FluentValidation
│   ├── Services/                    # Interfaces (IRecommendationService, etc)
│   ├── Mappings/                    # AutoMapper profiles
│   └── Application.csproj
│
├── Infrastructure/                  # Implementações Técnicas
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   ├── Repositories/
│   │   ├── Configurations/          # Fluent API
│   │   └── Migrations/
│   ├── MachineLearning/
│   │   ├── RecommendationService.cs
│   │   ├── DemandForecastService.cs
│   │   ├── SentimentAnalysisService.cs
│   │   └── PricingService.cs
│   ├── Identity/
│   │   ├── JwtService.cs
│   │   └── PasswordHasher.cs
│   ├── Storage/
│   │   └── FileStorageService.cs
│   ├── Cache/
│   │   └── RedisCacheService.cs
│   └── Infrastructure.csproj
│
├── API/                             # Web API
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── ProductsController.cs
│   │   ├── OrdersController.cs
│   │   ├── DashboardController.cs
│   │   ├── VendorFeedController.cs
│   │   └── AdminController.cs
│   ├── Middleware/
│   │   ├── ExceptionHandlerMiddleware.cs
│   │   └── LoggingMiddleware.cs
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── API.csproj
│
├── MarketplaceAPI.sln
├── .gitignore
└── README.md
```

---

## 🔧 Próximos Passos

### **Backend:**
1. ✅ Criar estrutura de projetos
2. ⏭️ Adicionar pacotes NuGet necessários
3. ⏭️ Criar estrutura de pastas
4. ⏭️ Configurar `appsettings.json`
5. ⏭️ Configurar `Program.cs` (DI, CORS, JWT, Swagger)
6. ⏭️ Criar entidades de Domain
7. ⏭️ Configurar DbContext e Migrations
8. ⏭️ Implementar primeiro endpoint (Auth)

### **Frontend:**
1. ⏭️ Setup projeto Angular 18
2. ⏭️ Configurar NgRx
3. ⏭️ Criar estrutura de módulos
4. ⏭️ Implementar autenticação

---

## 📊 Modelo de Dados (Resumido)

### **Principais Entidades:**

```
Users (abstract)
├── Customers (Cart, Orders, Reviews)
├── Vendors (Stores, Posts, Connections)
└── Admins (Permissions)

Stores (Vendor)
└── Products
    ├── Reviews (Customer + Photos)
    └── OrderItems

Orders (Customer)
└── OrderItems (Product)

Carts (Customer)
└── CartItems (Product)

VendorPosts (Feed)
VendorConnections (Vendor ↔ Vendor)
```

---

## 🤖 Features de Machine Learning

### **1. Recomendação de Produtos**
- **Algoritmo**: Matrix Factorization (Collaborative Filtering)
- **Dados**: Histórico de compras e visualizações
- **Output**: Top 10 produtos recomendados por usuário

### **2. Previsão de Demanda**
- **Algoritmo**: SSA (Singular Spectrum Analysis)
- **Dados**: Série temporal de vendas
- **Output**: Previsão de vendas para próximos 30 dias

### **3. Análise de Sentimento**
- **Algoritmo**: BERT ou FastTree (Text Classification)
- **Dados**: Texto das reviews
- **Output**: Score de -1 (negativo) a +1 (positivo)

### **4. Precificação Dinâmica**
- **Algoritmo**: FastTree Regression
- **Dados**: Categoria, demanda, concorrência, sazonalidade
- **Output**: Preço otimizado sugerido

---

## 🔐 Autenticação e Autorização

- **JWT (JSON Web Tokens)** para autenticação stateless
- **3 Roles**: Customer, Vendor, Admin
- **Claims-based authorization** em controllers
- **BCrypt** para hash de senhas (salt rounds: 12)

---

## 🎯 Padrões e Princípios

### **Design Patterns Utilizados:**
- **Clean Architecture** (separação de camadas)
- **CQRS** (Command Query Responsibility Segregation)
- **Repository Pattern** (abstração de dados)
- **Mediator Pattern** (MediatR para CQRS)
- **Dependency Injection** (IoC Container)

### **Princípios SOLID:**
- ✅ Single Responsibility Principle
- ✅ Open/Closed Principle
- ✅ Liskov Substitution Principle
- ✅ Interface Segregation Principle
- ✅ Dependency Inversion Principle

---

## 📝 Comandos Úteis

### **Rodar a API:**
```bash
cd API
dotnet run
# Ou com hot reload:
dotnet watch run
```

### **Criar Migration:**
```bash
cd Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../API
```

### **Aplicar Migration:**
```bash
dotnet ef database update --startup-project ../API
```

### **Adicionar Pacote NuGet:**
```bash
dotnet add package <PackageName>
```

### **Restaurar Dependências:**
```bash
dotnet restore
```

### **Limpar Build:**
```bash
dotnet clean
```

---

## 📖 Documentação Adicional

- [TECH_STACK.md](./TECH_STACK.md) - Tecnologias e justificativas detalhadas
- [ARQUITETURA_DETALHADA.md](./ARQUITETURA_DETALHADA.md) - Explicação didática da arquitetura

---

## 👥 Tipos de Usuário

### **Cliente (Customer)**
- ✅ Visualizar produtos
- ✅ Fazer reviews com fotos e nota (1-5)
- ✅ Adicionar ao carrinho
- ✅ Realizar compras
- ✅ Gerenciar conta

### **Vendedor (Vendor)**
- ✅ Criar até 2 lojas
- ✅ Dashboard com métricas:
  - Número de vendas
  - Receita
  - Gastos
  - Receita futura (previsão ML)
  - Produtos em alta
- ✅ Feed social (estilo Twitter) para conectar com outros vendedores
- ✅ Trocar produtos/matéria-prima com outros vendedores

### **Admin**
- ✅ CRUD completo de:
  - Usuários (clientes e vendedores)
  - Lojas
  - Produtos
  - Permissões de acesso
- ✅ Todas as permissões do sistema

---

## 🌟 Decisões de Design

### **Por que Clean Architecture?**
- Separação clara de responsabilidades
- Fácil de testar (mock de dependências)
- Independência de frameworks e tecnologias
- Escalável e manutenível

### **Por que CQRS?**
- Queries (leituras) otimizadas separadamente de Commands (escritas)
- Dashboards complexos com métricas não afetam performance de escritas
- Facilita cache em leituras

### **Por que ML.NET?**
- Integração nativa com .NET
- Performance (código compilado, não interpretado)
- Sem necessidade de APIs Python externas
- Deploy simplificado

---

## 🚧 Status do Projeto

- [x] Definição de escopo e features
- [x] Escolha de tecnologias
- [x] Documentação de arquitetura
- [x] Setup inicial do backend
- [ ] Implementação de entidades
- [ ] Configuração de banco de dados
- [ ] Implementação de autenticação
- [ ] Desenvolvimento de endpoints
- [ ] Integração de ML.NET
- [ ] Setup do frontend
- [ ] Testes

---

## 📧 Contato

Projeto desenvolvido como estudo de caso de uma aplicação full-stack moderna com Machine Learning.

---

**Última atualização:** Novembro 2025  
**Stack Principal:** .NET 9 + Angular 18 + PostgreSQL + ML.NET
