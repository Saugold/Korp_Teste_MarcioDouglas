using EmissaoNFAPI.DTOs.NotaFiscal;
using EmissaoNFAPI.Models;
using EmissaoNFAPI.Repositories.Interfaces;
using EmissaoNFAPI.Repositories.Interfaces.EmissaoNFAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmissaoNFAPI.Services.NotasFiscais
{
    public class NotaFiscalService : INotaFiscalService
    {
        private readonly INotaFiscalRepository _notaRepository;
        private readonly IProdutoRepository _produtoRepository;

        public NotaFiscalService(INotaFiscalRepository notaRepository, IProdutoRepository produtoRepository)
        {
            _notaRepository = notaRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<NotaFiscal> CreateAsync(NotaFiscalCreateDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.Produtos == null || !dto.Produtos.Any())
                throw new ArgumentException("A nota fiscal deve conter ao menos um produto.");

            var itens = new List<ProdutoNotaFiscal>();
            foreach (var item in dto.Produtos)
            {
                if (item.Quantidade <= 0) throw new ArgumentException("A quantidade deve ser maior que zero.");
                var produto = await _produtoRepository.GetByIdAsync(item.ProdutoId);
                if (produto == null) throw new KeyNotFoundException($"Produto com id {item.ProdutoId} não encontrado.");

                if (produto.Quantidade < item.Quantidade)
                    throw new InvalidOperationException($"Produto {produto.Nome} (id {produto.Id}) não possui saldo suficiente.");
                itens.Add(new ProdutoNotaFiscal
                {
                    ProdutoId = produto.Id,
                    Quantidade = item.Quantidade
                });
            }
            var ultimo = await _notaRepository.GetMaxNumeroAsync();
            var proximoNumero = ultimo + 1;
            var nota = new NotaFiscal
            {
                Numero = proximoNumero,
                DataEmissao = DateTime.UtcNow,
                Status = string.IsNullOrWhiteSpace(dto.Status) ? "Aberta" : dto.Status
            };
            if (nota.Status != "Aberta" && nota.Status != "Fechada")
                throw new ArgumentException("Status inválido. Deve ser 'Aberta' ou 'Fechada'.");

            foreach (var pn in itens)
            {
                nota.Produtos.Add(pn);
            }

            await _notaRepository.AddAsync(nota);
            return nota;
        }

        public async Task<List<NotaFiscal>> GetAllAsync()
        {
            var list = await _notaRepository.GetAllWithItensAsync();
            return list.ToList();
        }

        public async Task<NotaFiscal?> GetByNumeroAsync(int numero)
        {
            return await _notaRepository.GetByNumeroAsync(numero);
        }

        public async Task CloseAsync(int numero)
        {
            var nota = await _notaRepository.GetByNumeroAsync(numero);
            if (nota == null) throw new KeyNotFoundException("Nota fiscal não encontrada.");
            if (nota.Status == "Fechada") throw new InvalidOperationException("Nota já está fechada.");

            // valida saldo primeiro
            foreach (var item in nota.Produtos)
            {
                var produto = await _produtoRepository.GetByIdAsync(item.ProdutoId);
                if (produto == null)
                    throw new KeyNotFoundException($"Produto {item.ProdutoId} não encontrado.");
                if (produto.Quantidade < item.Quantidade)
                    throw new InvalidOperationException($"Produto '{produto.Nome}' não possui saldo suficiente. Saldo atual: {produto.Quantidade}, necessário: {item.Quantidade}.");
            }
            foreach (var item in nota.Produtos)
            {
                await _produtoRepository.DecrementQuantidadeAsync(item.ProdutoId, item.Quantidade);
            }
            foreach (var pn in nota.Produtos)
            {
                pn.Produto = null;
            }
            nota.Status = "Fechada";
            await _notaRepository.UpdateAsync(nota);
        }
    }
}