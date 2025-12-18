using Domain.Enums;
using Domain.Interfaces;
using Domain.ValueObject;

namespace Domain.Services;

/// <summary>
/// Serviço de domínio responsável por calcular frete entre dois endereços
/// Reutilizável em qualquer contexto que precise calcular frete
/// </summary>
public class ServicoCalculoFrete
{
    private readonly IApiDistancia _apiDistancia;
    
    public ServicoCalculoFrete(IApiDistancia apiDistancia)
    {
        _apiDistancia = apiDistancia ?? throw new ArgumentNullException(nameof(apiDistancia));
    }
    
    /// <summary>
    /// Calcula o frete entre dois endereços aplicando a regra de taxação
    /// </summary>
    /// <param name="origem">Endereço de origem (de onde sai a mercadoria)</param>
    /// <param name="destino">Endereço de destino (para onde vai a mercadoria)</param>
    /// <param name="tipoTarifa">Tipo de taxação a ser aplicada</param>
    /// <param name="valorTarifa">Valor base da tarifa</param>
    /// <returns>Valor do frete calculado</returns>
    public async Task<decimal> CalcularFrete(
        Endereco origem,
        Endereco destino,
        TipoTarifa tipoTarifa,
        decimal valorTarifa)
    {
        if (origem == null)
            throw new ArgumentNullException(nameof(origem));
        
        if (destino == null)
            throw new ArgumentNullException(nameof(destino));
        
        if (valorTarifa < 0)
            throw new ArgumentException("Valor da tarifa não pode ser negativo", nameof(valorTarifa));
        
        // Se for frete grátis, retorna zero direto (não precisa calcular distância)
        if (tipoTarifa == TipoTarifa.Gratis)
            return 0m;
        
        // Se for tarifa fixa, retorna o valor direto (não precisa calcular distância)
        if (tipoTarifa == TipoTarifa.Fixa)
            return valorTarifa;
        
        // Para outros tipos, precisa calcular a distância
        var distanciaEmKm = await _apiDistancia.CalcularDistancia(origem, destino);
        
        if (distanciaEmKm < 0)
            throw new InvalidOperationException("Distância calculada não pode ser negativa");
        
        return tipoTarifa switch
        {
            TipoTarifa.PorKm => valorTarifa * distanciaEmKm,
            TipoTarifa.PorFaixaDistancia => CalcularPorFaixaDistancia(distanciaEmKm, valorTarifa),
            _ => throw new InvalidOperationException($"Tipo de tarifa '{tipoTarifa}' não suportado")
        };
    }
    
    /// <summary>
    /// Calcula frete usando faixas de distância
    /// TODO: Implementar com configuração de faixas personalizáveis
    /// </summary>
    private decimal CalcularPorFaixaDistancia(decimal distanciaEmKm, decimal valorBase)
    {
        // Implementação futura: permitir configuração de faixas
        // Por enquanto, lança exceção
        throw new NotImplementedException("Cálculo por faixa de distância será implementado futuramente");
    }
}
