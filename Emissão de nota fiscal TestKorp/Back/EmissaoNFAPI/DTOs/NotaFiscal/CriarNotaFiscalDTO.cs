using System.ComponentModel.DataAnnotations;

namespace EmissaoNFAPI.DTOs.NotaFiscal
{
    public class NotaFiscalCreateDTO
    {
        [Required]
        [MaxLength(10)]
        public string Status { get; set; } = "Aberta";
        public List<ProdutoNotaFiscalDTO> Produtos { get; set; } = new List<ProdutoNotaFiscalDTO>();
    }

    public class ProdutoNotaFiscalDTO
    {
        [Required]
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
