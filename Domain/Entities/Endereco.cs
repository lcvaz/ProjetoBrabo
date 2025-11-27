namespace Domain.Entities;

/// <summary>
/// Value Object que representa um endere�o.
/// � imut�vel e sempre associado a uma entidade (Cliente, Loja, Pedido).
/// N�o possui ID pr�prio, sendo embedded na entidade que o cont�m.
/// </summary>
public class Endereco
{
    /// <summary>
    /// Nome da rua/avenida/logradouro
    /// </summary>
    public string Rua { get; private set; } = string.Empty;

    /// <summary>
    /// Numero do endereco
    /// </summary>
    public string Numero { get; private set; } = string.Empty;

    /// <summary>
    /// Complemento opcional (apartamento, bloco, etc)
    /// </summary>
    public string? Complemento { get; private set; }

    /// <summary>
    /// Bairro do endere�o
    /// </summary>
    public string Bairro { get; private set; } = string.Empty;

    /// <summary>
    /// C�digo de Endere�amento Postal (CEP) no formato XXXXX-XXX
    /// </summary>
    public string Cep { get; private set; } = string.Empty;

    /// <summary>
    /// Cidade/munic�pio onde o endere�o est� localizado
    /// </summary>
    public string Cidade { get; private set; } = string.Empty;

    /// <summary>
    /// Estado/unidade federativa (ex: SP, RJ, MG) em formato de sigla
    /// </summary>
    public string Estado { get; private set; } = string.Empty;

    /// <summary>
    /// Construtor privado para EF Core (shadow state)
    /// </summary>
    private Endereco() { }

    /// <summary>
    /// Construtor que cria um novo endere�o com valida��es
    /// </summary>
    /// <param name="rua">Nome da rua</param>
    /// <param name="numero">N�mero do endere�o</param>
    /// <param name="bairro">Bairro do endere�o</param>
    /// <param name="cep">CEP no formato XXXXX-XXX</param>
    /// <param name="cidade">Cidade do endere�o</param>
    /// <param name="estado">Estado em formato de sigla (2 caracteres)</param>
    /// <param name="complemento">Complemento opcional</param>
    public Endereco(string rua, string numero, string bairro, string cep, string cidade, string estado, string? complemento = null)
    {
        ValidarDados(rua, numero, bairro, cep, cidade, estado);

        Rua = rua;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cep = cep;
        Cidade = cidade;
        Estado = estado;
    }

    /// <summary>
    /// Valida os dados do endere�o
    /// </summary>
    private static void ValidarDados(string rua, string numero, string bairro, string cep, string cidade, string estado)
    {
        if (string.IsNullOrWhiteSpace(rua))
            throw new ArgumentException("Rua n�o pode ser vazia", nameof(rua));

        if (string.IsNullOrWhiteSpace(numero))
            throw new ArgumentException("N�mero n�o pode ser vazio", nameof(numero));

        if (string.IsNullOrWhiteSpace(bairro))
            throw new ArgumentException("Bairro n�o pode ser vazio", nameof(bairro));

        if (string.IsNullOrWhiteSpace(cep))
            throw new ArgumentException("CEP n�o pode ser vazio", nameof(cep));

        if (!ValidarCep(cep))
            throw new ArgumentException("CEP inv�lido. Use o formato XXXXX-XXX", nameof(cep));

        if (string.IsNullOrWhiteSpace(cidade))
            throw new ArgumentException("Cidade n�o pode ser vazia", nameof(cidade));

        if (string.IsNullOrWhiteSpace(estado) || estado.Length != 2)
            throw new ArgumentException("Estado deve ser uma sigla com 2 caracteres (ex: SP, RJ)", nameof(estado));
    }

    /// <summary>
    /// Valida o formato do CEP brasileiro (XXXXX-XXX)
    /// </summary>
    private static bool ValidarCep(string cep)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(cep, @"^\d{5}-\d{3}$");
    }

    /// <summary>
    /// Retorna o endere�o formatado como string
    /// </summary>
    public override string ToString()
    {
        var complementoFormatado = string.IsNullOrWhiteSpace(Complemento) ? string.Empty : $", {Complemento}";
        return $"{Rua}, {Numero}{complementoFormatado} - {Bairro}, {Cidade} - {Estado}, {Cep}";
    }

    /// <summary>
    /// Compara dois Value Objects de endere�o pelo valor de suas propriedades
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not Endereco endereco)
            return false;

        return Rua == endereco.Rua &&
               Numero == endereco.Numero &&
               Complemento == endereco.Complemento &&
               Bairro == endereco.Bairro &&
               Cep == endereco.Cep &&
               Cidade == endereco.Cidade &&
               Estado == endereco.Estado;
    }

    /// <summary>
    /// Gera hash code baseado nas propriedades do endere�o
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Rua, Numero, Complemento, Bairro, Cep, Cidade, Estado);
    }
}
