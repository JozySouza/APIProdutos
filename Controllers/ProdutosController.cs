using Microsoft.AspNetCore.Mvc;
using APIProdutos.Models;
using APIProdutos.Services;

namespace APIProdutos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _service;

    public ProdutosController(IProdutoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetProdutos()
    {
        var produtos = await _service.GetProdutosAsync();
        return Ok(produtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProdutoById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "ID inválido. Tente novamente." });
        }

        var produto = await _service.GetByIdAsync(id);
        if (produto == null)
        {
            return NotFound(new { message = "Produto não encontrado." });
        }

        return Ok(produto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduto([FromBody] Produto produto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var produtoCriado = await _service.CreateAsync(produto);
            return CreatedAtAction(nameof(GetProdutoById), new { id = produtoCriado.Id }, produtoCriado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
       
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduto(int id, [FromBody] Produto produto)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "ID inválido. Tente novamente." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        produto.Id = id;
        var produtoAtualizado = await _service.UpdateAsync(produto);
        if (produtoAtualizado == null)
        {
            return NotFound(new { message = "Produto não encontrado." });
        }

        return Ok(produtoAtualizado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "ID inválido. Tente novamente." });
        }

        var removido = await _service.DeleteAsync(id);
        if (!removido)
        {
            return NotFound(new { message = "Produto não encontrado." });
        }

        return NoContent();
    }
}