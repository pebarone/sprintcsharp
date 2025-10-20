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
        modelBuilder.Entity<ProdutoInvestimento>(entity =>
        {
            entity.ToTable("INVESTIMENTO_PRODUTO");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(200)
                .IsRequired();
            
            entity.Property(e => e.Categoria)
                .HasColumnName("CATEGORIA")
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.RentabilidadeAnual)
                .HasColumnName("RENTABILIDADE_ANUAL")
                .HasPrecision(5, 2)
                .IsRequired();
            
            entity.Property(e => e.NivelRisco)
                .HasColumnName("NIVEL_RISCO")
                .HasMaxLength(50)
                .IsRequired();
            
            entity.Property(e => e.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(500);
        });
    }
}