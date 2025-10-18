// Local: sprintcsharp/Data/MeuDbContext.cs
using Microsoft.EntityFrameworkCore;
using sprintcsharp.Models; // Importa seu modelo existente

namespace sprintcsharp.Data;

public class MeuDbContext : DbContext
{
    public MeuDbContext(DbContextOptions<MeuDbContext> options) : base(options)
    {
    }

    // Mapeia sua classe 'ProdutoInvestimento' para a tabela 'investimento_produto'
    public DbSet<ProdutoInvestimento> ProdutosInvestimento { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração explícita da tabela e colunas (opcional, mas recomendado)
        modelBuilder.Entity<ProdutoInvestimento>(entity =>
        {
            // O nome da tabela no banco Oracle (geralmente maiúsculo)
            entity.ToTable("INVESTIMENTO_PRODUTO");

            // Mapeia as propriedades para as colunas
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nome).HasColumnName("NOME");
            entity.Property(e => e.Tipo).HasColumnName("TIPO");
            entity.Property(e => e.Risco).HasColumnName("RISCO");
            entity.Property(e => e.Preco).HasColumnName("PRECO");
        });
    }
}