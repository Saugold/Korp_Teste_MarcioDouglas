using EmissaoNFAPI.Data;
using EmissaoNFAPI.Models;
using EmissaoNFAPI.Repositories.Interfaces;
using EmissaoNFAPI.Repositories.Interfaces.EmissaoNFAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmissaoNFAPI.Repositories
{
    public class NotaFiscalRepository : Repository<NotaFiscal>, INotaFiscalRepository
    {
        public NotaFiscalRepository(AppDbContext context) : base(context) { }

        public async Task<NotaFiscal?> GetByNumeroAsync(int numero)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(nf => nf.Produtos)
                    .ThenInclude(pn => pn.Produto)
                .FirstOrDefaultAsync(nf => nf.Numero == numero);
        }

        public async Task<int> GetMaxNumeroAsync()
        {
            var max = await _dbSet.MaxAsync<NotaFiscal, int?>(nf => (int?)nf.Numero);
            return max ?? 0;
        }

        public async Task<IEnumerable<NotaFiscal>> GetAllWithItensAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Include(nf => nf.Produtos)
                    .ThenInclude(pn => pn.Produto)
                .OrderBy(nf => nf.Numero)
                .ToListAsync();
        }
    }
}
