using EmissaoNFAPI.DTOs.NotaFiscal;
using EmissaoNFAPI.Services.NotasFiscais;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmissaoNFAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotaFiscalController : ControllerBase
    {
        private readonly INotaFiscalService _notaService;

        public NotaFiscalController(INotaFiscalService notaService)
        {
            _notaService = notaService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var notas = await _notaService.GetAllAsync();

            var result = notas.Select(n => new NotaFiscalDTO
            {
                Numero = n.Numero,
                Status = n.Status,
                CriadoEm = n.DataEmissao,
                Produtos = n.Produtos.Select(pn => new ProdutoNotaFiscalDTO
                {
                    ProdutoId = pn.ProdutoId,
                    Quantidade = pn.Quantidade
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{numero:int}")]
        public async Task<IActionResult> GetByNumero(int numero)
        {
            var nota = await _notaService.GetByNumeroAsync(numero);
            if (nota == null) return NotFound(new { message = "Nota fiscal não encontrada." });

            var dto = new NotaFiscalDTO
            {
                Numero = nota.Numero,
                Status = nota.Status,
                CriadoEm = nota.DataEmissao,
                Produtos = nota.Produtos.Select(pn => new ProdutoNotaFiscalDTO
                {
                    ProdutoId = pn.ProdutoId,
                    Quantidade = pn.Quantidade
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotaFiscalCreateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var nota = await _notaService.CreateAsync(dto);

                var result = new NotaFiscalDTO
                {
                    Numero = nota.Numero,
                    Status = nota.Status,
                    CriadoEm = nota.DataEmissao,
                    Produtos = nota.Produtos.Select(pn => new ProdutoNotaFiscalDTO
                    {
                        ProdutoId = pn.ProdutoId,
                        Quantidade = pn.Quantidade
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetByNumero), new { numero = result.Numero }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno", detail = ex.Message });
            }
        }

        [HttpPost("{numero:int}/fechar")]
        public async Task<IActionResult> Close(int numero)
        {
            try
            {
                await _notaService.CloseAsync(numero);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno", detail = ex.Message });
            }
        }
    }
}

