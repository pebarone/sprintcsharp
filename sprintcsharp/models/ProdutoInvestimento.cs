// Local: sprintcsharp/Models/ProdutoInvestimento.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sprintcsharp.Models;

/// <summary>
/// Representa um produto de investimento no sistema
/// Mapeado para a tabela INVESTIMENTO_PRODUTO existente no Oracle
/// </summary>
[Table("INVESTIMENTO_PRODUTO")]
public class ProdutoInvestimento
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [Column("NOME")]
    [StringLength(255)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [Column("TIPO")]
    [StringLength(100)]
    public string Tipo { get; set; } = string.Empty;

    [Required]
    [Column("RISCO")]
    [StringLength(50)]
    public string Risco { get; set; } = string.Empty;    [Required]
    [Column("PRECO", TypeName = "NUMBER(19,2)")]
    public decimal Preco { get; set; }

    public override string ToString()
    {
        return $"[{Id}] {Nome} - {Tipo} | Preço: R$ {Preco:N2} | Risco: {Risco}";
    }
}