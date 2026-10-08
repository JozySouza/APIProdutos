using Microsoft.AspNetCore.Mvc;
using APIProdutos.Models;

namespace APIProdutos.Controllers;

[ApiController]

[Route("api/[controller]")]

public class ProdutosController : ControllerBase
{
    private static readonly List<Produto> Produtos = new List<Produto>{ 
        new Produto { Id = 1, Nome = "Caderno", Preco = 10.99m, Quantidade = 5 },
        new Produto { Id = 2, Nome = "Caneta", Preco = 2.49m, Quantidade = 10 },
        new Produto { Id = 3, Nome = "Lápis", Preco = 1.99m, Quantidade = 15 },
        new Produto { Id = 4, Nome = "Borracha", Preco = 0.99m, Quantidade = 20 },
        new Produto { Id = 5, Nome = "Mochila", Preco = 49.99m, Quantidade = 3 },
        new Produto { Id = 6, Nome = "Estojo", Preco = 15.99m, Quantidade = 7 },
        new Produto { Id = 7, Nome = "Apontador", Preco = 3.49m, Quantidade = 12 },
        new Produto { Id = 8, Nome = "Régua", Preco = 5.99m, Quantidade = 8 },
        new Produto { Id = 9, Nome = "Tesoura", Preco = 7.99m, Quantidade = 6 },
        new Produto { Id = 10, Nome = "Cola", Preco = 4.99m, Quantidade = 9 }
    };

    [HttpGet]
    public IActionResult GetProdutos()
    {
        // Lógica para obter produtos
        return Ok(Produtos);
    }

    [HttpGet("{id}")]
    public IActionResult GetProdutoById(int id)
    {
        // Lógica para obter um produto específico pelo ID
        if (id <= 0)
        {
            return BadRequest(new { message = "ID inválido. Tente novamente." });
        }

            var produto = Produtos.FirstOrDefault(p => p.Id == id);
            if (produto == null)
            {
                return NotFound(new { message = "Produto não encontrado." });
            }
        
        return Ok($"Produto encontrado: {produto.Nome}, Preço: {produto.Preco}, Quantidade: {produto.Quantidade}");  
    }

    [HttpPost]
    public IActionResult CreateProduto([FromBody] Produto produto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // Lógica para criar um novo produto
        return CreatedAtAction(nameof(GetProdutoById), new { id = 1 }, produto);
        
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduto(int id, [FromBody] object produto)
    {
        // Lógica para atualizar um produto existente
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduto(int id)
    {
        // Lógica para deletar um produto existente
        return NoContent();
    }
}