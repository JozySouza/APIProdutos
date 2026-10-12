namespace APIProdutos.Services;

using APIProdutos.Models;

public interface IProdutoService
{
    Task<List<Produto>> GetProdutosAsync();
    Task<Produto?> GetByIdAsync(int id);
    Task<Produto> CreateAsync(Produto produto);
    Task<Produto?> UpdateAsync(Produto produto);
    Task<bool> DeleteAsync(int id);
}