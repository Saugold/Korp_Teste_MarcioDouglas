using EmissaoNFAPI.Models;

namespace EmissaoNFAPI.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    { 
        Task<Produto?> GetByCodigoAsync(string codigo);
        Task DecrementQuantidadeAsync(int produtoId, int quantidade);
    }
}
