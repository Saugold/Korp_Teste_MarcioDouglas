using EmissaoNFAPI.DTOs.NotaFiscal;
using EmissaoNFAPI.Models;

namespace EmissaoNFAPI.Services.NotasFiscais
{
    public interface INotaFiscalService
    {
        Task<NotaFiscal> CreateAsync(NotaFiscalCreateDTO dto);
        Task<NotaFiscal?> GetByNumeroAsync(int numero);
        Task<List<NotaFiscal>> GetAllAsync();
        Task CloseAsync(int numero);
    }
}
