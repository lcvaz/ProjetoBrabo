using Domain.ValueObject;

namespace Domain.Entities;

public class Avaliacao 
{
    ///<summary>
    /// Uma avaliação terá um padrão de suportar até 5 fotos , um texto de 1000 caracteres 
    /// e uma avalização de 0 a 5  
    ///</summary> 
     
    private const int MaximoFotos = 5;
    private const int NotaMaxima = 5;
    private const int MaximoCaracteres = 1000;
     
    public Guid Id { get; private set; } 
    public Guid ProdutoId { get; private set; } = Guid.Empty;
    public Guid UsuarioId { get; private set; } = Guid.Empty;
    public int Nota { get; private set; }
    public string? Texto { get; private set; }
    public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;


    private readonly List<FotosAvaliacao> _fotos = new();
    public IReadOnlyCollection<FotosAvaliacao> Fotos => _fotos.AsReadOnly();   

    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="produtoId"></param>
    /// <param name="usuarioId"></param>
    /// <param name="nota"></param>
    /// <param name="texto"></param>
    private Avaliacao(Guid produtoId, Guid usuarioId, int nota, string? texto)
    {
        Id = Guid.NewGuid();
        ProdutoId = produtoId;
        UsuarioId = usuarioId;
        Nota = nota;
        Texto = texto;
        DataCriacao = DateTime.UtcNow;
    }

    public static Avaliacao Criar(Guid produtoId, Guid usuarioId, int nota, string? texto){
        ValidarDados(produtoId, usuarioId, nota, texto);
        return new Avaliacao(produtoId, usuarioId, nota, texto);
    }

    /// <summary>
    /// Adiciona uma foto à avaliação
    /// </summary>
    /// <param name="foto">Foto a ser adicionada</param>
    public void AdicionarFoto(FotosAvaliacao foto)
    {
        if (foto == null )
            throw new ArgumentNullException(nameof(foto));

        if (_fotos.Count >= MaximoFotos)
            throw new InvalidOperationException($"Avaliação pode ter no máximo {MaximoFotos} fotos");

        _fotos.Add(foto);
    }

    public void RemoverFoto(FotosAvaliacao foto)
    {
        if (foto == null)
            throw new ArgumentNullException(nameof(foto));
            
        _fotos.Remove(foto);
    }

    private static void ValidarDados(Guid produtoId, Guid usuarioId, int nota, string? texto)
    {
        if (produtoId == Guid.Empty)
            throw new ArgumentException("ProdutoId não pode ser vazio", nameof(produtoId));

        if (usuarioId == Guid.Empty)
            throw new ArgumentException("ClienteId não pode ser vazio", nameof(usuarioId));

        if (nota > NotaMaxima)
            throw new ArgumentException($"Nota deve ser até {NotaMaxima}", nameof(nota));
        
        // ✅ Validação de tamanho do texto (texto é opcional, mas se informado deve respeitar limite)
        if (!string.IsNullOrWhiteSpace(texto) && texto.Length > MaximoCaracteres)
            throw new ArgumentException($"Texto não pode ter mais de {MaximoCaracteres} caracteres", nameof(texto));    
    }

    // Uma Entity deve ter esses métodos
    public override bool Equals(object? obj)
    {
        if (obj is not Avaliacao other)
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