// Local: sprintcsharp/Models/ProdutoInvestimento.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sprintcsharp.Models;

/// <summary>
/// Representa um produto de investimento no sistema
/// </summary>
[Table("INVESTIMENTO_PRODUTO")]
public class ProdutoInvestimento
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [Column("NOME")]
    [StringLength(200)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [Column("CATEGORIA")]
    [StringLength(100)]
    public string Categoria { get; set; } = string.Empty;

    [Required]
    [Column("RENTABILIDADE_ANUAL")]
    public decimal RentabilidadeAnual { get; set; }

    [Required]
    [Column("NIVEL_RISCO")]
    [StringLength(50)]
    public string NivelRisco { get; set; } = string.Empty;

    [Column("DESCRICAO")]
    [StringLength(500)]
    public string? Descricao { get; set; }

    public override string ToString()
    {
        return $"[{Id}] {Nome} - {Categoria} | Rentabilidade: {RentabilidadeAnual}% | Risco: {NivelRisco}";
    }
}