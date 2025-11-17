using EmissaoNFAPI.Models;

namespace EmissaoNFAPI.Repositories.Interfaces
{
    namespace EmissaoNFAPI.Repositories.Interfaces
    {
        public interface INotaFiscalRepository : IRepository<NotaFiscal>
        {
            Task<NotaFiscal?> GetByNumeroAsync(int numero);
            Task<int> GetMaxNumeroAsync();
            Task<IEnumerable<NotaFiscal>> GetAllWithItensAsync();
        }
    }
}
