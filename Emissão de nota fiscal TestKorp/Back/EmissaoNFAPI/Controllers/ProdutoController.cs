using EmissaoNFAPI.DTOs.Produto;
using EmissaoNFAPI.Models;
using EmissaoNFAPI.Services.Produtos;
using Microsoft.AspNetCore.Mvc;

namespace EmissaoNFAPI.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var produtos = await _produtoService.GetAllProdutosAsync();
            return Ok(produtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null) return NotFound(new { message = "Produto não encontrado." });
            return Ok(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProdutoCreateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var produto = new Produto { Codigo = dto.Codigo, Nome = dto.Nome, Quantidade = dto.Quantidade };
                await _produtoService.AddAsync(produto);
                return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Produto produto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != produto.Id) return BadRequest(new { message = "Id do recurso diferente do body." });

            try
            {
                await _produtoService.UpdateAsync(produto);
                return NoContent();
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _produtoService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno", detail = ex.Message });
            }
        }
    }
}

