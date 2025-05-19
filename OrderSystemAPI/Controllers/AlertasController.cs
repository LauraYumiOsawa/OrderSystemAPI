using Microsoft.AspNetCore.Mvc;
using OrderSystemAPI.Data;
using OrderSystemAPI.Models;

[ApiController]
[Route("[controller]")]
public class AlertasController : ControllerBase
{
    private readonly AppDbContext _context;

    public AlertasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<string>> EnviarAlerta([FromBody] Alerta Alerta)
    {
        if (Alerta.PedidoId == null || Alerta.Email == null)
        {
            return BadRequest();
        }

        _context.Alertas.Add(Alerta);
        await _context.SaveChangesAsync();

        return Ok("ALERTA!ALGUMACOISA");
    }
}