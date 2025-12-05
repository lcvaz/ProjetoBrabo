using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Agregado raiz que representa um pedido no marketplace
/// Pode conter produtos de múltiplas lojas
/// </summary>
public class Pedido
{
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public StatusPedido Status { get; private set; }
    public FormaPagamento? FormaPagamento { get; private set; }
    public Endereco EnderecoEntrega { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataPagamento { get; private set; }
    
    private readonly List<ItemPedido> _itens = new();
    public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();
    
    private readonly List<FreteLoja> _fretes = new();
    public IReadOnlyCollection<FreteLoja> Fretes => _fretes.AsReadOnly();
    
    // Valores calculados
    public decimal ValorItens => _itens.Sum(i => i.Subtotal);
    public decimal ValorFrete => _fretes.Sum(f => f.Valor);
    public decimal ValorTotal => ValorItens + ValorFrete;
    
    private Pedido(Guid usuarioId, Endereco enderecoEntrega)
    {
        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        EnderecoEntrega = enderecoEntrega;
        Status = StatusPedido.Carrinho;
        DataCriacao = DateTime.UtcNow;
    }
    
    public static Pedido Criar(Guid usuarioId, Endereco enderecoEntrega)
    {
        if (usuarioId == Guid.Empty)
            throw new ArgumentException("UsuarioId não pode ser vazio", nameof(usuarioId));
        
        if (enderecoEntrega == null)
            throw new ArgumentNullException(nameof(enderecoEntrega));
        
        return new Pedido(usuarioId, enderecoEntrega);
    }
    
     /// <summary>
    /// Adiciona um item ao pedido
    /// VERIFICA estoque mas NÃO subtrai
    /// </summary>
    public void AdicionarItem(Guid produtoId, Guid lojaId, int quantidade, decimal precoUnitario, int estoqueDisponivel)
    {
        if (Status != StatusPedido.Carrinho)
            throw new InvalidOperationException("Só é possível adicionar itens em pedidos com status Carrinho");
        
        // Verificação preventiva de estoque
        if (quantidade > estoqueDisponivel)
            throw new InvalidOperationException($"Quantidade solicitada ({quantidade}) maior que estoque disponível ({estoqueDisponivel})");
        
        // Verifica se já existe item deste produto no pedido
        var itemExistente = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        
        if (itemExistente != null)
        {
            // Atualiza quantidade do item existente
            var novaQuantidade = itemExistente.Quantidade + quantidade;
            
            if (novaQuantidade > estoqueDisponivel)
                throw new InvalidOperationException($"Quantidade total ({novaQuantidade}) maior que estoque disponível ({estoqueDisponivel})");
            
            itemExistente.AtualizarQuantidade(novaQuantidade);
        }
        else
        {
            // Adiciona novo item
            var novoItem = ItemPedido.Criar(Id, produtoId, lojaId, quantidade, precoUnitario);
            _itens.Add(novoItem);
        }
    }
    
    /// <summary>
    /// Remove um item do pedido
    /// </summary>
    public void RemoverItem(Guid itemId)
    {
        if (Status != StatusPedido.Carrinho)
            throw new InvalidOperationException("Só é possível remover itens em pedidos com status Carrinho");
        
        var item = _itens.FirstOrDefault(i => i.Id == itemId);
        
        if (item == null)
            throw new ArgumentException("Item não encontrado no pedido", nameof(itemId));
        
        _itens.Remove(item);
    }
    
    /// <summary>
    /// Atualiza a quantidade de um item
    /// </summary>
    public void AtualizarQuantidadeItem(Guid itemId, int novaQuantidade, int estoqueDisponivel)
    {
        if (Status != StatusPedido.Carrinho)
            throw new InvalidOperationException("Só é possível atualizar itens em pedidos com status Carrinho");
        
        var item = _itens.FirstOrDefault(i => i.Id == itemId);
        
        if (item == null)
            throw new ArgumentException("Item não encontrado no pedido", nameof(itemId));
        
        if (novaQuantidade > estoqueDisponivel)
            throw new InvalidOperationException($"Quantidade solicitada ({novaQuantidade}) maior que estoque disponível ({estoqueDisponivel})");
        
        item.AtualizarQuantidade(novaQuantidade);
    }
    
    /// <summary>
    /// Adiciona frete de uma loja específica
    /// </summary>
    public void AdicionarFrete(Guid lojaId, decimal valorFrete)
    {
        if (Status != StatusPedido.Carrinho && Status != StatusPedido.AguardandoPagamento)
            throw new InvalidOperationException("Não é possível adicionar frete neste status");
        
        // Remove frete anterior desta loja se existir
        var freteExistente = _fretes.FirstOrDefault(f => f.LojaId == lojaId);
        if (freteExistente != null)
            _fretes.Remove(freteExistente);
        
        var novoFrete = FreteLoja.Criar(lojaId, valorFrete);
        _fretes.Add(novoFrete);
    }
    
    /// <summary>
    /// Finaliza o pedido e coloca em aguardando pagamento
    /// </summary>
    public void FinalizarPedido(FormaPagamento formaPagamento)
    {
        if (Status != StatusPedido.Carrinho)
            throw new InvalidOperationException("Apenas pedidos em Carrinho podem ser finalizados");
        
        if (!_itens.Any())
            throw new InvalidOperationException("Pedido deve ter pelo menos um item");
        
        // Verifica se todas as lojas têm frete calculado
        var lojasDosProdutos = _itens.Select(i => i.LojaId).Distinct();
        var lojasComFrete = _fretes.Select(f => f.LojaId);
        
        var lojasSemFrete = lojasDosProdutos.Except(lojasComFrete).ToList();
        if (lojasSemFrete.Any())
            throw new InvalidOperationException($"Frete não calculado para {lojasSemFrete.Count} loja(s)");
        
        FormaPagamento = formaPagamento;
        Status = StatusPedido.AguardandoPagamento;
    }
    
    /// <summary>
    /// Confirma o pagamento
    /// VALIDA estoque novamente e SUBTRAI
    /// </summary>
    public void ConfirmarPagamento(Dictionary<Guid, int> estoqueAtualPorProduto)
    {
        if (Status != StatusPedido.AguardandoPagamento)
            throw new InvalidOperationException("Apenas pedidos em AguardandoPagamento podem ter pagamento confirmado");
        
        // VALIDAÇÃO CRÍTICA: Verifica se ainda tem estoque de TODOS os produtos
        foreach (var item in _itens)
        {
            if (!estoqueAtualPorProduto.TryGetValue(item.ProdutoId, out var estoqueAtual))
                throw new InvalidOperationException($"Estoque não informado para produto {item.ProdutoId}");
            
            if (item.Quantidade > estoqueAtual)
                throw new InvalidOperationException($"Produto {item.ProdutoId} sem estoque suficiente. Disponível: {estoqueAtual}, Solicitado: {item.Quantidade}");
        }
        
        // Se chegou aqui, tem estoque de tudo - pode confirmar
        Status = StatusPedido.Pago;
        DataPagamento = DateTime.UtcNow;
        
        // IMPORTANTE: Aqui o Application Layer deve subtrair o estoque dos produtos!
    }
    
    /// <summary>
    /// Avança para status Preparando
    /// </summary>
    public void IniciarPreparacao()
    {
        if (Status != StatusPedido.Pago)
            throw new InvalidOperationException("Apenas pedidos Pagos podem iniciar preparação");
        
        Status = StatusPedido.Preparando;
    }
    
    /// <summary>
    /// Avança para status Enviado
    /// </summary>
    public void MarcarComoEnviado()
    {
        if (Status != StatusPedido.Preparando)
            throw new InvalidOperationException("Apenas pedidos em Preparação podem ser enviados");
        
        Status = StatusPedido.Enviado;
    }
    
    /// <summary>
    /// Avança para status Entregue
    /// </summary>
    public void MarcarComoEntregue()
    {
        if (Status != StatusPedido.Enviado)
            throw new InvalidOperationException("Apenas pedidos Enviados podem ser marcados como entregues");
        
        Status = StatusPedido.Entregue;
    }
    
    /// <summary>
    /// Cancela o pedido
    /// Se já estava Pago, DEVOLVE estoque
    /// </summary>
    public void Cancelar()
    {
        if (Status == StatusPedido.Entregue)
            throw new InvalidOperationException("Pedidos entregues não podem ser cancelados");
        
        if (Status == StatusPedido.Cancelado)
            throw new InvalidOperationException("Pedido já está cancelado");
        
        var estavaComEstoqueSubtraido = Status == StatusPedido.Pago || 
                                        Status == StatusPedido.Preparando || 
                                        Status == StatusPedido.Enviado;
        
        Status = StatusPedido.Cancelado;
        
        // IMPORTANTE: Se estavaComEstoqueSubtraido = true, 
        // o Application Layer deve DEVOLVER o estoque dos produtos!
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Pedido other)
            return false;
        
        if (ReferenceEquals(this, other))
            return true;
        
        return Id == other.Id;
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}