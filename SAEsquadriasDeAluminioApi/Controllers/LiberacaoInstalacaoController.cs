using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadriasDeAluminioApi.Data;
using SAEsquadriasDeAluminioApi.Models;
using static SAEsquadriasDeAluminioApi.Models.DTOs;

namespace SAEsquadriasDeAluminioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LiberacaoInstalacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LiberacaoInstalacaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<LiberacaoInstalacaoResponseDto>>> GetLiberacoes()
        {
            var liberacoes = await _context.LiberacoesInstalacaos.Select(l => new LiberacaoInstalacaoResponseDto
            {
                Id = l.Id,
                ProjetoId = l.ProjetoId,
                UsuarioId = l.UsuarioId,
                DataLiberacao = l.DataLiberacao,
                EscopoLiberado = l.EscopoLiberado,
                Observacoes = l.Observacoes,
                Liberado = l.Liberado
            }).ToListAsync();

            return Ok(liberacoes);
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CriarLiberacao([FromBody] CriarLiberacaoInstalacaoDto dto)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            var usuarioId = userIdClaim != null ? int.Parse(userIdClaim) : dto.UsuarioId;

            var novaLiberacao = new LiberacaoInstalacao
            {
                ProjetoId = dto.ProjetoId,
                UsuarioId = usuarioId,
                DataLiberacao = DateTime.Now,
                EscopoLiberado = dto.EscopoLiberado,
                Observacoes = dto.Observacoes,
                Liberado = dto.Liberado
            };

            await _context.LiberacoesInstalacaos.AddAsync(novaLiberacao);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Liberação de instalação cadastrada com sucesso" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "GESTOR")]
        public async Task<IActionResult> DeletarLiberacao(int id)
        {
            var liberacao = await _context.LiberacoesInstalacaos.FindAsync(id);

            if (liberacao == null)
                return NotFound(new { mensagem = "Liberação não encontrada" });

            _context.LiberacoesInstalacaos.Remove(liberacao);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Liberação removida com sucesso" });
        }
    }
}