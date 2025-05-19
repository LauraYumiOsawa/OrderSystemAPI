using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using OrderSystemAPI.Models;

class Producer
{
    static void Main()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri("URL_TO_RABBITMQ")
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        string exchangeName = "direct_logs";
        string routingKey = "pedido.criado";

        channel.ExchangeDeclare(exchange: exchangeName, type: "direct");

        // EXEMPLO PRA TESTAR
        var pedido = new Pedido
        {
            PedidoId = 12345,
            ClienteId = 6789,
            ValorTotal = 10,
            ItensPedidos = new List<ItemPedido>
            {
                new ItemPedido { ProdutoId = 1, Quantidade = 2, ValorParcial = 10 },
                new ItemPedido { ProdutoId = 2, Quantidade = 1, ValorParcial = 20 }
            }
        };

        string message = JsonSerializer.Serialize(pedido);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(
            exchange: exchangeName,
            routingKey: routingKey,
            basicProperties: null,
            body: body
        );

        Console.WriteLine($"[x] Sent '{routingKey}':'{message}'");
    }
}