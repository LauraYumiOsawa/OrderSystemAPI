namespace OrderSystemAPI.Models;

public class Alerta
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ClienteId { get; set; }
    public string Mensagem { get; set; }
    public string Email { get; set; }
}