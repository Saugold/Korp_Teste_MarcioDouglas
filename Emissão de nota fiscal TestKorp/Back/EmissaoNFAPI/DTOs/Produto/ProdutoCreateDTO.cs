using System.ComponentModel.DataAnnotations;

namespace EmissaoNFAPI.DTOs.Produto
{
    public class ProdutoCreateDTO
    {
        [Required] public string Codigo { get; set; } = string.Empty;
        [Required] public string Nome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
    }
}
