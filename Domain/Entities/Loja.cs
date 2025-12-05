using Domain.Enums;
using Domain.ValueObject;

namespace Domain.Entities;

/// <summary>
/// Representa uma loja no marketplace
/// </summary>
public class Loja
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public Guid VendedorId { get; private set; }
    public string? CNPJ { get; private set; }
    public string Email { get; private set; }
    public string Telefone { get; private set; }
    public Endereco EnderecoOrigem { get; private set; }
    public string? Descricao { get; private set; }
    public TipoTarifa TipoTarifa { get; private set; }
    public decimal ValorTarifa { get; private set; }
    public bool Ativa { get; private set; }
    public bool Credenciada { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime DataModificacao { get; private set; }
    
    private Loja(
        string nome,
        Guid vendedorId,
        string email,
        string telefone,
        Endereco enderecoOrigem,
        TipoTarifa tipoTarifa,
        decimal valorTarifa,
        string? cnpj = null,
        string? descricao = null)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        VendedorId = vendedorId;
        CNPJ = cnpj;
        Email = email;
        Telefone = telefone;
        EnderecoOrigem = enderecoOrigem;
        Descricao = descricao;
        TipoTarifa = tipoTarifa;
        ValorTarifa = valorTarifa;
        Ativa = true;
        Credenciada = false;
        DataCriacao = DateTime.UtcNow;
        DataModificacao = DateTime.UtcNow;
    }
    
    public static Loja Criar(
        string nome,
        Guid vendedorId,
        string email,
        string telefone,
        Endereco enderecoOrigem,
        TipoTarifa tipoTarifa,
        decimal valorTarifa,
        string? cnpj = null,
        string? descricao = null)
    {
        ValidarDados(nome, vendedorId, email, telefone, enderecoOrigem, tipoTarifa, valorTarifa, cnpj);
        return new Loja(nome, vendedorId, email, telefone, enderecoOrigem, tipoTarifa, valorTarifa, cnpj, descricao);
    }
    
    private static void ValidarDados(
        string nome,
        Guid vendedorId,
        string email,
        string telefone,
        Endereco enderecoOrigem,
        TipoTarifa tipoTarifa,
        decimal valorTarifa,
        string? cnpj)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome da loja não pode ser vazio", nameof(nome));
        
        if (nome.Length > 100)
            throw new ArgumentException("Nome da loja não pode ter mais de 100 caracteres", nameof(nome));
        
        if (vendedorId == Guid.Empty)
            throw new ArgumentException("VendedorId não pode ser vazio", nameof(vendedorId));
        
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email não pode ser vazio", nameof(email));
        
        if (!email.Contains("@"))
            throw new ArgumentException("Email inválido", nameof(email));
        
        if (string.IsNullOrWhiteSpace(telefone))
            throw new ArgumentException("Telefone não pode ser vazio", nameof(telefone));
        
        if (enderecoOrigem == null)
            throw new ArgumentNullException(nameof(enderecoOrigem));
        
        if (!string.IsNullOrWhiteSpace(cnpj) && cnpj.Length != 14)
            throw new ArgumentException("CNPJ deve ter 14 dígitos", nameof(cnpj));
        
        if (tipoTarifa != TipoTarifa.Gratis && valorTarifa < 0)
            throw new ArgumentException("Valor da tarifa não pode ser negativo", nameof(valorTarifa));
        
        if (tipoTarifa == TipoTarifa.Gratis && valorTarifa != 0)
            throw new ArgumentException("Tarifa grátis deve ter valor zero", nameof(valorTarifa));
    }
    
    /// <summary>
    /// Ativa a loja
    /// </summary>
    public void Ativar()
    {
        if (Ativa)
            throw new InvalidOperationException("Loja já está ativa");
        
        Ativa = true;
        DataModificacao = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Desativa a loja
    /// </summary>
    public void Desativar()
    {
        if (!Ativa)
            throw new InvalidOperationException("Loja já está desativada");
        
        Ativa = false;
        DataModificacao = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Marca a loja como credenciada (gerenciado por administradores via Application Layer)
    /// </summary>
    public void MarcarComoCredenciada()
    {
        if (Credenciada)
            throw new InvalidOperationException("Loja já está credenciada");
        
        Credenciada = true;
        DataModificacao = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Remove credenciamento da loja (gerenciado por administradores via Application Layer)
    /// </summary>
    public void RemoverCredenciamento()
    {
        if (!Credenciada)
            throw new InvalidOperationException("Loja não está credenciada");
        
        Credenciada = false;
        DataModificacao = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Atualiza informações básicas da loja
    /// </summary>
    public void AtualizarInformacoes(string? nome = null, string? descricao = null, string? email = null, string? telefone = null)
    {
        bool houveAlteracao = false;
        
        if (!string.IsNullOrWhiteSpace(nome))
        {
            if (nome.Length > 100)
                throw new ArgumentException("Nome da loja não pode ter mais de 100 caracteres");
            Nome = nome;
            houveAlteracao = true;
        }
        
        if (descricao != null)
        {
            Descricao = descricao;
            houveAlteracao = true;
        }
        
        if (!string.IsNullOrWhiteSpace(email))
        {
            if (!email.Contains("@"))
                throw new ArgumentException("Email inválido");
            Email = email;
            houveAlteracao = true;
        }
        
        if (!string.IsNullOrWhiteSpace(telefone))
        {
            Telefone = telefone;
            houveAlteracao = true;
        }
        
        if (houveAlteracao)
            DataModificacao = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Atualiza a configuração de tarifa de frete
    /// </summary>
    public void AtualizarTarifa(TipoTarifa tipoTarifa, decimal valorTarifa)
    {
        if (tipoTarifa != TipoTarifa.Gratis && valorTarifa < 0)
            throw new ArgumentException("Valor da tarifa não pode ser negativo");
        
        if (tipoTarifa == TipoTarifa.Gratis && valorTarifa != 0)
            throw new ArgumentException("Tarifa grátis deve ter valor zero");
        
        TipoTarifa = tipoTarifa;
        ValorTarifa = valorTarifa;
        DataModificacao = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Atualiza o endereço de origem da loja
    /// </summary>
    public void AtualizarEndereco(Endereco novoEndereco)
    {
        if (novoEndereco == null)
            throw new ArgumentNullException(nameof(novoEndereco));
        
        EnderecoOrigem = novoEndereco;
        DataModificacao = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Calcula o frete com base no tipo de tarifa e distância
    /// </summary>
    public decimal CalcularFrete(decimal distanciaEmKm)
    {
        if (distanciaEmKm < 0)
            throw new ArgumentException("Distância não pode ser negativa", nameof(distanciaEmKm));
        
        return TipoTarifa switch
        {
            TipoTarifa.Gratis => 0m,
            TipoTarifa.Fixa => ValorTarifa,
            TipoTarifa.PorKm => ValorTarifa * distanciaEmKm,
            TipoTarifa.PorFaixaDistancia => throw new NotImplementedException("Cálculo por faixa de distância será implementado futuramente"),
            _ => throw new InvalidOperationException("Tipo de tarifa inválido")
        };
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Loja other)
            return false;
        
        if (ReferenceEquals(this, other))
            return true;
        
        return Id == other.Id;
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}