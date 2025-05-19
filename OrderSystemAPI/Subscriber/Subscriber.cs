using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

public class PedidoSubscriber
{
    public static void Main()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri("URL_TO_RABBITMQ")
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        string exchangeName = "direct_logs";
        string queueName = channel.QueueDeclare().QueueName;
        string routingKey = "pedido.criado";

        channel.ExchangeDeclare(exchange: exchangeName, type: "direct");
        channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            using var http = new HttpClient();
            var content = new StringContent(message, Encoding.UTF8, "application/json");
            var response = await http.PostAsync("URL_TO_API", content);
            Console.WriteLine($"Status: {response.StatusCode}");
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        Console.WriteLine("...");
        Console.ReadLine();
    }
}