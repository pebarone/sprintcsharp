// Local: sprintcsharp/Data/ProdutoInvestimentoRepository.cs
using sprintcsharp.Models; // <-- Atenção aqui, vamos criar a pasta Models logo em seguida
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace sprintcsharp.Data; // O namespace muda para corresponder ao seu nome de projeto

public class ProdutoInvestimentoRepository
{
    private readonly DatabaseConnection _dbConnection;

    public ProdutoInvestimentoRepository()
    {
        _dbConnection = new DatabaseConnection();
    }

    // READ - Listar todos os produtos
    public List<ProdutoInvestimento> GetAll()
    {
        var produtos = new List<ProdutoInvestimento>();
        using var conn = _dbConnection.GetConnection();

        var cmd = new OracleCommand("SELECT id, nome, tipo, risco, preco FROM investimento_produto ORDER BY id", conn);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            produtos.Add(new ProdutoInvestimento
            {
                Id = Convert.ToInt32(reader["id"]),
                Nome = reader["nome"].ToString()!,
                Tipo = reader["tipo"].ToString()!,
                Risco = reader["risco"].ToString()!,
                Preco = Convert.ToDecimal(reader["preco"])
            });
        }
        return produtos;
    }

    // CREATE - Adicionar um novo produto
    public void Add(ProdutoInvestimento produto)
    {
        using var conn = _dbConnection.GetConnection();
        var sql = "INSERT INTO investimento_produto (nome, tipo, risco, preco) VALUES (:nome, :tipo, :risco, :preco)";
        using var cmd = new OracleCommand(sql, conn);

        cmd.Parameters.Add(new OracleParameter("nome", produto.Nome));
        cmd.Parameters.Add(new OracleParameter("tipo", produto.Tipo));
        cmd.Parameters.Add(new OracleParameter("risco", produto.Risco));
        cmd.Parameters.Add(new OracleParameter("preco", produto.Preco));

        cmd.ExecuteNonQuery();
    }

    // UPDATE - Atualizar um produto existente
    public void Update(ProdutoInvestimento produto)
    {
        using var conn = _dbConnection.GetConnection();
        var sql = "UPDATE investimento_produto SET nome = :nome, tipo = :tipo, risco = :risco, preco = :preco WHERE id = :id";
        using var cmd = new OracleCommand(sql, conn);

        cmd.Parameters.Add(new OracleParameter("nome", produto.Nome));
        cmd.Parameters.Add(new OracleParameter("tipo", produto.Tipo));
        cmd.Parameters.Add(new OracleParameter("risco", produto.Risco));
        cmd.Parameters.Add(new OracleParameter("preco", produto.Preco));
        cmd.Parameters.Add(new OracleParameter("id", produto.Id));

        cmd.ExecuteNonQuery();
    }

    // DELETE - Deletar um produto
    public void Delete(int id)
    {
        using var conn = _dbConnection.GetConnection();
        var sql = "DELETE FROM investimento_produto WHERE id = :id";
        using var cmd = new OracleCommand(sql, conn);

        cmd.Parameters.Add(new OracleParameter("id", id));
        cmd.ExecuteNonQuery();
    }
}