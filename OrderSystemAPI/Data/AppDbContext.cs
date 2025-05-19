using Microsoft.EntityFrameworkCore;
using OrderSystemAPI.Models;

namespace OrderSystemAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }
    public DbSet<Estoque> Estoques { get; set; }
    public DbSet<Alerta> Alertas { get; set; }

}