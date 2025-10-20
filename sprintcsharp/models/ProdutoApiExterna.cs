// Local: sprintcsharp/Models/ProdutoApiExterna.cs
using Newtonsoft.Json; // Importa o pacote que adicionamos

namespace sprintcsharp.Models;

// Classe para deserializar a resposta da API externa
// (Baseado no schema 'ProdutoInvestimento' do seu YAML)
public class ProdutoApiExterna
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonProperty("tipo")]
    public string Tipo { get; set; } = string.Empty;

    [JsonProperty("risco")]
    public string Risco { get; set; } = string.Empty;

    [JsonProperty("preco")]
    public decimal Preco { get; set; }
}