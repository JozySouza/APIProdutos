using APIProdutos.Models;
using APIProdutos.Data;
using Microsoft.EntityFrameworkCore;

namespace APIProdutos.Repositories;

public class ProdutoRepository : IProdutosRepository
{
    private readonly AppDBContext _context;

    public ProdutoRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task<List<Produto>> GetProdutosAsync()
    {
        return await _context.Produtos.ToListAsync();
    }

    public async Task<Produto?> GetByIdAsync(int id)
    {
        return await _context.Produtos.FindAsync(id);
    }

    public async Task<Produto> CreateAsync(Produto produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task<Produto?> UpdateAsync(Produto produto)
    {
        var existingProduto = await _context.Produtos.FindAsync(produto.Id);
        if (existingProduto == null)
        {
            return null;
        }

        existingProduto.Nome = produto.Nome;
        existingProduto.Preco = produto.Preco;
        existingProduto.Quantidade = produto.Quantidade;

        await _context.SaveChangesAsync();
        return existingProduto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
        {
            return false;
        }

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
        return true;
    }
}