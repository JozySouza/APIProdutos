using Microsoft.AspNetCore.Mvc;

namespace APIProdutos.Controllers;

[ApiController]

[Route("api/[controller]")]

public class ProdutosController : ControllerBase
{
    private static readonly string[] Produtos = { 
        "Caderno", "Caneta", "Borracha", 
        "Lápis","Mochila","Estojo", 
        "Apontador", "Régua","Tesoura", "Cola"
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
        if (id <= 0)
        {
            return BadRequest(new { message = "ID inválido. Tente novamente." });
        }
        // Lógica para obter um produto específico pelo ID
        return Ok(Produtos[id - 1]);  
    }

    [HttpPost]
    public IActionResult CreateProduto([FromBody] string produto)
    {
        if (produto == null)
        {
            return BadRequest(new { message = "Produto inválido. Tente novamente." });
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