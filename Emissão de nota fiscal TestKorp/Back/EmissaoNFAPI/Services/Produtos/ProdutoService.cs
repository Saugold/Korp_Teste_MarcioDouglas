using EmissaoNFAPI.Models;
using EmissaoNFAPI.Repositories.Interfaces;

namespace EmissaoNFAPI.Services.Produtos
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task AddAsync(Produto produto)
        {
            if (produto == null) throw new ArgumentNullException(nameof(produto));
            if (string.IsNullOrWhiteSpace(produto.Codigo)) throw new ArgumentException("Código é obrigatório");
            if (string.IsNullOrWhiteSpace(produto.Nome)) throw new ArgumentException("Nome é obrigatório");
            if (produto.Quantidade < 0) throw new ArgumentException("Saldo não pode ser negativo");
            var existente = await _produtoRepository.GetByCodigoAsync(produto.Codigo);
            if (existente != null) throw new InvalidOperationException("Já existe um produto cadastrado com o mesmo código");
            await _produtoRepository.AddAsync(produto);
        }

        public async Task DeleteAsync(int produtoId)
        {
            var produto = await _produtoRepository.GetByIdAsync(produtoId);
            if (produto == null) throw new KeyNotFoundException("Produto não encontrado");
            await _produtoRepository.DeleteAsync(produto);
        }

        public async Task<List<Produto>> GetAllProdutosAsync()
        {
            var list = await _produtoRepository.GetAllAsync();
            return new List<Produto>(list);
        }

        public async Task<Produto?> GetByIdAsync(int produtoId)
        {
            return await _produtoRepository.GetByIdAsync(produtoId);
        }

        public async Task UpdateAsync(Produto produto)
        {
            if (produto == null) throw new ArgumentNullException(nameof(produto));
            if (string.IsNullOrWhiteSpace(produto.Codigo)) throw new ArgumentException("Código é obrigatório");
            if (string.IsNullOrWhiteSpace(produto.Nome)) throw new ArgumentException("Nome é obrigatório");
            if (produto.Quantidade < 0) throw new ArgumentException("Saldo não pode ser negativo");
            var existente = await _produtoRepository.GetByIdAsync(produto.Id);
            if (existente == null) throw new KeyNotFoundException("Produto não encontrado");
            var porCodigo = await _produtoRepository.GetByCodigoAsync(produto.Codigo);
            if (porCodigo != null && porCodigo.Id != produto.Id)
                throw new InvalidOperationException("Outro proto já utiliza esse códig");
            await _produtoRepository.UpdateAsync(produto);
        }
    }
}
