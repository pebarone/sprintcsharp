// Local: sprintcsharp/Data/ProdutoInvestimentoRepository.cs
using Microsoft.EntityFrameworkCore;
using sprintcsharp.Models;
using System.Collections.Generic;
using System.Linq; // <-- Importante para o LINQ

namespace sprintcsharp.Data;

public class ProdutoInvestimentoRepository
{
    // 1. Injeta o DbContext (Requisito EF)
    private readonly MeuDbContext _context;

    public ProdutoInvestimentoRepository(MeuDbContext context)
    {
        _context = context;
    }

    // READ (GET) - Usando LINQ
    public List<ProdutoInvestimento> GetAll()
    {
        // 2. LINQ: .ToList() busca todos os produtos
        return _context.ProdutosInvestimento.ToList();
    }

    // READ (GET BY ID) - Usando LINQ
    public ProdutoInvestimento? GetById(int id)
    {
        // 3. LINQ: .FirstOrDefault() busca o primeiro que bate com a condição
        return _context.ProdutosInvestimento.FirstOrDefault(p => p.Id == id);
    }

    // CREATE (POST) - Usando EF
    public void Add(ProdutoInvestimento produto)
    {
        _context.ProdutosInvestimento.Add(produto);
        _context.SaveChanges(); // O EF gera o SQL de INSERT
    }

    // UPDATE (PUT) - Usando EF
    public void Update(ProdutoInvestimento produto)
    {
        _context.Entry(produto).State = EntityState.Modified;
        _context.SaveChanges(); // O EF gera o SQL de UPDATE
    }

    // DELETE - Usando EF
    public void Delete(int id)
    {
        // 4. LINQ: .Find() busca pela chave primária
        var produto = _context.ProdutosInvestimento.Find(id);
        if (produto != null)
        {
            _context.ProdutosInvestimento.Remove(produto);
            _context.SaveChanges(); // O EF gera o SQL de DELETE
        }
    }    // ===== CONSULTAS LINQ AVANÇADAS (Requisito 2: 10%) =====

    /// <summary>
    /// Busca produtos por tipo usando LINQ Where
    /// </summary>
    public List<ProdutoInvestimento> BuscarPorTipo(string tipo)
    {
        return _context.ProdutosInvestimento
            .Where(p => p.Tipo.ToLower().Contains(tipo.ToLower()))
            .ToList();
    }

    /// <summary>
    /// Busca produtos com preço mínimo usando LINQ Where + OrderBy
    /// </summary>
    public List<ProdutoInvestimento> BuscarPorPrecoMinimo(decimal precoMinimo)
    {
        return _context.ProdutosInvestimento
            .Where(p => p.Preco >= precoMinimo)
            .OrderByDescending(p => p.Preco)
            .ToList();
    }

    /// <summary>
    /// Busca produtos com risco específico e ordena por nome usando LINQ Where + OrderBy
    /// </summary>
    public List<ProdutoInvestimento> BuscarPorRisco(string nivelRisco)
    {
        return _context.ProdutosInvestimento
            .Where(p => p.Risco.ToLower() == nivelRisco.ToLower())
            .OrderBy(p => p.Nome)
            .ToList();
    }

    /// <summary>
    /// Retorna estatísticas agregadas usando LINQ (Count, Average, Max, Min)
    /// </summary>
    public object ObterEstatisticas()
    {
        var produtos = _context.ProdutosInvestimento;
        
        return new
        {
            TotalProdutos = produtos.Count(),
            PrecoMedio = produtos.Any() ? produtos.Average(p => p.Preco) : 0,
            MaiorPreco = produtos.Any() ? produtos.Max(p => p.Preco) : 0,
            MenorPreco = produtos.Any() ? produtos.Min(p => p.Preco) : 0,
            ProdutosPorTipo = produtos
                .GroupBy(p => p.Tipo)
                .Select(g => new { Tipo = g.Key, Quantidade = g.Count() })
                .ToList(),
            ProdutosPorRisco = produtos
                .GroupBy(p => p.Risco)
                .Select(g => new { Risco = g.Key, Quantidade = g.Count() })
                .ToList()
        };
    }    /// <summary>
    /// Busca produtos com filtros múltiplos usando LINQ complexo
    /// </summary>
    public List<ProdutoInvestimento> BuscarComFiltros(
        string? tipo = null, 
        string? risco = null, 
        decimal? precoMinimo = null,
        string? ordenarPor = "nome")
    {
        var query = _context.ProdutosInvestimento.AsQueryable();

        // Filtros condicionais
        if (!string.IsNullOrEmpty(tipo))
            query = query.Where(p => p.Tipo.ToLower().Contains(tipo.ToLower()));

        if (!string.IsNullOrEmpty(risco))
            query = query.Where(p => p.Risco.ToLower() == risco.ToLower());

        if (precoMinimo.HasValue)
            query = query.Where(p => p.Preco >= precoMinimo.Value);

        // Ordenação dinâmica
        query = ordenarPor?.ToLower() switch
        {
            "preco" => query.OrderByDescending(p => p.Preco),
            "tipo" => query.OrderBy(p => p.Tipo),
            "risco" => query.OrderBy(p => p.Risco),
            _ => query.OrderBy(p => p.Nome)
        };

        return query.ToList();
    }

    /// <summary>
    /// Busca produtos e retorna apenas campos específicos usando LINQ Select (projeção)
    /// </summary>
    public List<object> ObterResumo()
    {
        return _context.ProdutosInvestimento
            .Select(p => new
            {
                p.Id,
                p.Nome,
                p.Tipo,
                p.Preco,
                RiscoSimplificado = p.Risco.Substring(0, 1).ToUpper()
            })
            .ToList<object>();
    }

    /// <summary>
    /// Paginação usando LINQ Skip e Take
    /// </summary>
    public object ObterProdutosPaginados(int pagina = 1, int itensPorPagina = 10)
    {
        var query = _context.ProdutosInvestimento.OrderBy(p => p.Id);
        var total = query.Count();
        var produtos = query
            .Skip((pagina - 1) * itensPorPagina)
            .Take(itensPorPagina)
            .ToList();

        return new
        {
            Pagina = pagina,
            ItensPorPagina = itensPorPagina,
            TotalItens = total,
            TotalPaginas = (int)Math.Ceiling(total / (double)itensPorPagina),
            Produtos = produtos
        };
    }
}