# 🤖 CLAUDE.md - Instruções para Assistente IA

> Arquivo de contexto e instruções para assistentes IA que trabalharão neste projeto

---

## 📋 CONTEXTO DO PROJETO

### **Nome:** Marketplace Platform

### **Objetivo:** Sistema de marketplace multi-loja com Machine Learning integrado

### **Descrição:**

Plataforma de e-commerce onde:

- **Clientes**: Compram produtos, fazem reviews com fotos, gerenciam carrinho
- **Vendedores**: Criam até 2 lojas, visualizam dashboards com métricas e previsões ML, conectam-se via feed social (tipo Twitter)
- **Admins**: Gestão completa (CRUD de usuários, lojas, produtos, permissões)

### **Diferenciais:**

- 🤖 Machine Learning (recomendação, previsão de demanda, análise de sentimento, precificação dinâmica)
- 👥 Rede social B2B entre vendedores
- 📊 Dashboards com métricas inteligentes

---

## 🏗️ DECISÕES ARQUITETURAIS

### **Backend: .NET 9 - Clean Architecture**

**Estrutura de Projetos:**

```
MarketplaceAPI/
├── Domain/              # Núcleo puro (entidades, interfaces, enums)
├── Application/         # Casos de uso (CQRS: Commands/Queries, DTOs)
├── Infrastructure/      # Implementações técnicas (DB, ML, Storage)
└── API/                 # Controllers REST
```

**Padrões Obrigatórios:**

- ✅ **Clean Architecture** (dependências sempre apontam para dentro)
- ✅ **CQRS** via MediatR (separar Commands de Queries)
- ✅ **Repository Pattern** (abstração de dados)
- ✅ **Dependency Injection** (IoC Container)
- ✅ **DTOs** para transferência de dados (nunca expor entidades diretamente)

**Regras de Dependência:**

```
API → Application + Infrastructure
Infrastructure → Application + Domain
Application → Domain
Domain → NADA (puro)
```

### **Frontend: Angular 18 + NgRx**

- Arquitetura modular com Lazy Loading
- NgRx para state management (Redux pattern)
- Separação: core/, shared/, features/

---

## 🛠️ TECH STACK

### **Backend - Versões ESPECÍFICAS**

| Pacote                                              | Versão | Projeto        |
| --------------------------------------------------- | ------ | -------------- |
| .NET SDK                                            | 9.0    | -              |
| Microsoft.EntityFrameworkCore                       | 9.0.0  | Infrastructure |
| Microsoft.EntityFrameworkCore.Design                | 9.0.0  | Infrastructure |
| Microsoft.EntityFrameworkCore.Tools                 | 9.0.0  | API            |
| Npgsql.EntityFrameworkCore.PostgreSQL               | 9.0.0  | Infrastructure |
| Microsoft.AspNetCore.Authentication.JwtBearer       | 9.0.0  | API            |
| MediatR                                             | Latest | Application    |
| AutoMapper                                          | Latest | Application    |
| AutoMapper.Extensions.Microsoft.DependencyInjection | Latest | Application    |
| FluentValidation                                    | Latest | Application    |
| FluentValidation.DependencyInjectionExtensions      | Latest | Application    |
| Microsoft.ML                                        | Latest | Infrastructure |
| Microsoft.ML.Recommender                            | Latest | Infrastructure |
| Microsoft.ML.TimeSeries                             | Latest | Infrastructure |
| StackExchange.Redis                                 | Latest | Infrastructure |
| BCrypt.Net-Next                                     | Latest | Infrastructure |
| Serilog.AspNetCore                                  | Latest | API            |
| Swashbuckle.AspNetCore                              | Latest | API            |

**⚠️ IMPORTANTE:** Pacotes da Microsoft DEVEM usar versão 9.0.0 (compatibilidade com .NET 9)

### **Comando de Instalação Correto:**

```bash
# Para pacotes Microsoft:
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0

# Outros pacotes pegam latest automaticamente:
dotnet add package MediatR
```

---

## 📐 CONVENÇÕES DE CÓDIGO

### **C# (.NET 9)**

**Namespaces (file-scoped):**

```csharp
namespace Domain.Entities;  // ✅ Correto (C# 10+)

// ❌ Evitar:
namespace Domain.Entities
{
    // ...
}
```

**Propriedades:**

```csharp
public Guid Id { get; set; }           // ✅ Auto-properties
public string? Email { get; set; }     // ✅ Nullable quando apropriado
```

**Métodos Assíncronos:**

```csharp
public async Task<Product> GetByIdAsync(Guid id)  // ✅ Sufixo Async
{
    return await _context.Products.FindAsync(id);
}
```

