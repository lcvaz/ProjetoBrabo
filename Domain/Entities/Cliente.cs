namespace Domain.Entities;

/// <summary>
/// Representa um cliente (comprador) no marketplace.
/// Herda as propriedades comuns de Usuario e adiciona funcionalidades específicas.
/// </summary>
public class Cliente : Usuario
{
    /// <summary>
    /// Número de telefone do cliente para contato
    /// </summary>
    public string? Telefone { get; set; }

    /// <summary>
    /// Endereço de entrega padrão do cliente (embedded Value Object)
    /// </summary>
    public Endereco? EnderecoEntrega { get; set; }

    /// <summary>
    /// Coleção de pedidos realizados pelo cliente
    /// </summary>
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    /// <summary>
    /// Coleção de avaliações (reviews) feitas pelo cliente
    /// </summary>
    public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();

    /// <summary>
    /// Construtor padrão do Cliente
    /// </summary>
    public Cliente() : base()
    {
    }

    /// <summary>
    /// Construtor para criar um cliente com dados básicos
    /// </summary>
    /// <param name="nome">Nome completo do cliente</param>
    /// <param name="email">Email do cliente</param>
    /// <param name="telefone">Telefone opcional</param>
    public Cliente(string nome, string email, string? telefone = null) : base()
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        TipoUsuario = Domain.Enums.TipoUsuario.Cliente;
    }

    /// <summary>
    /// Define o endereço de entrega do cliente
    /// </summary>
    /// <param name="endereco">O novo endereço de entrega</param>
    public void DefinirEnderecoEntrega(Endereco endereco)
    {
        EnderecoEntrega = endereco ?? throw new ArgumentNullException(nameof(endereco), "Endereço não pode ser nulo");
        AtualizadoEm = DateTime.UtcNow;
    }

    /// <summary>
    /// Verifica se o cliente tem um endereço de entrega configurado
    /// </summary>
    public bool TemEnderecoEntrega => EnderecoEntrega != null;
}