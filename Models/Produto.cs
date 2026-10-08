using System.ComponentModel.DataAnnotations;
namespace APIProdutos.Models;


public class Produto
{
    public int Id { get; set; }

    [Required (ErrorMessage = "O nome do produto é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O nome do produto não pode exceder 100 caracteres.")]
    public required string Nome { get; set; }
    
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço do produto deve ser maior que zero.")]
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
}