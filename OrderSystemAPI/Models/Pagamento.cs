namespace OrderSystemAPI.Models;

public class Pagamento
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public string StatusPagamento { get; set; }
    public decimal ValorPago { get; set; }
    public DateTime DataConfirmacao { get; set; }
}