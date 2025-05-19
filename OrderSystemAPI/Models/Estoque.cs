using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderSystemAPI.Models;

public class Estoque
{
    public int Id { get; set; }
    public int? PedidoId { get; set; }
    public string StatusEstoque { get; set; }
    public List<ItemEstoque> ItensEstoque { get; set; }
    public DateTime DataReserva { get; set; }
}

public class ItemEstoque
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string Nome { get; set; }
    public int Quantidade { get; set; }
}
