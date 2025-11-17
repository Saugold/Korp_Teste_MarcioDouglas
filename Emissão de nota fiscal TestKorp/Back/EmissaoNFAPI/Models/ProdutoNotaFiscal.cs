using System.ComponentModel.DataAnnotations;

namespace EmissaoNFAPI.Models
{
    public class ProdutoNotaFiscal
    {
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;

        public int NotaFiscalId { get; set; }
        public NotaFiscal NotaFiscal { get; set; } = null!;
        public int Quantidade { get; set; }
    }
}
