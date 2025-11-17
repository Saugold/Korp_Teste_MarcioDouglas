using EmissaoNFAPI.Models;

namespace EmissaoNFAPI.Services.Produtos
{
    public interface IProdutoService
    {
        Task AddAsync(Produto produto);
        Task UpdateAsync(Produto produto);
        Task DeleteAsync(int produtoId);
        Task<Produto?> GetByIdAsync(int produtoId);
        Task<List<Produto>> GetAllProdutosAsync();

    }
}
