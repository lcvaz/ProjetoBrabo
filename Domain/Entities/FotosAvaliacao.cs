namespace Domain.Entities;

/// <summary>
/// Value Object que representa uma foto anexada a uma avaliação.
/// É imutável e sempre associado a uma entidade Avaliacao.
/// Preparado para futuras análises de Machine Learning.
/// </summary>
public class FotosAvaliacao
{
    /// <summary>
    /// URL onde a foto está armazenada (cloud storage)
    /// </summary>
    public string UrlFoto { get; private set; } = string.Empty;

    /// <summary>
    /// Texto alternativo opcional para acessibilidade
    /// </summary>
    public string? AltText { get; private set; }

    /// <summary>
    /// Ordem de exibição da foto na avaliação (1, 2, 3...)
    /// </summary>
    public int Ordem { get; private set; }

    /// <summary>
    /// Data e hora em que a foto foi enviada (UTC)
    /// </summary>
    public DateTime DataUpload { get; private set; }

    /// <summary>
    /// Construtor privado para EF Core (shadow state)
    /// </summary>
    private FotosAvaliacao() { }

    /// <summary>
    /// Construtor que cria uma nova foto com validações
    /// </summary>
    /// <param name="urlFoto">URL da foto armazenada</param>
    /// <param name="ordem">Ordem de exibição</param>
    /// <param name="altText">Texto alternativo opcional</param>
    public FotosAvaliacao(string urlFoto, int ordem, string? altText = null)
    {
        ValidarDados(urlFoto, ordem);

        UrlFoto = urlFoto;
        Ordem = ordem;
        AltText = altText;
        DataUpload = DateTime.UtcNow;
    }

    /// <summary>
    /// Valida os dados da foto
    /// </summary>
    private static void ValidarDados(string urlFoto, int ordem)
    {
        if (string.IsNullOrWhiteSpace(urlFoto))
            throw new ArgumentException("URL da foto não pode ser vazia", nameof(urlFoto));

        if (!Uri.TryCreate(urlFoto, UriKind.Absolute, out _))
            throw new ArgumentException("URL da foto deve ser válida", nameof(urlFoto));

        if (ordem < 1)
            throw new ArgumentException("Ordem deve ser maior que zero", nameof(ordem));
    }

    /// <summary>
    /// Retorna a representação em string da foto
    /// </summary>
    public override string ToString()
    {
        var alt = string.IsNullOrWhiteSpace(AltText) ? "Sem descrição" : AltText;
        return $"Foto #{Ordem}: {UrlFoto} ({alt})";
    }

    /// <summary>
    /// Compara dois Value Objects de foto pelo valor de suas propriedades
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not FotosAvaliacao foto)
            return false;

        return UrlFoto == foto.UrlFoto &&
               Ordem == foto.Ordem &&
               AltText == foto.AltText;
    }

    /// <summary>
    /// Gera hash code baseado nas propriedades da foto
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(UrlFoto, Ordem, AltText);
    }
}
