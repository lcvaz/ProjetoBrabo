using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Representa um vendedor no marketplace.
/// Herda as propriedades comuns de Usuario e adiciona funcionalidades específicas para vendedores.
/// Um vendedor pode ter até 2 lojas e acesso a dashboards e feed social.
/// </summary>
public class Vendedor : Usuario
{
    /// <summary>
    /// Número do documento do vendedor (CPF ou CNPJ)
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Avaliação média do vendedor (0.0 a 5.0)
    /// </summary>
    public decimal Rating { get; set; } = 0.0m;

    /// <summary>
    /// Coleção de lojas do vendedor (máximo de 2 lojas permitidas)
    /// </summary>
    public ICollection<Loja> Lojas { get; set; } = new List<Loja>();

    /// <summary>
    /// Coleção de posts criados pelo vendedor no feed social
    /// </summary>
    public ICollection<Post> Posts { get; set; } = new List<Post>();

    /// <summary>
    /// Coleção de conexões do vendedor com outros vendedores/fornecedores
    /// </summary>
    public ICollection<Conexao> Conexoes { get; set; } = new List<Conexao>();

    /// <summary>
    /// Construtor padrão do Vendedor
    /// </summary>
    public Vendedor() : base()
    {
        TipoUsuario = TipoUsuario.Vendedor;
    }

    /// <summary>
    /// Construtor para criar um vendedor com dados básicos
    /// </summary>
    /// <param name="nome">Nome completo do vendedor</param>
    /// <param name="email">Email do vendedor</param>
    /// <param name="documentNumber">CPF ou CNPJ do vendedor</param>
    public Vendedor(string nome, string email, string documentNumber) : base()
    {
        Nome = nome;
        Email = email;
        DocumentNumber = documentNumber;
        TipoUsuario = TipoUsuario.Vendedor;
    }

    /// <summary>
    /// Verifica se o vendedor atingiu o limite máximo de lojas (2)
    /// </summary>
    public bool PodeAdicionarLoja => Lojas.Count < 2;

    /// <summary>
    /// Verifica se o vendedor possui ao menos uma loja
    /// </summary>
    public bool TemLojas => Lojas.Any();

    /// <summary>
    /// Adiciona uma loja ao vendedor, respeitando o limite de 2 lojas
    /// </summary>
    /// <param name="loja">A loja a ser adicionada</param>
    /// <exception cref="InvalidOperationException">Lançada quando o vendedor já possui 2 lojas</exception>
    /// <exception cref="ArgumentNullException">Lançada quando a loja é nula</exception>
    public void AdicionarLoja(Loja loja)
    {
        if (loja == null)
            throw new ArgumentNullException(nameof(loja), "Loja não pode ser nula");

        if (!PodeAdicionarLoja)
            throw new InvalidOperationException("Vendedor já possui o número máximo de lojas (2)");

        Lojas.Add(loja);
        AtualizadoEm = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza a avaliação (rating) do vendedor
    /// </summary>
    /// <param name="novoRating">Novo valor de rating (deve estar entre 0.0 e 5.0)</param>
    /// <exception cref="ArgumentOutOfRangeException">Lançada quando o rating está fora do intervalo válido</exception>
    public void AtualizarRating(decimal novoRating)
    {
        if (novoRating < 0.0m || novoRating > 5.0m)
            throw new ArgumentOutOfRangeException(nameof(novoRating), "Rating deve estar entre 0.0 e 5.0");

        Rating = novoRating;
        AtualizadoEm = DateTime.UtcNow;
    }
}
