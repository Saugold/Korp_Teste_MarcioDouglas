namespace EmissaoNFAPI.DTOs.Produto
{
    public class ProdutoDTO
    {
        public int Id { get; set; }

        public string Codigo { get; set; } = string.Empty;


        public string Nome { get; set; } = string.Empty;


        public int Quantidade { get; set; }
    }
}
