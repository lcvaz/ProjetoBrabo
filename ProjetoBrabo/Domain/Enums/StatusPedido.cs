namespace Domain.Enums;

/// <summary>
/// Status do ciclo de vida de um pedido
/// </summary>
public enum StatusPedido
{
    Carrinho = 0,           // Ainda montando o pedido
    AguardandoPagamento = 1, // Finalizou mas não pagou
    Pago = 2,               // Pagamento confirmado - SUBTRAI ESTOQUE
    Preparando = 3,         // Separando produtos
    Enviado = 4,            // Em trânsito
    Entregue = 5,           // Finalizado com sucesso
    Cancelado = 6           // Cancelado - DEVOLVE ESTOQUE
}