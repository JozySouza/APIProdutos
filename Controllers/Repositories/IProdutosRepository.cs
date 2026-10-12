namespace APIProdutos.Repositories;

using APIProdutos.Models;

public interface IProdutosRepository
{
    Task<List<Produto>> GetProdutosAsync();
    Task<Produto?> GetByIdAsync(int id);
    Task<Produto> CreateAsync(Produto produto);
    Task <Produto?> UpdateAsync(Produto produto);
    Task <bool> DeleteAsync(int id);

    Task <bool> ExisteNomeDuplicadoAsync(string nome);
    
}