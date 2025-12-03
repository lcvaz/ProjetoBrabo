namespace Domain.Entities;

/// <summary>
/// Representa uma avaliação feita por um cliente sobre um produto.
/// Contém nota, texto descritivo e opcionalmente fotos.
/// </summary>
public class Avaliacao
{
    /// <summary>
    /// Identificador único da avaliação
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador do produto avaliado
    /// </summary>
    public Guid ProdutoId { get; set; }

    /// <summary>
    /// Identificador do cliente que fez a avaliação
    /// </summary>
    public Guid ClienteId { get; set; }

    /// <summary>
    /// Nota atribuída ao produto (de 1 a 5 estrelas)
    /// </summary>
    public int Nota { get; set; }

    /// <summary>
    /// Texto descritivo da avaliação (comentário do cliente)
    /// </summary>
    public string Texto { get; set; } = string.Empty;

    /// <summary>
    /// Data e hora em que a avaliação foi criada (UTC)
    /// </summary>
    public DateTime CriadoEm { get; set; }

    /// <summary>
    /// Coleção de fotos anexadas à avaliação (Value Object)
    /// </summary>
    public ICollection<FotosAvaliacao> FotosAvaliacao { get; set; } = new List<FotosAvaliacao>();

    // Navigation Properties
    /// <summary>
    /// Navegação para o cliente que fez a avaliação
    /// </summary>
    public Cliente? Cliente { get; set; }

    /// <summary>
    /// Construtor padrão que inicializa as propriedades de auditoria
    /// </summary>
    public Avaliacao()
    {
        Id = Guid.NewGuid();
        CriadoEm = DateTime.UtcNow;
    }

    /// <summary>
    /// Construtor completo com validações
    /// </summary>
    /// <param name="produtoId">ID do produto avaliado</param>
    /// <param name="clienteId">ID do cliente que avalia</param>
    /// <param name="nota">Nota de 1 a 5</param>
    /// <param name="texto">Texto da avaliação</param>
    public Avaliacao(Guid produtoId, Guid clienteId, int nota, string texto)
    {
        ValidarDados(produtoId, clienteId, nota, texto);

        Id = Guid.NewGuid();
        ProdutoId = produtoId;
        ClienteId = clienteId;
        Nota = nota;
        Texto = texto;
        CriadoEm = DateTime.UtcNow;
        FotosAvaliacao = new List<FotosAvaliacao>();
    }

    /// <summary>
    /// Valida os dados da avaliação
    /// </summary>
    private static void ValidarDados(Guid produtoId, Guid clienteId, int nota, string texto)
    {
        if (produtoId == Guid.Empty)
            throw new ArgumentException("ProdutoId não pode ser vazio", nameof(produtoId));

        if (clienteId == Guid.Empty)
            throw new ArgumentException("ClienteId não pode ser vazio", nameof(clienteId));

        if (nota < 1 || nota > 5)
            throw new ArgumentException("Nota deve estar entre 1 e 5", nameof(nota));

        if (string.IsNullOrWhiteSpace(texto))
            throw new ArgumentException("Texto da avaliação não pode ser vazio", nameof(texto));
    }

    /// <summary>
    /// Adiciona uma foto à avaliação
    /// </summary>
    /// <param name="foto">Foto a ser adicionada</param>
    public void AdicionarFoto(FotosAvaliacao foto)
    {
        if (foto == null)
            throw new ArgumentNullException(nameof(foto));

        FotosAvaliacao.Add(foto);
    }
}
