// Local: sprintcsharp/Models/ProdutoInvestimento.cs
namespace sprintcsharp.Models; // O namespace corresponde ao local do arquivo

// Esta classe representa a tabela 'investimento_produto' do banco de dados.
public class ProdutoInvestimento
{
    // As propriedades da classe correspondem às colunas da sua tabela.
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Tipo { get; set; }
    public string Risco { get; set; }
    public decimal Preco { get; set; }

    // Este é um método auxiliar para facilitar a exibição do objeto no console.
    public override string ToString()
    {
        // O ':C' formata o preço como moeda (ex: R$ 108,50)
        return $"ID: {Id}, Nome: {Nome}, Tipo: {Tipo}, Risco: {Risco}, Preço: {Preco:C}";
    }
}