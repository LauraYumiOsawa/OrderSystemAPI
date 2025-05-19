using Microsoft.AspNetCore.Mvc;
using OrderSystemAPI.Data;
using OrderSystemAPI.Models;

[ApiController]
[Route("[controller]")]
public class PagamentosController : ControllerBase
{
    private readonly AppDbContext _context;

    public PagamentosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Pagamento>> ConfirmarPagamento([FromBody] Pagamento pagamento)
    {
        if (pagamento.PedidoId == null)
        {
            return BadRequest();
        }

        var pedido = await _context.Pedidos.FindAsync(pagamento.PedidoId);
        if (pedido == null)
        {
            return NotFound();
        }

        pagamento.StatusPagamento = "Pago";
        pagamento.ValorPago = pedido.ValorTotal;
        _context.Pagamentos.Add(pagamento);
        await _context.SaveChangesAsync();

        return Ok(pagamento);
    }
}