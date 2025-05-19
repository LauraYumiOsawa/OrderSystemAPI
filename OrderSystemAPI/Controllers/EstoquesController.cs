using Microsoft.AspNetCore.Mvc;
using OrderSystemAPI.Data;
using OrderSystemAPI.Models;

[ApiController]
[Route("[controller]")]
public class EstoqueController : ControllerBase
{
    private readonly AppDbContext _context;

    public EstoqueController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("reservar")]
    public async Task<ActionResult<Estoque>> ReservarEstoque([FromBody] Estoque reserva)
    {
        if (reserva.PedidoId == null)
        {
            return BadRequest();
        }

        var pedido = await _context.Pedidos.FindAsync(reserva.PedidoId);
        if (pedido == null)
        {
            return NotFound();
        }

        var estoque = new Estoque
        {
            PedidoId = reserva.PedidoId,
            StatusEstoque = "Reservado",
            ItensEstoque = reserva.ItensEstoque,
            DataReserva = DateTime.Now
        };

        _context.Estoques.Add(estoque);
        await _context.SaveChangesAsync();

        return Ok(estoque);
    }
}