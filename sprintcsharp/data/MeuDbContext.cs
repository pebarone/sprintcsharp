// Local: sprintcsharp/Data/MeuDbContext.cs
using Microsoft.EntityFrameworkCore;
using sprintcsharp.Models;

namespace sprintcsharp.Data;

/// <summary>
/// Contexto do Entity Framework para acesso ao banco de dados Oracle
/// </summary>
public class MeuDbContext : DbContext
{
    public MeuDbContext(DbContextOptions<MeuDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet para a tabela de produtos de investimento
    /// </summary>
    public DbSet<ProdutoInvestimento> ProdutosInvestimento { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração explícita da entidade ProdutoInvestimento
        // Mapeia para a tabela INVESTIMENTO_PRODUTO existente no Oracle
        modelBuilder.Entity<ProdutoInvestimento>(entity =>
        {
            entity.ToTable("INVESTIMENTO_PRODUTO");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(255)
                .IsRequired();
            
            entity.Property(e => e.Tipo)
                .HasColumnName("TIPO")
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.Risco)
                .HasColumnName("RISCO")
                .HasMaxLength(50)
                .IsRequired();
            
            entity.Property(e => e.Preco)
                .HasColumnName("PRECO")
                .HasPrecision(19, 2)
                .IsRequired();
        });
    }
}