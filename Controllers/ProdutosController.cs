using Microsoft.AspNetCore.Mvc;
using APIProdutos.Models;
using APIProdutos.Data;
using Microsoft.EntityFrameworkCore;

namespace APIProdutos.Controllers;

[ApiController]

[Route("api/[controller]")]

public class ProdutosController : ControllerBase
{
 private readonly AppDBContext _context;

    public ProdutosController(AppDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProdutos()
    {
        // Lógica para obter produtos
        var Produtos = await _context.Produtos.ToListAsync();
        return Ok(Produtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProdutoById(int id)
    {
        // Lógica para obter um produto específico pelo ID
        if (id <= 0)
        {
            return BadRequest(new { message = "ID inválido. Tente novamente." });
        }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound(new { message = "Produto não encontrado." });
            }
        
        return Ok($"Produto encontrado: {produto.Nome}, Preço: {produto.Preco}, Quantidade: {produto.Quantidade}");  
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduto([FromBody] Produto produto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // Lógica para criar um novo produto
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProdutoById), new { id = 1 }, produto);
        
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduto(int id, [FromBody] object produto)
    {
        // Lógica para atualizar um produto existente
        if(ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var produtoExistente = await _context.Produtos.FindAsync(id);
        if (produtoExistente == null)
        {
            return NotFound(new { message = "Produto não encontrado." });
        }
        await _context.SaveChangesAsync();
        return Ok($"Produto atualizado: {produtoExistente.Nome}, Preço: {produtoExistente.Preco}, Quantidade: {produtoExistente.Quantidade}");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        // Lógica para deletar um produto existente
        var produtoExistente = await _context.Produtos.FindAsync(id);
        if (produtoExistente == null)
        {
            return NotFound(new { message = "Produto não encontrado." });
        }
        _context.Produtos.Remove(produtoExistente);
        await _context.SaveChangesAsync();
        return Ok($"Produto deletado: {produtoExistente.Nome}");
    }
}