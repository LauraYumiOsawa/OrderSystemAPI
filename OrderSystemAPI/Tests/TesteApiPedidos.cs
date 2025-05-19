using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class TesteApiPedidos
{
    static async Task Main()
    {
        var pedidoJson = @"{
            ""pedido_id"": ""12345"",
            ""cliente_id"": ""abcde"",
            ""itens"": [
                { ""produto_id"": ""p1"", ""quantidade"": 2 },
                { ""produto_id"": ""p2"", ""quantidade"": 1 }
            ],
            ""valor_total"": 150.00
        }";

        using var http = new HttpClient();
        var content = new StringContent(pedidoJson, Encoding.UTF8, "application/json");
        var response = await http.PostAsync("http://localhost:5108/pedidos", content);
        var respBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Status: {response.StatusCode} - Body: {respBody}");
    }
}