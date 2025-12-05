namespace Domain.Entities;

/// <summary>
/// Representa um item dentro de um pedido
/// </summary>
public class ItemPedido
{
    public Guid Id { get; private set; }
    public Guid PedidoId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public Guid LojaId { get; private set; }
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }
    public decimal Subtotal => Quantidade * PrecoUnitario;
    
    private ItemPedido(Guid pedidoId, Guid produtoId, Guid lojaId, int quantidade, decimal precoUnitario)
    {
        Id = Guid.NewGuid();
        PedidoId = pedidoId;
        ProdutoId = produtoId;
        LojaId = lojaId;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }
    
    public static ItemPedido Criar(Guid pedidoId, Guid produtoId, Guid lojaId, int quantidade, decimal precoUnitario)
    {
        if (pedidoId == Guid.Empty)
            throw new ArgumentException("PedidoId não pode ser vazio", nameof(pedidoId));
        
        if (produtoId == Guid.Empty)
            throw new ArgumentException("ProdutoId não pode ser vazio", nameof(produtoId));
        
        if (lojaId == Guid.Empty)
            throw new ArgumentException("LojaId não pode ser vazio", nameof(lojaId));
        
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero", nameof(quantidade));
        
        if (precoUnitario <= 0)
            throw new ArgumentException("Preço unitário deve ser maior que zero", nameof(precoUnitario));
        
        return new ItemPedido(pedidoId, produtoId, lojaId, quantidade, precoUnitario);
    }
    
    public void AtualizarQuantidade(int novaQuantidade)
    {
        if (novaQuantidade <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero", nameof(novaQuantidade));
        
        Quantidade = novaQuantidade;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not ItemPedido other)
            return false;
        
        return Id == other.Id;
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}