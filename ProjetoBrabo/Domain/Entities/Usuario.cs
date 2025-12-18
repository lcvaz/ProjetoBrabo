namespace Domain.Entities;

using Domain.Enums;

/// <summary>
/// Classe base abstrata que define as propriedades comuns a todos os tipos de usuários.
/// Segue o padrão de herança Table-per-Type do Entity Framework Core.
/// </summary>
public abstract class Usuario
{
    /// <summary>
    /// Identificador único do usuário
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome completo do usuário
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email único para acesso e comunicação
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hash da senha do usuário (nunca armazenar senha em texto puro)
    /// </summary>
    public string SenhaHash { get; set; } = string.Empty;

    /// <summary>
    /// Data e hora em que o usuário foi criado (UTC)
    /// </summary>
    public DateTime CriadoEm { get; set; }

    /// <summary>
    /// Data e hora da última atualização do usuário (UTC)
    /// </summary>
    public DateTime AtualizadoEm { get; set; }

    /// <summary>
    /// Status atual do usuário no sistema
    /// </summary>
    public StatusUsuario StatusUsuario { get; set; } = StatusUsuario.Ativo;

    /// <summary>
    /// Tipo de usuário (Admin, Cliente, Vendedor) - define as permissões e funcionalidades
    /// </summary>
    public TipoUsuario TipoUsuario { get; set; }

    /// <summary>
    /// Construtor protegido que inicializa as propriedades de auditoria
    /// </summary>
    protected Usuario()
    {
        Id = Guid.NewGuid();
        CriadoEm = DateTime.UtcNow;
        AtualizadoEm = DateTime.UtcNow;
    }
} 