**Nomenclatura:**

- **Classes/Métodos/Propriedades:** PascalCase
- **Variáveis/Parâmetros:** camelCase
- **Constantes:** UPPER_SNAKE_CASE
- **Interfaces:** Prefixo `I` (IRepository, IUserService)

**Comentários:**

```csharp
/// <summary>
/// Descrição do método (XML comments para documentação)
/// </summary>
public void MyMethod() { }
```

### **Estrutura de Pastas**

**Domain:**

```
Domain/
├── Entities/        # Classes de negócio
├── Enums/          # Tipos fixos
├── ValueObjects/   # Objetos imutáveis
├── Interfaces/     # Contratos
└── Exceptions/     # Exceções customizadas
```

**Application:**

```
Application/
├── Commands/       # Write operations (Create, Update, Delete)
│   └── [Feature]/  # Ex: Products/, Orders/
├── Queries/        # Read operations (Get, List, Search)
│   └── [Feature]/
├── DTOs/           # Data Transfer Objects
├── Validators/     # FluentValidation
├── Services/       # Interfaces
├── Mappings/       # AutoMapper profiles
└── Common/         # Shared classes
```

**Infrastructure:**

```
Infrastructure/
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── Repositories/
│   ├── Configurations/      # Fluent API
│   └── Migrations/
├── MachineLearning/
├── Identity/
├── Storage/
└── Cache/
```

**API:**

```
API/
├── Controllers/
├── Middleware/
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```

---

## 🎯 ABORDAGEM DE DESENVOLVIMENTO

### **Metodologia: Opção C (Usuário Cria → IA Revisa)**

**Fluxo:**

1. ✅ IA dá desafio/instruções claras
2. ✅ Usuário escreve código
3. ✅ Usuário mostra código para IA
4. ✅ IA valida e dá feedback construtivo
5. ✅ IA sugere melhorias (se houver)
6. ✅ IA dá próximo desafio

**Princípios:**

- 🎓 **Didático**: Explicar COMO e POR QUÊ (não só o código)
- 🧩 **Incremental**: Pequenos passos, validar antes de avançar
- 💡 **Construtivo**: Feedback positivo + sugestões de melhoria
- 🚫 **Não fazer pelo usuário**: Só dar código quando explicitamente pedido

---

## 📚 MODELO DE DADOS

### **Entidades Principais (Implementação Atual):**

```
Usuario (abstract)
├── Guid Id
├── string Nome
├── string Email
├── string SenhaHash
├── DateTime CriadoEm
├── DateTime AtualizadoEm
├── StatusUsuario StatusUsuario (enum: Ativo, Inativo, Suspenso)
└── TipoUsuario TipoUsuario (enum: Admin=1, Cliente=2, Vendedor=3)
    │
    ├── Cliente : Usuario
    │   ├── string? Telefone
    │   ├── Endereco? EnderecoEntrega (Value Object)
    │   ├── ICollection<Pedido> Pedidos
    │   └── ICollection<Avaliacao> Avaliacoes
    │
    ├── Vendedor : Usuario  [📋 EM CRIAÇÃO]
    │   ├── string DocumentNumber (CPF/CNPJ)
    │   ├── decimal Rating
    │   ├── ICollection<Loja> Lojas (máx 2)
    │   ├── ICollection<Post> Posts
    │   └── ICollection<Conexao> Conexoes
    │
    └── Admin : Usuario  [📋 EM CRIAÇÃO]
        └── (Todas as permissões via Authorize)

Endereco (Value Object - Embedded)
├── string Rua
├── string Numero
├── string? Complemento
├── string Bairro
├── string Cep (formato: XXXXX-XXX)
├── string Cidade
└── string Estado (2 caracteres)

Loja
├── Guid Id
├── Guid VendedorId (FK)
├── string Nome
├── string Descricao
├── Endereco? EnderecoLoja (Value Object)
└── ICollection<Produto> Produtos

Produto
├── Guid Id
├── Guid LojaId (FK)
├── string Nome
├── decimal Preco
├── int QuantidadeEstoque
└── Navigation: Avaliacoes[], ItensCarrinho[]

Avaliacao
├── Guid Id
├── Guid ProdutoId (FK)
├── Guid ClienteId (FK)
├── int Nota (1-5)
├── string Texto
└── Navigation: FotosAvaliacao[]

Pedido
├── Guid Id
├── Guid ClienteId (FK)
├── decimal TotalPedido
├── StatusPedido Status (enum)
├── Endereco? EnderecoEntrega (Value Object)
└── Navigation: ItensPedido[]
```

