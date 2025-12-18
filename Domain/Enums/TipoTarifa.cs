namespace Domain.Enums;

/// <summary>
/// Define como a loja aplica sua tarifa de frete
/// </summary>
public enum TipoTarifa
{
    PorKm = 1,              // R$ X por quilômetro
    Fixa = 2,               // Valor fixo independente da distância
    PorFaixaDistancia = 3,  // Tabela de faixas (0-5km, 5-10km, etc)
    Gratis = 4              // Frete grátis
}