// Local: sprintcsharp/Services/ApiClientService.cs
using Newtonsoft.Json;
using sprintcsharp.Models; // Importa o novo modelo

namespace sprintcsharp.Services;

public class ApiClientService
{
    private readonly HttpClient _httpClient;
    // URL base da API externa (do seu YAML)
    private const string BaseUrl = "https://assessor-virtual-api.onrender.com/api";

    public ApiClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    // Método que busca os produtos da API externa
    public async Task<List<ProdutoApiExterna>?> GetProdutosDaApiExterna()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/investimentos");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                // Deserializa o JSON na nossa classe
                return JsonConvert.DeserializeObject<List<ProdutoApiExterna>>(json);
            }
            return null; // Retorna nulo se a API falhar
        }
        catch (Exception ex)
        {
            // Logar o erro (no console, por enquanto)
            Console.WriteLine($"Erro ao chamar API externa: {ex.Message}");
            return null;
        }
    }
}