### **Enums Atualizados:**

```csharp
// Domain/Enums/TipoUsuario.cs ✅ CRIADO
TipoUsuario { Admin = 1, Cliente = 2, Vendedor = 3 }

// Domain/Enums/StatusUsuario.cs ✅ CRIADO
StatusUsuario { Ativo = 1, Inativo = 2, Suspenso = 3 }

// Ainda não criados:
OrderStatus { Pendente, Pago, Processando, Enviado, Entregue, Cancelado }
PaymentStatus { Pendente, Aprovado, Rejeitado }
```

---

## 🚀 FEATURES DE MACHINE LEARNING

### **1. Recomendação de Produtos**

- **Algoritmo**: Matrix Factorization (ML.NET)
- **Service**: `IRecommendationService`
- **Input**: UserId, histórico de compras/visualizações
- **Output**: List<ProductDto> (top 10)

### **2. Previsão de Demanda**

- **Algoritmo**: SSA Time Series (ML.NET)
- **Service**: `IDemandForecastService`
- **Input**: Série temporal de vendas
- **Output**: Previsão próximos 30 dias

### **3. Análise de Sentimento**

- **Algoritmo**: Text Classification (ML.NET)
- **Service**: `ISentimentAnalysisService`
- **Input**: Texto da review
- **Output**: Score -1 a +1

### **4. Precificação Dinâmica**

- **Algoritmo**: FastTree Regression (ML.NET)
- **Service**: `IPricingService`
- **Input**: Categoria, demanda, concorrência
- **Output**: Preço sugerido

---

## ✅ ESTADO ATUAL DO PROJETO

### **Concluído:**

- [x] Definição de escopo
- [x] Escolha de tech stack
- [x] Documentação (README.md, TECH_STACK.md, ARQUITETURA_DETALHADA.md)
- [x] Setup inicial do backend (projetos criados, referências configuradas)
- [x] Instalação de pacotes NuGet
- [x] **Entidades de Domain (Fase 1)**
  - [x] TipoUsuario.cs (enum: Admin=1, Cliente=2, Vendedor=3)
  - [x] StatusUsuario.cs (enum: Ativo=1, Inativo=2, Suspenso=3)
  - [x] Usuario.cs (abstract com construtor protegido, validação de DateTime)
  - [x] Endereco.cs (Value Object com validações: CEP, Estado, etc)
  - [x] Cliente.cs (extends Usuario com Telefone, EnderecoEntrega, Pedidos, Avaliacoes)

### **Em Andamento:**

- [ ] **Entidades de Domain (Fase 2)**
  - [ ] Vendedor.cs (extends Usuario)
  - [ ] Admin.cs (extends Usuario)

### **Próximos Passos:**

1. Criar Vendedor.cs e Admin.cs
2. Criar entidades complementares (Loja, Produto, Pedido, Avaliacao, etc)
3. Criar interfaces (IUsuarioRepository, IEnderecoService, etc)
4. Configurar DbContext e Fluent API (especialmente mapping de Value Objects)
5. Criar Commands/Queries de Application (Auth, Cadastro, etc)
6. Implementar AutoMapper profiles
7. Implementar FluentValidation para DTOs
8. Configurar API (Program.cs, AuthController, UsuariosController)
9. Primeira Migration do EF Core
10. Testar endpoints (Register/Login/Cadastro)

---

## 🎯 PADRÕES ADOTADOS NESTE PROJETO

### **Herança de Usuários (Table-per-Type)**

```csharp
// ✅ Implementado
Usuario (abstrata)
├── Cliente : Usuario
├── Vendedor : Usuario
└── Admin : Usuario
```

**Benefícios:**
- Type safety em C#
- Polimorfismo natural
- EF Core mapeia automaticamente com discriminator
- Fácil validações específicas por tipo

### **Value Objects (Endereco)**

```csharp
// ✅ Implementado
public class Endereco  // SEM Guid Id próprio
{
    public string Rua { get; private set; }  // Imutável
    public string Cep { get; private set; }
    // Validações no construtor
    private Endereco() { }  // Para EF Core
    public Endereco(string rua, ...) { ValidarDados(...); }
    public override bool Equals(object? obj) { }
    public override int GetHashCode() { }
}
```

**Benefícios:**
- Nunca pode existir inválido
- Imutável (thread-safe)
- Reutilizável em Cliente, Loja, Pedido, etc
- Comparação por valor (não por referência)

### **Validações em Múltiplas Camadas**

