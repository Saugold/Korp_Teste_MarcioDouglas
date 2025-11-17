using EmissaoNFAPI.Data;
using EmissaoNFAPI.Models;
using EmissaoNFAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmissaoNFAPI.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public async Task<Produto?> GetByCodigoAsync(string codigo)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Codigo == codigo);
        }
        public async Task DecrementQuantidadeAsync(int produtoId, int quantidade)
        {
            var result = await _context.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE Produtos SET Quantidade = Quantidade - {quantidade} WHERE Id = {produtoId} AND Quantidade >= {quantidade}");

            if (result == 0)
            {
                throw new System.InvalidOperationException($"Não foi possível decrementar o produto {produtoId}. Saldo insuficiente ou produto não encontrado.");
            }
        }
    }
}
