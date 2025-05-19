using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class PedidoSubscriber
{
    private readonly string _queueName = "pedido.criado";
    private readonly string _apiUrl = "URL_TO_API";

    public void Start()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            using var http = new HttpClient();
            var content = new StringContent(message, Encoding.UTF8, "application/json");
            var response = await http.PostAsync(_apiUrl, content);
            Console.WriteLine($"Status: {response.StatusCode}");
        };

        channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

        Console.WriteLine("...");
        Console.ReadLine();
    }
}