```
Domain/Entities/
├── Constructor Validation (fail-fast, puro, sem dependências)
└── Exemplos: Endereco.Validardados(), Usuario constructor

Application/DTOs/
├── Data Annotations (documentação API)
└── FluentValidation (regras complexas)

API/
└── ModelState (resultado final antes de persistir)
```

### **Construtor Protegido em Usuario**

```csharp
// ✅ Implementado
protected Usuario()
{
    Id = Guid.NewGuid();
    CriadoEm = DateTime.UtcNow;  // Runtime, não compile-time
    AtualizadoEm = DateTime.UtcNow;
}

// Classes derivadas chamam base()
public Cliente(string nome, string email) : base()
{
    Nome = nome;
    Email = email;
    TipoUsuario = TipoUsuario.Cliente;
}
```

**Benefícios:**
- Garantia que Id é único (Guid.NewGuid())
- Garantia que datas são do momento da criação
- Encapsulamento (apenas subclasses podem criar)

---

## 🚫 O QUE NÃO FAZER

### **Erros a Evitar:**

❌ **Domain com dependências externas**

```csharp
// ❌ ERRADO - Domain não deve conhecer EF Core
public class User : DbContext { }

// ✅ CORRETO - Domain puro
public class User { }
```

❌ **Expor entidades na API**

```csharp
// ❌ ERRADO
[HttpGet]
public User GetUser() => _context.Users.Find(1);

// ✅ CORRETO
[HttpGet]
public UserDto GetUser() => _mapper.Map<UserDto>(_repo.GetById(1));
```

❌ **Validações na Controller**

```csharp
// ❌ ERRADO
[HttpPost]
public IActionResult Create(CreateProductCommand cmd)
{
    if (string.IsNullOrEmpty(cmd.Name)) return BadRequest();
    // ...
}

// ✅ CORRETO - Usar FluentValidation
public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
```

❌ **Lógica de negócio na Controller**

```csharp
// ❌ ERRADO
[HttpPost]
public IActionResult CreateOrder(CreateOrderDto dto)
{
    var total = dto.Items.Sum(i => i.Price * i.Quantity);
    var order = new Order { Total = total };
    _context.Add(order);
    _context.SaveChanges();
}

// ✅ CORRETO - Usar Handler
[HttpPost]
public async Task<IActionResult> CreateOrder(CreateOrderCommand cmd)
{
    var orderId = await _mediator.Send(cmd);
    return Ok(orderId);
}
```

❌ **Usar versões incompatíveis**

```bash
# ❌ ERRADO
dotnet add package Microsoft.EntityFrameworkCore  # Pega latest (pode não ser 9.0)

# ✅ CORRETO
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0
```

---

## 💬 ESTILO DE COMUNICAÇÃO

### **Ao Orientar o Usuário:**

✅ **Seja didático**: Explique conceitos como se fosse para iniciante
✅ **Use exemplos**: Mostre código bom vs ruim
✅ **Seja encorajador**: "Ótimo trabalho!", "Você está no caminho certo!"
✅ **Feedback construtivo**: "Está quase perfeito! Só ajustar X porque Y"
✅ **Pergunte antes de criar**: Não criar arquivos sem permissão
✅ **Divida em passos**: Não sobrecarregar com muita informação de uma vez

❌ **Evitar**:

- Criar código sem ser solicitado
- Respostas excessivamente longas sem perguntar
- Assumir conhecimento avançado
- Dar todas as respostas (deixar o usuário pensar)

### **Formato de Desafios:**

```markdown
## 🚀 DESAFIO N: [Título]

**O que criar:**

- Arquivo X em pasta Y
- Arquivo Z em pasta W

**Requisitos:**

- Propriedade A do tipo B
- Método C que retorna D

**Dicas:**

- Use padrão X
- Lembre-se de Y

**Quando terminar:**
Me mostre o código e eu valido!
```

---

## 📖 DOCUMENTOS DE REFERÊNCIA

**Já criados no projeto:**

- `README.md` - Visão geral, comandos, estrutura
- `TECH_STACK.md` - Tecnologias detalhadas com justificativas
- `ARQUITETURA_DETALHADA.md` - Explicação didática da arquitetura
- `CLAUDE.md` - Este arquivo (instruções para IA)

**Sempre referenciar esses documentos** quando o usuário tiver dúvidas sobre decisões arquiteturais.

---

## 🔄 FLUXO DE UMA FEATURE COMPLETA

### **Exemplo: Criar Produto**

**1. Domain (Entidade):**

```csharp
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    // ... outras propriedades
}
```

**2. Application (Command + Handler):**

