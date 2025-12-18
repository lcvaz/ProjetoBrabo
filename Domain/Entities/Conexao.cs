using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Representa uma conexão entre dois vendedores no feed social B2B.
/// Implementa sistema de solicitação/aprovação similar ao LinkedIn.
/// </summary>
public class Conexao
{
    /// <summary>
    /// Identificador único da conexão
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID do vendedor que iniciou a solicitação de conexão
    /// </summary>
    public Guid VendedorInicianteId { get; set; }

    /// <summary>
    /// ID do vendedor que recebeu a solicitação de conexão
    /// </summary>
    public Guid VendedorDestinoId { get; set; }

    /// <summary>
    /// Status atual da conexão (Pendente, Aceita, Rejeitada, Bloqueada)
    /// </summary>
    public StatusConexao Status { get; set; }

    /// <summary>
    /// Data e hora em que a solicitação foi criada (UTC)
    /// </summary>
    public DateTime CriadoEm { get; set; }

    /// <summary>
    /// Data e hora da última atualização do status (UTC)
    /// </summary>
    public DateTime AtualizadoEm { get; set; }

    /// <summary>
    /// Navigation property para o vendedor que iniciou a conexão
    /// </summary>
    public Vendedor VendedorIniciante { get; set; } = null!;

    /// <summary>
    /// Navigation property para o vendedor que recebeu a solicitação
    /// </summary>
    public Vendedor VendedorDestino { get; set; } = null!;

    /// <summary>
    /// Construtor padrão para Entity Framework
    /// </summary>
    public Conexao()
    {
        Id = Guid.NewGuid();
        Status = StatusConexao.Pendente;
        CriadoEm = DateTime.UtcNow;
        AtualizadoEm = DateTime.UtcNow;
    }

    /// <summary>
    /// Construtor para criar uma nova solicitação de conexão
    /// </summary>
    /// <param name="vendedorInicianteId">ID do vendedor que está enviando a solicitação</param>
    /// <param name="vendedorDestinoId">ID do vendedor que receberá a solicitação</param>
    /// <exception cref="ArgumentException">Lançada quando os IDs são inválidos ou iguais</exception>
    public Conexao(Guid vendedorInicianteId, Guid vendedorDestinoId) : this()
    {
        if (vendedorInicianteId == Guid.Empty)
            throw new ArgumentException("ID do vendedor iniciante não pode ser vazio", nameof(vendedorInicianteId));

        if (vendedorDestinoId == Guid.Empty)
            throw new ArgumentException("ID do vendedor destino não pode ser vazio", nameof(vendedorDestinoId));

        if (vendedorInicianteId == vendedorDestinoId)
            throw new ArgumentException("Vendedor não pode se conectar consigo mesmo");

        VendedorInicianteId = vendedorInicianteId;
        VendedorDestinoId = vendedorDestinoId;
    }

    /// <summary>
    /// Verifica se a conexão está pendente de aprovação
    /// </summary>
    public bool EstaPendente => Status == StatusConexao.Pendente;

    /// <summary>
    /// Verifica se a conexão está ativa (aceita)
    /// </summary>
    public bool EstaAtiva => Status == StatusConexao.Aceita;

    /// <summary>
    /// Verifica se a conexão foi rejeitada
    /// </summary>
    public bool FoiRejeitada => Status == StatusConexao.Rejeitada;

    /// <summary>
    /// Verifica se a conexão está bloqueada
    /// </summary>
    public bool EstaBloqueada => Status == StatusConexao.Bloqueada;

    /// <summary>
    /// Aceita a solicitação de conexão
    /// </summary>
    /// <exception cref="InvalidOperationException">Lançada quando a conexão não está pendente</exception>
    public void Aceitar()
    {
        if (!EstaPendente)
            throw new InvalidOperationException("Apenas conexões pendentes podem ser aceitas");

        Status = StatusConexao.Aceita;
        AtualizadoEm = DateTime.UtcNow;
    }

    /// <summary>
    /// Rejeita a solicitação de conexão
    /// </summary>
    /// <exception cref="InvalidOperationException">Lançada quando a conexão não está pendente</exception>
    public void Rejeitar()
    {
        if (!EstaPendente)
            throw new InvalidOperationException("Apenas conexões pendentes podem ser rejeitadas");

        Status = StatusConexao.Rejeitada;
        AtualizadoEm = DateTime.UtcNow;
    }

    /// <summary>
    /// Bloqueia a conexão (pode ser feito a qualquer momento)
    /// </summary>
    public void Bloquear()
    {
        if (EstaBloqueada)
            return; // Já está bloqueada

        Status = StatusConexao.Bloqueada;
        AtualizadoEm = DateTime.UtcNow;
    }

    /// <summary>
    /// Verifica se um vendedor específico é participante desta conexão
    /// </summary>
    /// <param name="vendedorId">ID do vendedor a verificar</param>
    /// <returns>True se o vendedor é iniciante ou destino da conexão</returns>
    public bool EnvolveVendedor(Guid vendedorId)
    {
        return VendedorInicianteId == vendedorId || VendedorDestinoId == vendedorId;
    }

    /// <summary>
    /// Obtém o ID do outro vendedor na conexão
    /// </summary>
    /// <param name="vendedorId">ID do vendedor atual</param>
    /// <returns>ID do outro vendedor na conexão</returns>
    /// <exception cref="ArgumentException">Lançada quando o vendedor não participa desta conexão</exception>
    public Guid ObterOutroVendedor(Guid vendedorId)
    {
        if (VendedorInicianteId == vendedorId)
            return VendedorDestinoId;

        if (VendedorDestinoId == vendedorId)
            return VendedorInicianteId;

        throw new ArgumentException("Vendedor não participa desta conexão", nameof(vendedorId));
    }
}
