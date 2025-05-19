using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;

public class EstoqueSubscriber
{
    public static void Main()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri("amqps://rktqsjyl:xjNUMhnnTuKdKV21t5Reb8rLXFCOvEJT@jackal.rmq.cloudamqp.com/rktqsjyl")
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        string exchangeName = "direct_logs";
        string queueName = channel.QueueDeclare().QueueName;
        string routingKey = "pedido.criado";
        string estoqueRoutingKey = "estoque.reservado";

        channel.ExchangeDeclare(exchange: exchangeName, type: "direct");
        channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var pedido = JsonSerializer.Deserialize<PedidoCriadoMessage>(message);

            var estoqueReservado = new
            {
                pedido_id = pedido.pedido_id,
                status_estoque = "reservado",
                itens_reservados = pedido.itens,
                data_reserva = DateTime.UtcNow.ToString("o")
            };

            var estoqueMsg = JsonSerializer.Serialize(estoqueReservado);
            var estoqueBody = Encoding.UTF8.GetBytes(estoqueMsg);

            channel.BasicPublish(
                exchange: exchangeName,
                routingKey: estoqueRoutingKey,
                basicProperties: null,
                body: estoqueBody
            );

            using var http = new HttpClient();
            var content = new StringContent(estoqueMsg, Encoding.UTF8, "application/json");
            var response = await http.PostAsync("http://localhost:5108/estoque/reservar", content);
            Console.WriteLine($"Estoque reservado para pedido {pedido.pedido_id}. Status API: {response.StatusCode}");
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        Console.WriteLine("Aguardando pedidos para reservar estoque...");
        Console.ReadLine();
    }

    public class PedidoCriadoMessage
    {
        public string pedido_id { get; set; }
        public string cliente_id { get; set; }
        public List<Item> itens { get; set; }
        public decimal valor_total { get; set; }
    }

    public class Item
    {
        public string produto_id { get; set; }
        public int quantidade { get; set; }
    }
}