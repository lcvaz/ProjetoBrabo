using Domain.ValueObject;

namespace Domain.Interfaces;

/// <summary>
/// Interface para serviços externos de cálculo de distância (Google Maps, etc)
/// Implementação ficará na Infrastructure Layer
/// </summary>
public interface IApiDistancia
{
    /// <summary>
    /// Calcula a distância em quilômetros entre dois endereços
    /// </summary>
    /// <param name="origem">Endereço de origem</param>
    /// <param name="destino">Endereço de destino</param>
    /// <returns>Distância em quilômetros</returns>
    Task<decimal> CalcularDistancia(Endereco origem, Endereco destino);
}
