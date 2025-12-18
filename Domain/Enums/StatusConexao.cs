namespace Domain.Enums;

/// <summary>
/// Define os possíveis estados de uma conexão entre vendedores
/// </summary>
public enum StatusConexao
{
    /// <summary>
    /// Solicitação de conexão enviada, aguardando aprovação
    /// </summary>
    Pendente = 1,

    /// <summary>
    /// Conexão aceita por ambas as partes
    /// </summary>
    Aceita = 2,

    /// <summary>
    /// Solicitação de conexão rejeitada
    /// </summary>
    Rejeitada = 3,

    /// <summary>
    /// Conexão bloqueada por um dos vendedores
    /// </summary>
    Bloqueada = 4
}
