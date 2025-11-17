using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmissaoNFAPI.Models
{
    public class NotaFiscal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DataEmissao { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        [MaxLength(10)]
        public string Status { get; set; } = "Aberta";

        public ICollection<ProdutoNotaFiscal> Produtos { get; set; } = new List<ProdutoNotaFiscal>();
    }
}