```csharp
// Command
public class CreateProductCommand : IRequest<Guid>
{
    public string Name { get; set; }
}

// Handler
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = new Product { Name = request.Name };
        await _repository.AddAsync(product);
        return product.Id;
    }
}

// Validator
public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
```

**3. Infrastructure (Repository):**

```csharp
public class ProductRepository : IProductRepository
{
    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }
}
```

**4. API (Controller):**

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }
}
```

---

## 🎓 CONCEITOS A REFORÇAR

Sempre que relevante, explicar:

- **Clean Architecture**: Por que separar em camadas?
- **CQRS**: Por que separar Commands de Queries?
- **Repository Pattern**: Por que abstrair acesso a dados?
- **Dependency Injection**: Como funciona IoC?
- **DTOs vs Entities**: Quando usar cada um?
- **Async/Await**: Por que usar em operações I/O?
- **Nullable Reference Types**: Quando usar `?`
- **Value Objects**: Quando criar (ex: Address, Money)

---

## 📊 MÉTRICAS DE SUCESSO

**O usuário está aprendendo bem quando:**

- ✅ Cria código sem precisar de exemplo
- ✅ Entende POR QUE usar determinado padrão
- ✅ Identifica erros antes da IA apontar
- ✅ Faz perguntas relevantes
- ✅ Sugere melhorias/alternativas

---

## 🔐 SEGURANÇA

**Lembretes importantes:**

- ⚠️ NUNCA usar senhas em texto puro (sempre BCrypt)
- ⚠️ NUNCA expor `PasswordHash` em DTOs
- ⚠️ Sempre validar inputs (FluentValidation)
- ⚠️ JWT Secret deve estar em `appsettings.json` (nunca hardcoded)
- ⚠️ Usar `[Authorize]` em endpoints protegidos
- ⚠️ SQL Injection: EF Core já protege (parametrized queries)

---

## 🎯 OBJETIVO FINAL

**Criar uma aplicação:**

- ✅ **Funcional**: Endpoints testáveis, features completas
- ✅ **Bem arquitetada**: Clean Architecture, SOLID
- ✅ **Performática**: Cache, paginação, queries otimizadas
- ✅ **Segura**: JWT, validações, hash de senhas
- ✅ **Inteligente**: ML integrado (recomendações, previsões)
- ✅ **Didática**: Código comentado, fácil de entender

---

## 📝 CHECKLIST ANTES DE AVANÇAR

Antes de passar para próximo passo, garantir:

- [ ] Código compila sem erros
- [ ] Nomenclatura consistente (PascalCase, camelCase)
- [ ] Dependências corretas (Domain → nada, Application → Domain, etc)
- [ ] Interfaces em Domain/Application, implementações em Infrastructure
- [ ] DTOs para APIs (nunca expor entidades)
- [ ] Validações com FluentValidation
- [ ] Métodos assíncronos para I/O
- [ ] Comentários XML em métodos públicos

---

---

## 📝 HISTÓRICO DE ATUALIZAÇÃO

| Data | Versão | Status | O que foi feito |
|------|--------|--------|-----------------|
| 27/11/2025 | 1.1 | Domain Fase 1 ✅ | Criadas entidades: Usuario (abstract), Cliente, Endereco (Value Object). Implementadas enums: TipoUsuario, StatusUsuario |
| - | 1.0 | Setup Inicial ✅ | Definição de escopo, tech stack, documentação, setup de projetos |

**Última atualização:** 27 de Novembro de 2025
**Versão:** 1.1
**Status do Projeto:** Em desenvolvimento - Domain Fase 1 Completa ✅ / Domain Fase 2 Em Planejamento

### **Arquivos Criados Nesta Sessão:**

```
Domain/
├── Entities/
│   ├── Usuario.cs                  ✅ Abstract com construtor protegido
│   ├── Cliente.cs                  ✅ Herda Usuario, com EnderecoEntrega, Pedidos, Avaliacoes
│   └── Endereco.cs                 ✅ Value Object com validações (CEP, Estado, etc)
└── Enums/
    ├── TipoUsuario.cs              ✅ Admin=1, Cliente=2, Vendedor=3
    └── StatusUsuario.cs            ✅ Ativo=1, Inativo=2, Suspenso=3
```

### **Decisões Arquiteturais Confirmadas:**

✅ Validações no construtor (Domain puro, sem dependências)
✅ Data Annotations + FluentValidation apenas em Application/DTOs
✅ Value Objects para Endereco (imutável, sem Id, reutilizável)
✅ Herança Table-per-Type para Usuario e subclasses
✅ Namespace file-scoped (C# 10+)
✅ Nullable reference types quando apropriado
✅ XML comments em todas as propriedades públicas
