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
    }
}