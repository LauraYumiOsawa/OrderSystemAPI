using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystemAPI.Data;
using OrderSystemAPI.Models;

[ApiController]
[Route("[controller]")]
public class PedidosController : ControllerBase
{
    private readonly AppDbContext _context;

    public PedidosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Pedido>> CriarPedido([FromBody] Pedido pedido)
    {
        if (pedido.ClienteId == null || pedido.ItensPedidos == null)
        {
            return BadRequest();
        }

        pedido.ValorTotal = CalcularValorTotal(pedido.ItensPedidos);
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(CriarPedido), new { id = pedido.PedidoId }, pedido);
    }

    private decimal CalcularValorTotal(List<ItemPedido> itens)
    {
        decimal total = 0;
        foreach (var item in itens)
        {
            total += item.Quantidade * item.ValorParcial;
        }
        return total;
    }
}