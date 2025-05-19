namespace OrderSystemAPI.Models;
public class Pedido
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ClienteId { get; set; }
    public List<ItemPedido> ItensPedidos { get; set; }
    public decimal ValorTotal { get; set; }
}

public class ItemPedido
{
    public int Id { get; set; }
    
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorParcial { get; set; }
}
