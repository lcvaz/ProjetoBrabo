namespace Domain.ValueObject;

public class FotosAvaliacao
{
    ///<summary>
    /// Este é o ValueObject pois uma foto ou varias para uma avaliacao é um objeto complexo e
    /// são salvos junto com a entidade no banco de dados
    ///</summary> 
     
    public string Url { get; set; } = string.Empty;

    public FotosAvaliacao(string Url)
    {
        ValidaUrl(Url);
        this.Url = Url; 
    }

    private void ValidaUrl(String Url)
    {
        if (string.IsNullOrWhiteSpace(Url))
            throw new ArgumentException("URL da foto não pode ser vazia", nameof(Url));
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

