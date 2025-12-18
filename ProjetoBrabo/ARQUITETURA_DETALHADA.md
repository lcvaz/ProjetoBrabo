# 🏗️ Arquitetura e Estrutura de Pastas - Guia Completo

> Explicação detalhada da estrutura do projeto Marketplace para iniciantes em .NET e Angular

---

## 📚 Índice

1. [Conceitos Fundamentais](#conceitos-fundamentais)
2. [Backend - Clean Architecture](#backend-clean-architecture)
3. [Frontend - Arquitetura Modular](#frontend-arquitetura-modular)
4. [Fluxo Completo de Requisição](#fluxo-completo-de-requisição)
5. [Dúvidas Comuns](#dúvidas-comuns)

---

## 🎯 Conceitos Fundamentais

### **Clean Architecture - Separação de Responsabilidades**

A Clean Architecture organiza código em **camadas concêntricas** onde cada camada tem uma responsabilidade específica:

```
┌─────────────────────────────────────┐
│         API (Camada Externa)         │  ← Controllers HTTP
├─────────────────────────────────────┤
│     Infrastructure (Técnica)         │  ← Banco, ML, Storage
├─────────────────────────────────────┤
│   Application (Casos de Uso)        │  ← Lógica de aplicação
├─────────────────────────────────────┤
│      Domain (Coração/Núcleo)        │  ← Regras de negócio puras
└─────────────────────────────────────┘
```

**🔑 Regra de Ouro:** Camadas internas **NUNCA** conhecem camadas externas.

**Por quê?**
- **Domain** não sabe que existe PostgreSQL, ele só conhece conceitos de negócio
- **Application** não sabe se a API é REST ou GraphQL
- Isso permite **trocar** tecnologias sem quebrar tudo

**Analogia:** Pense em uma cebola 🧅
- O núcleo (Domain) é protegido pelas camadas externas
- Cada camada só pode "ver" para dentro, nunca para fora

---

### **CQRS - Command Query Responsibility Segregation**

Separar operações que **alteram** dados das que apenas **leem** dados.

```
┌─────────────────┐         ┌─────────────────┐
│   COMMANDS      │         │    QUERIES      │
│  (Escrita)      │         │   (Leitura)     │
├─────────────────┤         ├─────────────────┤
│ CreateProduct   │         │ GetProducts     │
│ UpdateOrder     │         │ GetDashboard    │
│ DeleteReview    │         │ SearchStores    │
└─────────────────┘         └─────────────────┘
       ↓                            ↓
  Validações Complexas        Otimizado p/ leitura
  Regras de Negócio          Cache, Paginação
```

**Por quê CQRS?**
- **Leituras** (90% das operações) ficam **simples e rápidas**
- **Escritas** têm **validações complexas** isoladas
- Cada lado pode ser otimizado independentemente

---

## 🏢 BACKEND (.NET 9) - Clean Architecture

### **Visão Geral da Estrutura**

```
MarketplaceAPI/
├── Domain/              # 🧠 Núcleo do negócio
├── Application/         # 📋 Casos de uso
├── Infrastructure/      # 🔧 Implementações técnicas
└── API/                 # 🌐 Controllers HTTP
```

---

## 1️⃣ DOMAIN - O Coração do Sistema

**📂 Localização:** `MarketplaceAPI/Domain/`

### **Responsabilidade:**
Contém as **regras de negócio puras**, independentes de tecnologia. Não sabe o que é banco de dados, API REST ou Angular.

### **Estrutura Completa:**

```
Domain/
├── Entities/           # Classes principais do negócio
├── Enums/             # Tipos fixos (Status, Tipos de Usuário)
├── ValueObjects/      # Objetos imutáveis
├── Interfaces/        # Contratos para outras camadas
└── Exceptions/        # Exceções customizadas
```

---

### **📁 Entities/ - Entidades de Domínio**

**O que são?** Classes que representam conceitos centrais do negócio.

```
Entities/
├── User.cs            # Classe base abstrata
├── Customer.cs        # Cliente (herda de User)
├── Vendor.cs          # Vendedor (herda de User)
├── Admin.cs           # Administrador (herda de User)
├── Store.cs           # Loja do vendedor
├── Product.cs         # Produto
├── Review.cs          # Avaliação de produto
├── ReviewPhoto.cs     # Foto da review
├── Order.cs           # Pedido
├── OrderItem.cs       # Item do pedido
├── Cart.cs            # Carrinho
├── CartItem.cs        # Item do carrinho
├── VendorPost.cs      # Post no feed de vendedores
└── VendorConnection.cs # Conexão entre vendedores
```

#### **Exemplo Prático: User.cs**

```csharp
namespace Domain.Entities;

/// <summary>
/// Classe base abstrata para todos os tipos de usuários
/// Abstrata = não pode ser instanciada diretamente, apenas suas filhas
/// </summary>
public abstract class User
{
    public Guid Id { get; set; }           // Identificador único
    public string Email { get; set; }      // Email para login
    public string PasswordHash { get; set; } // Senha em hash (nunca texto puro!)
    public UserType UserType { get; set; } // Enum: Customer, Vendor, Admin
    public DateTime CreatedAt { get; set; } // Quando foi criado
    public DateTime? UpdatedAt { get; set; } // ? = nullable (pode ser null)
    public bool IsActive { get; set; }     // Usuário ativo ou bloqueado?
    
    // Método de domínio - regra de negócio
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
```

**Por que User é abstrata?**
- Não existe um "usuário genérico", sempre é Cliente, Vendedor ou Admin
- Força você a sempre especificar o tipo
- Compartilha propriedades comuns (Email, Senha, etc)

#### **Exemplo: Customer.cs (herda de User)**

```csharp
namespace Domain.Entities;

/// <summary>
/// Cliente - pode comprar produtos, fazer reviews, ter carrinho
/// </summary>
public class Customer : User
{
    public string? PhoneNumber { get; set; }
    public Address? ShippingAddress { get; set; } // ValueObject
    
    // Relacionamentos (Navigation Properties no EF Core)
    public Cart Cart { get; set; }                    // 1 cliente tem 1 carrinho
    public ICollection<Order> Orders { get; set; }    // 1 cliente tem N pedidos
    public ICollection<Review> Reviews { get; set; }  // 1 cliente tem N reviews
    
    public Customer()
    {
        UserType = UserType.Customer;  // Define tipo automaticamente
        Orders = new List<Order>();
        Reviews = new List<Review>();
    }
}
```

#### **Exemplo: Vendor.cs**

```csharp
namespace Domain.Entities;

/// <summary>
/// Vendedor - pode ter até 2 lojas, fazer posts, conectar com outros vendedores
/// </summary>
public class Vendor : User
{
    public string DocumentNumber { get; set; } // CPF/CNPJ
    public decimal Rating { get; set; }        // Avaliação média do vendedor
    
    // Relacionamentos
    public ICollection<Store> Stores { get; set; }  // Máximo 2 lojas
    public ICollection<VendorPost> Posts { get; set; }
    public ICollection<VendorConnection> Connections { get; set; }
    
    public Vendor()
    {
        UserType = UserType.Vendor;
        Stores = new List<Store>();
        Posts = new List<VendorPost>();
        Connections = new List<VendorConnection>();
    }
    
    // Regra de negócio no domínio
    public bool CanCreateStore()
    {
        return Stores.Count < 2;  // Máximo 2 lojas
    }
}
```

#### **Exemplo: Product.cs**

```csharp
namespace Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Category { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Relacionamentos
    public Store Store { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    
    // Propriedade calculada (não salva no banco)
    public decimal AverageRating => 
        Reviews.Any() ? Reviews.Average(r => r.Rating) : 0;
    
    // Regras de negócio
    public bool IsInStock() => StockQuantity > 0;
    
    public void DecreaseStock(int quantity)
    {
        if (quantity > StockQuantity)
            throw new InvalidOperationException("Estoque insuficiente");
            
        StockQuantity -= quantity;
    }
}
```

---

### **📁 Enums/ - Tipos Fixos**

**O que são?** Valores predefinidos que um campo pode ter.

```
Enums/
├── UserType.cs
├── OrderStatus.cs
├── PaymentStatus.cs
└── ConnectionType.cs
```

#### **Exemplo: OrderStatus.cs**

```csharp
namespace Domain.Enums;

/// <summary>
/// Status possíveis de um pedido
/// </summary>
public enum OrderStatus
{
    Pending = 1,      // Aguardando pagamento
    Paid = 2,         // Pago
    Processing = 3,   // Separando produtos
    Shipped = 4,      // Enviado
    Delivered = 5,    // Entregue
    Cancelled = 6     // Cancelado
}
```

**Por que Enum?**
- Evita erros de digitação (não pode escrever "Pago" errado)
- IntelliSense ajuda (mostra opções)
- Banco salva como número (mais eficiente)

---

### **📁 ValueObjects/ - Objetos Imutáveis**

**O que são?** Objetos que representam um **valor completo** e não mudam depois de criados.

```
ValueObjects/
├── Price.cs
├── Address.cs
└── Money.cs
```

#### **Exemplo: Address.cs**

```csharp
namespace Domain.ValueObjects;

/// <summary>
/// Endereço completo - tratado como um bloco indivisível
/// Record = imutável por padrão no C#
/// </summary>
public record Address
{
    public string Street { get; init; }     // init = só pode setar no construtor
    public string Number { get; init; }
    public string? Complement { get; init; }
    public string Neighborhood { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string ZipCode { get; init; }
    public string Country { get; init; }
    
    // Construtor garante que endereço sempre é válido
    public Address(string street, string number, string city, string state, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Rua é obrigatória");
            
        Street = street;
        Number = number;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = "Brasil";
    }
    
    // Método para formatar
    public string ToFormattedString() =>
        $"{Street}, {Number} - {Neighborhood}, {City}/{State} - CEP: {ZipCode}";
}
```

**Por que ValueObject?**
- Endereço não é só "rua" ou "cidade", é um **conceito completo**
- Se mudar uma parte, o endereço inteiro muda (imutável)
- Evita campos soltos espalhados pelo código

---

### **📁 Interfaces/ - Contratos**

**O que são?** Definem **O QUÊ** precisa ser feito, mas não **COMO**.

```
Interfaces/
├── IRepository.cs           # Base para repositórios
├── IProductRepository.cs
├── IOrderRepository.cs
├── IUnitOfWork.cs          # Transações do banco
├── IEmailService.cs        # Enviar emails
└── IStorageService.cs      # Upload de arquivos
```

#### **Exemplo: IProductRepository.cs**

```csharp
namespace Domain.Interfaces;

/// <summary>
/// Contrato: qualquer repositório de produtos DEVE implementar isso
/// Domain só diz O QUÊ precisa, Infrastructure implementa COMO
/// </summary>
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetByStoreIdAsync(Guid storeId);
    Task<IEnumerable<Product>> SearchAsync(string searchTerm);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid id);
}
```

**Por que Interface?**
- Domain não sabe se é PostgreSQL, MongoDB ou arquivo txt
- Permite **trocar** implementação sem mexer em Domain
- Facilita **testes** (pode mockar)

---

## 2️⃣ APPLICATION - Casos de Uso

**📂 Localização:** `MarketplaceAPI/Application/`

### **Responsabilidade:**
Orquestra a lógica de aplicação. Usa as entidades de Domain e coordena ações.

### **Estrutura Completa:**

```
Application/
├── Commands/           # Ações que ALTERAM dados (CQRS - Write)
├── Queries/           # Ações que LEEM dados (CQRS - Read)
├── DTOs/              # Objetos de transferência
├── Validators/        # Validações de regras
├── Services/          # Interfaces de serviços
├── Mappings/          # AutoMapper profiles
└── Common/            # Classes base compartilhadas
```

---

### **📁 Commands/ - Comandos de Escrita**

**O que são?** Ações que **modificam** o estado do sistema.

```
Commands/
├── Products/
│   ├── CreateProduct/
│   │   ├── CreateProductCommand.cs        # Dados do comando
│   │   ├── CreateProductCommandHandler.cs # Executa o comando
│   │   └── CreateProductCommandValidator.cs # Valida antes de executar
│   ├── UpdateProduct/
│   └── DeleteProduct/
├── Orders/
│   ├── CreateOrder/
│   └── CancelOrder/
└── Reviews/
    ├── CreateReview/
    └── DeleteReview/
```

#### **Exemplo: CreateProductCommand.cs**

```csharp
namespace Application.Commands.Products.CreateProduct;

/// <summary>
/// Command = objeto que carrega os dados necessários para criar um produto
/// IRequest<Guid> = retorna o Id do produto criado
/// </summary>
public class CreateProductCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Category { get; set; }
    public Guid StoreId { get; set; }
    public IFormFile? Image { get; set; }  // Foto do produto (opcional)
}
```

#### **Exemplo: CreateProductCommandHandler.cs**

```csharp
namespace Application.Commands.Products.CreateProduct;

/// <summary>
/// Handler = executa o comando
/// Recebe CreateProductCommand e retorna Guid
/// </summary>
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;
    
    // Injeção de dependência no construtor
    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IStoreRepository storeRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _storeRepository = storeRepository;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // 1. Verificar se a loja existe
        var store = await _storeRepository.GetByIdAsync(request.StoreId);
        if (store == null)
            throw new NotFoundException("Loja não encontrada");
        
        // 2. Upload da imagem (se houver)
        string? imageUrl = null;
        if (request.Image != null)
        {
            imageUrl = await _storageService.UploadAsync(request.Image, "products");
        }
        
        // 3. Criar a entidade Product
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            Category = request.Category,
            StoreId = request.StoreId,
            ImageUrl = imageUrl,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        
        // 4. Adicionar no repositório
        await _productRepository.AddAsync(product);
        
        // 5. Salvar no banco (commit da transação)
        await _unitOfWork.CommitAsync(cancellationToken);
        
        // 6. Retornar o Id do produto criado
        return product.Id;
    }
}
```

**Fluxo explicado:**
1. Handler recebe o comando
2. Valida regras de negócio (loja existe?)
3. Faz upload de arquivo (se houver)
4. Cria a entidade
5. Salva no banco
6. Retorna resultado

#### **Exemplo: CreateProductCommandValidator.cs**

```csharp
namespace Application.Commands.Products.CreateProduct;

/// <summary>
/// Validador usando FluentValidation
/// Executa ANTES do Handler
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição é obrigatória")
            .MaximumLength(2000).WithMessage("Descrição muito longa");
        
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Preço deve ser maior que zero");
        
        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Estoque não pode ser negativo");
        
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Categoria é obrigatória");
        
        RuleFor(x => x.StoreId)
            .NotEmpty().WithMessage("StoreId é obrigatório");
    }
}
```

**Por que Validator separado?**
- Validações ficam **organizadas** em um lugar só
- Podem ser **reutilizadas**
- **Mensagens de erro** claras e customizadas

---

### **📁 Queries/ - Consultas de Leitura**

**O que são?** Ações que apenas **leem** dados, sem modificar nada.

```
Queries/
├── Products/
│   ├── GetProductById/
│   │   ├── GetProductByIdQuery.cs
│   │   └── GetProductByIdQueryHandler.cs
│   ├── GetProductsList/
│   └── SearchProducts/
├── Dashboard/
│   ├── GetStoreDashboard/
│   │   ├── GetStoreDashboardQuery.cs
│   │   └── GetStoreDashboardQueryHandler.cs
│   └── GetSalesMetrics/
└── Orders/
    ├── GetOrderById/
    └── GetCustomerOrders/
```

#### **Exemplo: GetProductByIdQuery.cs**

```csharp
namespace Application.Queries.Products.GetProductById;

/// <summary>
/// Query = pedido de leitura
/// IRequest<ProductDto> = retorna um ProductDto
/// </summary>
public class GetProductByIdQuery : IRequest<ProductDto>
{
    public Guid Id { get; set; }
    
    public GetProductByIdQuery(Guid id)
    {
        Id = id;
    }
}
```

#### **Exemplo: GetProductByIdQueryHandler.cs**

```csharp
namespace Application.Queries.Products.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IRecommendationService _recommendationService;
    private readonly IMapper _mapper;
    
    public GetProductByIdQueryHandler(
        IProductRepository productRepository,
        IRecommendationService recommendationService,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _recommendationService = recommendationService;
        _mapper = mapper;
    }
    
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Buscar produto no banco
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null)
            throw new NotFoundException("Produto não encontrado");
        
        // 2. Buscar produtos recomendados usando ML
        var recommendations = await _recommendationService
            .GetRecommendationsAsync(request.Id, limit: 5);
        
        // 3. Mapear entidade para DTO (AutoMapper)
        var productDto = _mapper.Map<ProductDto>(product);
        productDto.Recommendations = recommendations;
        
        return productDto;
    }
}
```

**Diferença Command vs Query:**
- **Command**: Muda dados → CreateProduct, UpdateOrder
- **Query**: Só lê → GetProduct, ListOrders

---

### **📁 DTOs/ - Data Transfer Objects**

**O que são?** Objetos "simplificados" para trafegar dados entre camadas.

```
DTOs/
├── ProductDto.cs
├── OrderDto.cs
├── CustomerDto.cs
├── DashboardMetricsDto.cs
└── ReviewDto.cs
```

#### **Exemplo: ProductDto.cs**

```csharp
namespace Application.DTOs;

/// <summary>
/// DTO = versão simplificada de Product
/// Só tem os dados que a API vai retornar
/// Não expõe tudo da entidade (ex: não mostra campos internos)
/// </summary>
public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Category { get; set; }
    public string? ImageUrl { get; set; }
    public decimal AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Dados da loja (relacionamento)
    public Guid StoreId { get; set; }
    public string StoreName { get; set; }
    
    // Recomendações (ML)
    public List<ProductDto>? Recommendations { get; set; }
}
```

**Por que DTO?**
- Entidade `Product` tem muitos campos internos (CreatedBy, UpdatedBy, etc)
- DTO mostra **apenas** o que o frontend precisa
- Evita **expor** informações sensíveis (ex: PasswordHash de User)

---

### **📁 Services/ - Interfaces de Serviços**

**O que são?** Contratos para funcionalidades específicas (ML, Email, etc).

```
Services/
├── IRecommendationService.cs      # ML: Recomendação de produtos
├── IDemandForecastService.cs      # ML: Previsão de vendas
├── ISentimentAnalysisService.cs   # ML: Análise de reviews
├── IPricingService.cs             # ML: Sugestão de preço
├── IEmailService.cs               # Enviar emails
└── INotificationService.cs        # Notificações push
```

#### **Exemplo: IRecommendationService.cs**

```csharp
namespace Application.Services;

/// <summary>
/// Interface para serviço de recomendação de produtos usando ML
/// Application define O QUÊ precisa
/// Infrastructure.MachineLearning implementa COMO
/// </summary>
public interface IRecommendationService
{
    /// <summary>
    /// Retorna produtos recomendados baseado no histórico do usuário
    /// </summary>
    Task<List<ProductDto>> GetRecommendationsForUserAsync(Guid userId, int limit = 10);
    
    /// <summary>
    /// Retorna produtos similares a um produto específico
    /// </summary>
    Task<List<ProductDto>> GetSimilarProductsAsync(Guid productId, int limit = 5);
    
    /// <summary>
    /// Treina o modelo com novos dados (roda em background)
    /// </summary>
    Task TrainModelAsync();
}
```

---

### **📁 Mappings/ - AutoMapper**

**O que são?** Configurações de como mapear Entidade → DTO e vice-versa.

```
Mappings/
├── ProductMappingProfile.cs
├── OrderMappingProfile.cs
└── UserMappingProfile.cs
```

#### **Exemplo: ProductMappingProfile.cs**

```csharp
namespace Application.Mappings;

/// <summary>
/// Define como mapear Product (entidade) → ProductDto
/// AutoMapper faz isso automaticamente
/// </summary>
public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        // Mapeamento básico (propriedades com mesmo nome)
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.ReviewCount, 
                opt => opt.MapFrom(src => src.Reviews.Count))
            .ForMember(dest => dest.StoreName,
                opt => opt.MapFrom(src => src.Store.Name));
        
        // Mapeamento reverso (DTO → Product)
        CreateMap<CreateProductCommand, Product>();
    }
}
```

**Por que AutoMapper?**
- Evita código repetitivo de copiar propriedades manualmente
- Mantém mapeamentos organizados
- Fácil de testar

---

## 3️⃣ INFRASTRUCTURE - Implementações Técnicas

**📂 Localização:** `MarketplaceAPI/Infrastructure/`

### **Responsabilidade:**
Implementa os contratos (interfaces) definidos em Application e Domain. Aqui fica o código que **conversa com o mundo externo** (banco, arquivos, ML).

### **Estrutura Completa:**

```
Infrastructure/
├── Data/                    # Banco de dados
├── MachineLearning/         # ML.NET
├── Identity/                # Autenticação JWT
├── Storage/                 # Upload de arquivos
├── Cache/                   # Redis
└── Services/                # Implementações gerais
```

---

### **📁 Data/ - Banco de Dados**

```
Data/
├── ApplicationDbContext.cs       # Conexão com PostgreSQL
├── Migrations/                   # Histórico de mudanças no banco
├── Repositories/                 # Implementação de IRepository
│   ├── ProductRepository.cs
│   ├── OrderRepository.cs
│   └── UserRepository.cs
├── Configurations/               # Mapeamento de tabelas
│   ├── ProductConfiguration.cs
│   └── UserConfiguration.cs
└── UnitOfWork.cs                 # Transações
```

#### **Exemplo: ApplicationDbContext.cs**

```csharp
namespace Infrastructure.Data;

/// <summary>
/// DbContext = ponte entre C# e PostgreSQL
/// Representa a conexão com o banco
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    // DbSet = representa uma tabela no banco
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Review> Reviews { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Aplicar todas as configurações (Fluent API)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
```

#### **Exemplo: ProductRepository.cs**

```csharp
namespace Infrastructure.Data.Repositories;

/// <summary>
/// Implementação concreta de IProductRepository
/// Esconde detalhes do EF Core
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    
    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        // Include = carregar relacionamentos (Eager Loading)
        return await _context.Products
            .Include(p => p.Store)       // Carregar loja
            .Include(p => p.Reviews)     // Carregar reviews
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
    {
        return await _context.Products
            .Where(p => p.IsActive &&
                (p.Name.Contains(searchTerm) || 
                 p.Description.Contains(searchTerm)))
            .ToListAsync();
    }
    
    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }
    
    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var product = await GetByIdAsync(id);
        if (product != null)
        {
            product.IsActive = false;  // Soft delete
        }
    }
}
```

**Por que Repository?**
- Application não precisa saber de EF Core
- Se trocar para MongoDB, só muda a implementação do Repository
- Facilita testes (pode mockar o repository)

#### **Exemplo: ProductConfiguration.cs (Fluent API)**

```csharp
namespace Infrastructure.Data.Configurations;

/// <summary>
/// Configuração de como mapear Product para a tabela no banco
/// Fluent API = alternativa a Data Annotations ([Required], etc)
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Nome da tabela
        builder.ToTable("Products");
        
        // Primary Key
        builder.HasKey(p => p.Id);
        
        // Propriedades
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(p => p.Description)
            .HasMaxLength(2000);
        
        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)")  // Precisão para dinheiro
            .IsRequired();
        
        // Índice para buscas
        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.Category);
        
        // Relacionamentos
        builder.HasOne(p => p.Store)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.Cascade);  // Se deletar loja, deleta produtos
        
        builder.HasMany(p => p.Reviews)
            .WithOne(r => r.Product)
            .HasForeignKey(r => r.ProductId);
    }
}
```

**Por que Fluent API?**
- Mais flexível que Data Annotations
- Configurações complexas (índices, relacionamentos)
- Não "polui" a entidade de Domain com detalhes de banco

---

### **📁 MachineLearning/ - ML.NET**

```
MachineLearning/
├── RecommendationService.cs      # Collaborative Filtering
├── DemandForecastService.cs      # Time Series Forecasting
├── SentimentAnalysisService.cs   # Text Classification
├── PricingService.cs             # Regression
├── Models/                       # Modelos treinados (.zip)
└── Data/                         # Datasets para treino
```

#### **Exemplo: RecommendationService.cs**

```csharp
namespace Infrastructure.MachineLearning;

/// <summary>
/// Implementação do serviço de recomendação usando ML.NET
/// Usa Matrix Factorization (Collaborative Filtering)
/// </summary>
public class RecommendationService : IRecommendationService
{
    private readonly MLContext _mlContext;
    private readonly IProductRepository _productRepository;
    private readonly string _modelPath;
    private ITransformer _model;
    
    public RecommendationService(IProductRepository productRepository)
    {
        _mlContext = new MLContext();
        _productRepository = productRepository;
        _modelPath = "MLModels/recommendation_model.zip";
        
        // Carregar modelo (se existir)
        LoadModel();
    }
    
    public async Task<List<ProductDto>> GetRecommendationsForUserAsync(Guid userId, int limit = 10)
    {
        if (_model == null)
        {
            // Modelo não treinado ainda, retorna produtos populares
            return await GetPopularProductsAsync(limit);
        }
        
        // 1. Criar PredictionEngine (faz predições)
        var predictionEngine = _mlContext.Model
            .CreatePredictionEngine<ProductRating, ProductPrediction>(_model);
        
        // 2. Buscar todos os produtos
        var allProducts = await _productRepository.GetAllAsync();
        
        // 3. Fazer predição para cada produto
        var predictions = new List<(Guid ProductId, float Score)>();
        
        foreach (var product in allProducts)
        {
            var input = new ProductRating
            {
                UserId = userId.ToString(),
                ProductId = product.Id.ToString()
            };
            
            var prediction = predictionEngine.Predict(input);
            predictions.Add((product.Id, prediction.Score));
        }
        
        // 4. Ordenar por score e pegar top N
        var topProductIds = predictions
            .OrderByDescending(p => p.Score)
            .Take(limit)
            .Select(p => p.ProductId)
            .ToList();
        
        // 5. Buscar detalhes dos produtos
        var recommendedProducts = allProducts
            .Where(p => topProductIds.Contains(p.Id))
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl
            })
            .ToList();
        
        return recommendedProducts;
    }
    
    public async Task TrainModelAsync()
    {
        // 1. Buscar dados de treinamento (histórico de compras/visualizações)
        var trainingData = await GetTrainingDataAsync();
        
        // 2. Carregar dados no ML.NET
        var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);
        
        // 3. Definir pipeline de treinamento
        var pipeline = _mlContext.Transforms.Conversion
            .MapValueToKey("UserIdEncoded", "UserId")
            .Append(_mlContext.Transforms.Conversion
                .MapValueToKey("ProductIdEncoded", "ProductId"))
            .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(
                new MatrixFactorizationTrainer.Options
                {
                    MatrixColumnIndexColumnName = "UserIdEncoded",
                    MatrixRowIndexColumnName = "ProductIdEncoded",
                    LabelColumnName = "Rating",
                    NumberOfIterations = 20,
                    ApproximationRank = 100
                }));
        
        // 4. Treinar modelo
        _model = pipeline.Fit(dataView);
        
        // 5. Salvar modelo em arquivo
        _mlContext.Model.Save(_model, dataView.Schema, _modelPath);
    }
    
    private void LoadModel()
    {
        if (File.Exists(_modelPath))
        {
            _model = _mlContext.Model.Load(_modelPath, out _);
        }
    }
}

// Classes auxiliares para ML.NET
public class ProductRating
{
    public string UserId { get; set; }
    public string ProductId { get; set; }
    public float Rating { get; set; }  // 1 = visualizou, 5 = comprou
}

public class ProductPrediction
{
    public float Score { get; set; }  // Score previsto
}
```

**Como funciona o ML de recomendação?**
1. **Treinar**: Usa histórico (usuário X comprou produto Y)
2. **Matriz**: Cria matriz de usuários × produtos
3. **Fatoração**: Encontra padrões ocultos (usuários similares)
4. **Predição**: "Se usuário A gostou de X e Y, e usuário B gostou de X, então B pode gostar de Y"

---

### **📁 Identity/ - Autenticação JWT**

```
Identity/
├── JwtService.cs           # Gera e valida tokens
├── PasswordHasher.cs       # BCrypt para senhas
└── TokenSettings.cs        # Configurações (secret, expiration)
```

#### **Exemplo: JwtService.cs**

```csharp
namespace Infrastructure.Identity;

/// <summary>
/// Serviço para gerar e validar tokens JWT
/// </summary>
public class JwtService
{
    private readonly TokenSettings _settings;
    
    public JwtService(IOptions<TokenSettings> settings)
    {
        _settings = settings.Value;
    }
    
    /// <summary>
    /// Gera um token JWT para o usuário autenticado
    /// </summary>
    public string GenerateToken(User user)
    {
        // 1. Criar claims (informações no token)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.UserType.ToString()),  // Customer, Vendor, Admin
            new Claim("UserType", user.UserType.ToString())
        };
        
        // 2. Criar chave de assinatura
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        // 3. Criar token
        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_settings.ExpirationHours),
            signingCredentials: credentials
        );
        
        // 4. Retornar token como string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    /// <summary>
    /// Valida um token JWT
    /// </summary>
    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_settings.Secret);
        
        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _settings.Issuer,
                ValidateAudience = true,
                ValidAudience = _settings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            
            return principal;
        }
        catch
        {
            return null;  // Token inválido
        }
    }
}
```

**Como funciona JWT?**
1. **Login**: Usuário envia email/senha
2. **Validação**: Backend verifica se senha está correta
3. **Token**: Backend gera JWT com informações do usuário
4. **Retorno**: Retorna token para o frontend
5. **Uso**: Frontend envia token em **TODA** requisição no header `Authorization: Bearer {token}`
6. **Validação**: Backend valida token antes de processar requisição

---

## 4️⃣ API - Controllers

**📂 Localização:** `MarketplaceAPI/API/`

### **Responsabilidade:**
Recebe requisições HTTP, chama Commands/Queries via MediatR, retorna JSON.

### **Estrutura:**

```
API/
├── Controllers/
│   ├── AuthController.cs
│   ├── ProductsController.cs
│   ├── StoresController.cs
│   ├── OrdersController.cs
│   ├── DashboardController.cs
│   ├── VendorFeedController.cs
│   └── AdminController.cs
├── Middleware/
│   ├── ExceptionHandlerMiddleware.cs
│   └── LoggingMiddleware.cs
└── Program.cs
```

#### **Exemplo: ProductsController.cs**

```csharp
namespace API.Controllers;

/// <summary>
/// Controller para gerenciar produtos
/// [ApiController] = adiciona validações automáticas
/// [Route] = define URL base: /api/products
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;  // MediatR para CQRS
    
    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// GET /api/products/{id}
    /// Busca produto por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetProductByIdQuery(id);
        var product = await _mediator.Send(query);
        
        return Ok(product);  // Retorna JSON
    }
    
    /// <summary>
    /// POST /api/products
    /// Cria novo produto
    /// [Authorize] = precisa estar autenticado
    /// [FromForm] = dados vêm de form (multipart, suporta arquivos)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Vendor")]  // Só vendedor pode criar produto
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromForm] CreateProductCommand command)
    {
        // Pega ID do vendedor do token JWT
        var vendorId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var productId = await _mediator.Send(command);
        
        return CreatedAtAction(
            nameof(GetById),
            new { id = productId },
            new { id = productId }
        );
    }
    
    /// <summary>
    /// GET /api/products/search?term=notebook
    /// Busca produtos por termo
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string term)
    {
        var query = new SearchProductsQuery { SearchTerm = term };
        var products = await _mediator.Send(query);
        
        return Ok(products);
    }
    
    /// <summary>
    /// PUT /api/products/{id}
    /// Atualiza produto
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Vendor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        
        return NoContent();  // 204 = sucesso sem retorno
    }
    
    /// <summary>
    /// DELETE /api/products/{id}
    /// Deleta produto (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Vendor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteProductCommand { Id = id };
        await _mediator.Send(command);
        
        return NoContent();
    }
}
```

**Convenções REST:**
- **GET** = Buscar dados (não muda nada)
- **POST** = Criar novo recurso
- **PUT** = Atualizar recurso completo
- **PATCH** = Atualizar parcial
- **DELETE** = Deletar recurso

---

## 🎨 FRONTEND (Angular 18)

### **Visão Geral da Estrutura**

```
src/app/
├── core/           # 🔐 Serviços globais (auth, interceptors)
├── shared/         # 🧩 Componentes reutilizáveis
├── features/       # 📦 Módulos por funcionalidade (lazy)
└── store/          # 📊 Estado global (NgRx)
```

---

## 1️⃣ CORE - Serviços Globais

**📂 Localização:** `src/app/core/`

### **Responsabilidade:**
Serviços **singleton** usados em toda aplicação. Só existe UMA instância.

### **Estrutura:**

```
core/
├── auth/
│   ├── auth.service.ts
│   ├── auth.guard.ts
│   └── role.guard.ts
├── interceptors/
│   ├── jwt.interceptor.ts
│   └── error.interceptor.ts
└── api/
    └── api.service.ts
```

#### **Exemplo: auth.service.ts**

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';

/**
 * Serviço de autenticação
 * providedIn: 'root' = singleton global
 */
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly API_URL = '/api/auth';
  
  // BehaviorSubject = Observable que guarda o último valor
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();
  
  constructor(private http: HttpClient) {
    // Verifica se já está logado ao inicializar
    this.checkAuth();
  }
  
  /**
   * Faz login e salva token
   */
  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.API_URL}/login`, { email, password })
      .pipe(
        tap(response => {
          // Salva token no localStorage
          localStorage.setItem(this.TOKEN_KEY, response.token);
          // Atualiza usuário atual
          this.currentUserSubject.next(response.user);
        })
      );
  }
  
  /**
   * Faz logout
   */
  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    this.currentUserSubject.next(null);
  }
  
  /**
   * Verifica se está autenticado
   */
  isAuthenticated(): boolean {
    const token = localStorage.getItem(this.TOKEN_KEY);
    return !!token && !this.isTokenExpired(token);
  }
  
  /**
   * Retorna o token JWT
   */
  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }
  
  /**
   * Verifica se token está expirado
   */
  private isTokenExpired(token: string): boolean {
    try {
      // Decodifica payload do JWT (parte do meio entre os '.')
      const payload = JSON.parse(atob(token.split('.')[1]));
      const expiration = payload.exp * 1000; // Converte para milissegundos
      return Date.now() > expiration;
    } catch {
      return true;
    }
  }
  
  private checkAuth(): void {
    if (this.isAuthenticated()) {
      // Buscar dados do usuário atual
      this.http.get<User>(`${this.API_URL}/me`).subscribe({
        next: user => this.currentUserSubject.next(user),
        error: () => this.logout()
      });
    }
  }
}

// Interfaces
export interface LoginResponse {
  token: string;
  user: User;
}

export interface User {
  id: string;
  email: string;
  userType: 'Customer' | 'Vendor' | 'Admin';
}
```

#### **Exemplo: auth.guard.ts**

```typescript
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './auth.service';

/**
 * Guard para proteger rotas
 * Só permite acessar se estiver autenticado
 */
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}
  
  canActivate(): boolean {
    if (this.authService.isAuthenticated()) {
      return true;  // Permite acessar
    }
    
    // Não autenticado, redireciona para login
    this.router.navigate(['/login']);
    return false;
  }
}
```

**Uso no routing:**
```typescript
const routes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard]  // ← Protege a rota
  }
];
```

#### **Exemplo: jwt.interceptor.ts**

```typescript
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';

/**
 * Interceptor que adiciona token JWT em TODAS as requisições
 */
@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}
  
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();
    
    // Se tem token, clona requisição e adiciona header
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }
    
    // Continua a requisição
    return next.handle(request);
  }
}
```

**Como funciona Interceptor?**
1. Toda requisição HTTP passa pelo interceptor
2. Interceptor adiciona header `Authorization`
3. Backend valida o token
4. Se válido, processa requisição

---

## 2️⃣ SHARED - Componentes Reutilizáveis

**📂 Localização:** `src/app/shared/`

### **Responsabilidade:**
Componentes, diretivas e pipes usados em **vários lugares** da aplicação.

### **Estrutura:**

```
shared/
├── components/
│   ├── navbar/
│   ├── product-card/
│   ├── rating-stars/
│   └── chart/
├── directives/
│   └── highlight.directive.ts
└── pipes/
    └── currency-brl.pipe.ts
```

#### **Exemplo: product-card.component.ts**

```typescript
import { Component, Input, Output, EventEmitter } from '@angular/core';

/**
 * Card reutilizável para mostrar produto
 * Usado em: lista de produtos, recomendações, busca
 */
@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent {
  // @Input = recebe dados do componente pai
  @Input() product!: Product;
  
  // @Output = envia eventos para o pai
  @Output() addToCart = new EventEmitter<Product>();
  @Output() viewDetails = new EventEmitter<string>();
  
  onAddToCart(): void {
    this.addToCart.emit(this.product);
  }
  
  onViewDetails(): void {
    this.viewDetails.emit(this.product.id);
  }
}

export interface Product {
  id: string;
  name: string;
  price: number;
  imageUrl: string;
  averageRating: number;
}
```

**Template (product-card.component.html):**
```html
<div class="product-card">
  <img [src]="product.imageUrl" [alt]="product.name">
  
  <div class="product-info">
    <h3>{{ product.name }}</h3>
    
    <!-- Componente de estrelas reutilizável -->
    <app-rating-stars [rating]="product.averageRating"></app-rating-stars>
    
    <p class="price">{{ product.price | currency:'BRL' }}</p>
    
    <div class="actions">
      <button (click)="onViewDetails()" class="btn-secondary">
        Ver Detalhes
      </button>
      <button (click)="onAddToCart()" class="btn-primary">
        Adicionar ao Carrinho
      </button>
    </div>
  </div>
</div>
```

**Uso em outro componente:**
```html
<app-product-card 
  [product]="product"
  (addToCart)="handleAddToCart($event)"
  (viewDetails)="navigateToProduct($event)">
</app-product-card>
```

---

## 3️⃣ FEATURES - Módulos por Funcionalidade

**📂 Localização:** `src/app/features/`

### **Responsabilidade:**
Funcionalidades isoladas em módulos. **Lazy Loading** = só carrega quando necessário.

### **Estrutura:**

```
features/
├── customer/          # Módulo do Cliente
├── vendor/            # Módulo do Vendedor
└── admin/             # Módulo do Admin
```

---

### **📁 customer/ - Módulo Cliente**

```
customer/
├── pages/
│   ├── products-list/
│   │   ├── products-list.component.ts
│   │   ├── products-list.component.html
│   │   └── products-list.component.scss
│   ├── product-detail/
│   ├── cart/
│   └── my-orders/
├── customer.module.ts
└── customer-routing.module.ts
```

#### **Exemplo: products-list.component.ts**

```typescript
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ProductService } from '../../services/product.service';

/**
 * Página de listagem de produtos
 */
@Component({
  selector: 'app-products-list',
  templateUrl: './products-list.component.html'
})
export class ProductsListComponent implements OnInit {
  // Observable = stream de dados assíncrono
  products$!: Observable<Product[]>;
  
  constructor(private productService: ProductService) {}
  
  ngOnInit(): void {
    // Busca produtos ao inicializar
    this.products$ = this.productService.getAll();
  }
  
  onAddToCart(product: Product): void {
    this.productService.addToCart(product).subscribe({
      next: () => alert('Produto adicionado ao carrinho!'),
      error: (err) => alert('Erro ao adicionar: ' + err.message)
    });
  }
}
```

**Template:**
```html
<div class="products-list">
  <h1>Produtos</h1>
  
  <!-- async pipe = subscreve no Observable automaticamente -->
  <div class="products-grid">
    <app-product-card
      *ngFor="let product of products$ | async"
      [product]="product"
      (addToCart)="onAddToCart($event)">
    </app-product-card>
  </div>
</div>
```

**Por que Observable?**
- Permite programação **reativa**
- Auto-subscreve e desinscreve (evita memory leaks)
- Facilita operações assíncronas (HTTP)

---

### **📁 vendor/ - Módulo Vendedor (com NgRx)**

```
vendor/
├── pages/
│   ├── dashboard/
│   │   ├── dashboard.component.ts      # Smart component
│   │   ├── sales-chart/               # Dumb component
│   │   └── revenue-forecast/          # Dumb component
│   ├── my-stores/
│   └── vendor-feed/
├── store/                              # NgRx State
│   ├── actions/
│   │   └── dashboard.actions.ts
│   ├── reducers/
│   │   └── dashboard.reducer.ts
│   ├── effects/
│   │   └── dashboard.effects.ts
│   └── selectors/
│       └── dashboard.selectors.ts
├── vendor.module.ts
└── vendor-routing.module.ts
```

#### **Exemplo: dashboard.actions.ts (NgRx)**

```typescript
import { createAction, props } from '@ngrx/store';

/**
 * Ações do dashboard
 * Ação = evento que acontece na aplicação
 */

// Carregar métricas
export const loadMetrics = createAction(
  '[Dashboard] Load Metrics',
  props<{ storeId: string }>()
);

// Sucesso ao carregar
export const loadMetricsSuccess = createAction(
  '[Dashboard] Load Metrics Success',
  props<{ metrics: DashboardMetrics }>()
);

// Erro ao carregar
export const loadMetricsFailure = createAction(
  '[Dashboard] Load Metrics Failure',
  props<{ error: string }>()
);

export interface DashboardMetrics {
  totalSales: number;
  revenue: number;
  expenses: number;
  futureRevenue: number;
  trendingProducts: Product[];
}
```

#### **Exemplo: dashboard.reducer.ts**

```typescript
import { createReducer, on } from '@ngrx/store';
import * as DashboardActions from '../actions/dashboard.actions';

/**
 * Reducer = função pura que define como o estado muda
 * Estado = snapshot dos dados em um momento
 */

export interface DashboardState {
  metrics: DashboardMetrics | null;
  loading: boolean;
  error: string | null;
}

export const initialState: DashboardState = {
  metrics: null,
  loading: false,
  error: null
};

export const dashboardReducer = createReducer(
  initialState,
  
  // Quando carrega métricas, marca loading = true
  on(DashboardActions.loadMetrics, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  
  // Sucesso: salva métricas, loading = false
  on(DashboardActions.loadMetricsSuccess, (state, { metrics }) => ({
    ...state,
    metrics,
    loading: false
  })),
  
  // Erro: salva mensagem de erro
  on(DashboardActions.loadMetricsFailure, (state, { error }) => ({
    ...state,
    error,
    loading: false
  }))
);
```

#### **Exemplo: dashboard.effects.ts**

```typescript
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, catchError, switchMap } from 'rxjs/operators';
import * as DashboardActions from '../actions/dashboard.actions';

/**
 * Effects = side effects (chamadas HTTP, navegação, etc)
 * Escuta ações e dispara outras ações
 */
@Injectable()
export class DashboardEffects {
  constructor(
    private actions$: Actions,
    private dashboardService: DashboardService
  ) {}
  
  /**
   * Quando dispara loadMetrics, faz HTTP call
   */
  loadMetrics$ = createEffect(() =>
    this.actions$.pipe(
      // Escuta ação loadMetrics
      ofType(DashboardActions.loadMetrics),
      
      // Faz requisição HTTP
      switchMap(({ storeId }) =>
        this.dashboardService.getMetrics(storeId).pipe(
          // Sucesso: dispara loadMetricsSuccess
          map(metrics => DashboardActions.loadMetricsSuccess({ metrics })),
          
          // Erro: dispara loadMetricsFailure
          catchError(error => 
            of(DashboardActions.loadMetricsFailure({ error: error.message }))
          )
        )
      )
    )
  );
}
```

#### **Exemplo: dashboard.component.ts**

```typescript
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import * as DashboardActions from '../store/actions/dashboard.actions';
import * as DashboardSelectors from '../store/selectors/dashboard.selectors';

/**
 * Smart component = conecta com NgRx Store
 */
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  // Selectors = buscam dados do state
  metrics$ = this.store.select(DashboardSelectors.selectMetrics);
  loading$ = this.store.select(DashboardSelectors.selectLoading);
  error$ = this.store.select(DashboardSelectors.selectError);
  
  constructor(private store: Store) {}
  
  ngOnInit(): void {
    // Dispara ação para carregar métricas
    this.store.dispatch(DashboardActions.loadMetrics({ storeId: 'abc-123' }));
  }
}
```

**Template:**
```html
<div class="dashboard">
  <h1>Dashboard</h1>
  
  <!-- Loading spinner -->
  <div *ngIf="loading$ | async" class="spinner"></div>
  
  <!-- Erro -->
  <div *ngIf="error$ | async as error" class="alert-error">
    {{ error }}
  </div>
  
  <!-- Métricas -->
  <div *ngIf="metrics$ | async as metrics" class="metrics-grid">
    <div class="metric-card">
      <h3>Vendas</h3>
      <p>{{ metrics.totalSales }}</p>
    </div>
    
    <div class="metric-card">
      <h3>Receita</h3>
      <p>{{ metrics.revenue | currency:'BRL' }}</p>
    </div>
    
    <!-- Gráfico de vendas -->
    <app-sales-chart [data]="metrics"></app-sales-chart>
    
    <!-- Previsão de receita (ML) -->
    <app-revenue-forecast [futureRevenue]="metrics.futureRevenue"></app-revenue-forecast>
  </div>
</div>
```

---

## 🔗 FLUXO COMPLETO DE REQUISIÇÃO

### **Cenário: Cliente adiciona produto ao carrinho**

#### **Frontend (Angular):**

```
1. Component (products-list.component.ts)
   ↓
   onAddToCart(product)
   ↓
2. Service (product.service.ts)
   ↓
   addToCart(product) → HTTP POST /api/cart/items
   ↓
3. Interceptor (jwt.interceptor.ts)
   ↓
   Adiciona header: Authorization: Bearer {token}
   ↓
4. HTTP Request enviado
```

#### **Backend (.NET):**

```
5. API (CartController)
   ↓
   [HttpPost] AddItem(AddItemCommand command)
   ↓
6. MediatR
   ↓
   Send(command) → Encontra Handler
   ↓
7. Handler (AddItemCommandHandler)
   ↓
   - Valida produto existe
   - Busca carrinho do cliente
   - Adiciona item
   ↓
8. Repository (CartRepository)
   ↓
   AddItemAsync() → EF Core
   ↓
9. PostgreSQL
   ↓
   INSERT INTO CartItems
   ↓
10. Retorna JSON
```

#### **Frontend recebe resposta:**

```
11. Interceptor (error.interceptor.ts)
    ↓
    Verifica se tem erro (401, 500)
    ↓
12. Service
    ↓
    Observable.next(response)
    ↓
13. Component
    ↓
    Mostra mensagem de sucesso
    ↓
14. (Opcional) NgRx Store atualiza contador do carrinho
```

---

## ❓ DÚVIDAS COMUNS

### **1. Por que tantas camadas?**

**R:** Separação de responsabilidades.
- **Domain**: Regras de negócio puras (independente de tecnologia)
- **Application**: Casos de uso (o que o sistema FAZ)
- **Infrastructure**: Detalhes técnicos (como faz: banco, ML, arquivos)
- **API**: Interface HTTP

**Benefícios:**
- **Manutenção**: Trocar banco? Só mexe em Infrastructure
- **Testes**: Testar lógica sem banco (mocka repositories)
- **Escalabilidade**: Fácil adicionar features

### **2. Por que CQRS?**

**R:** Leituras e escritas têm necessidades diferentes.
- **Queries (Read)**: Simples, rápidas, podem usar cache
- **Commands (Write)**: Validações complexas, regras de negócio

**Benefício:** Cada lado otimizado independentemente.

### **3. Por que NgRx?**

**R:** Para aplicações complexas com muito estado.

**Dashboard vendedor:**
- Múltiplos gráficos
- Métricas em tempo real
- Filtros complexos
- Vários componentes precisam dos mesmos dados

**NgRx:**
- **Centraliza** estado (single source of truth)
- **Facilita debug** (Redux DevTools mostra tudo)
- **Previsível** (sempre sabe como estado muda)

### **4. Lazy Loading vale a pena?**

**R:** SIM, principalmente para aplicações grandes.

**Sem Lazy Loading:**
```
Bundle inicial: 5MB
Tempo de carregamento: 10s
```

**Com Lazy Loading:**
```
Bundle inicial: 500KB (só core + login)
Tempo de carregamento: 2s
Módulos carregam sob demanda
```

Cliente não precisa carregar código do Admin!

### **5. Repository Pattern é necessário?**

**R:** Depende, mas recomendado.

**Sem Repository:**
```csharp
// Controller fala direto com DbContext
var product = await _context.Products.FindAsync(id);
```

**Problema:** Controller conhece detalhes de EF Core.

**Com Repository:**
```csharp
// Controller fala com abstração
var product = await _productRepository.GetByIdAsync(id);
```

**Benefício:** Trocar banco não quebra Application/Domain.

---

## 📋 RESUMO RÁPIDO

### **Backend:**
```
Domain       → Entidades, Regras de Negócio
Application  → Commands, Queries, DTOs
Infrastructure → Banco, ML, Storage
API          → Controllers REST
```

### **Frontend:**
```
core     → Serviços globais (auth, API)
shared   → Componentes reutilizáveis
features → Módulos lazy (customer, vendor, admin)
store    → Estado global NgRx
```

### **Princípios:**
1. **Separação de responsabilidades**
2. **Dependency Injection**
3. **CQRS para escalabilidade**
4. **Repository para abstração**
5. **Lazy Loading para performance**
6. **NgRx para estado complexo**

---

## 🎯 PRÓXIMOS PASSOS

Agora que você entende a arquitetura:

1. ✅ **Tecnologias definidas**
2. ✅ **Arquitetura explicada**
3. ⏭️ **Criar estrutura de pastas** (vazia, sem código)
4. ⏭️ **Setup inicial** (appsettings, package.json)
5. ⏭️ **Primeiro endpoint** (POST /api/auth/register)
6. ⏭️ **Desenvolvimento incremental** (sprints)

---

**Documento criado para:** Marketplace Platform  
**Público-alvo:** Iniciantes em .NET e Angular  
**Foco:** Explicações detalhadas e didáticas
