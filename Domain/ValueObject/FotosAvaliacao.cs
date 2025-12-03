namespace Domain.ValueObject;

public class FotosAvaliacao
{
    ///<summary>
    /// Este é o ValueObject pois uma foto ou varias para uma avaliacao é um objeto complexo e
    /// são salvos junto com a entidade no banco de dados
    ///</summary> 
     
    public string Url { get; private set; } = string.Empty;

    private FotosAvaliacao(string Url)
    {
        this.Url = Url; 
    }

    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static FotosAvaliacao Criar(string url)
{
    ValidaUrl(url);
    return new FotosAvaliacao(url);
}

    private static void ValidaUrl(String Url)
    {
        if (string.IsNullOrWhiteSpace(Url))
            throw new ArgumentException("URL da foto não pode ser vazia", nameof(Url));

        if (!Uri.TryCreate(Url, UriKind.Absolute, out var url))
            throw new ArgumentException("URL da foto é inválida", nameof(Url));  

        var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        var extensao = Path.GetExtension(url.AbsolutePath).ToLowerInvariant();      
        if (!extensoesPermitidas.Contains(extensao))
            throw new ArgumentException(
                $"Extensão de imagem não suportada. Use: {string.Join(", ", extensoesPermitidas)}", 
                nameof(url));
    }
    
    /// <summary>
    /// Compara dois Value Objects de foto pelo valor de suas propriedades
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not FotosAvaliacao foto)
            return false;

        return Url == foto.Url;
    }

    /// <summary>
    /// Gera hash code baseado nas propriedades da foto
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Url);
    }
}

