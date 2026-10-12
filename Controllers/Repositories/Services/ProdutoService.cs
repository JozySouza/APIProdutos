using APIProdutos.Models;
using APIProdutos.Repositories;


namespace APIProdutos.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutosRepository _produtoRepository;

    public ProdutoService(IProdutosRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<List<Produto>> GetProdutosAsync()
    {
        return await _produtoRepository.GetProdutosAsync();
    }

    public async Task<Produto?> GetByIdAsync(int id)
    {
        return await _produtoRepository.GetByIdAsync(id);
    }

    public async Task<Produto> CreateAsync(Produto produto)
    {
        if (produto.Preco < 0.01m)
        {
            throw new ArgumentNullException("Preço inválido. O preço deve ser maior que zero.");
        }
        if(await _produtoRepository.ExisteNomeDuplicadoAsync(produto.Nome))
        {
            throw new ArgumentException("Nome do produto já existe. Por favor, escolha outro nome.");
        }
        return await _produtoRepository.CreateAsync(produto);
    }

    public async Task<Produto?> UpdateAsync(Produto produto)
    {
        return await _produtoRepository.UpdateAsync(produto);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _produtoRepository.DeleteAsync(id);
    }
}