namespace Domain.ValueObject;

/// <summary>
/// Value Object que representa o frete cobrado por uma loja específica
/// </summary>
public class FreteLoja
{
    public Guid LojaId { get; }
    public decimal Valor { get; }
    
    private FreteLoja(Guid lojaId, decimal valor)
    {
        LojaId = lojaId;
        Valor = valor;
    }
    
    public static FreteLoja Criar(Guid lojaId, decimal valor)
    {
        if (lojaId == Guid.Empty)
            throw new ArgumentException("LojaId não pode ser vazio", nameof(lojaId));
        
        if (valor < 0)
            throw new ArgumentException("Valor do frete não pode ser negativo", nameof(valor));
        
        return new FreteLoja(lojaId, valor);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not FreteLoja other)
            return false;
        
        return LojaId == other.LojaId && Valor == other.Valor;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(LojaId, Valor);
    }
}