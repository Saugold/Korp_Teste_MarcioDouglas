namespace EmissaoNFAPI.DTOs.NotaFiscal
{
    public class NotaFiscalDTO
    {
        public int Numero { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; } = DateTime.Now;

        public List<ProdutoNotaFiscalDTO> Produtos { get; set; } = new List<ProdutoNotaFiscalDTO>();
    }
}
