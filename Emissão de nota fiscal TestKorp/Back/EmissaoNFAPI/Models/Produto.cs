using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmissaoNFAPI.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        [MaxLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty; 
        public int Quantidade { get; set; }
        public ICollection<ProdutoNotaFiscal> ProdutoNotaFiscais { get; set; } = new List<ProdutoNotaFiscal>();
    }
}
