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
    public class RegistroObraController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RegistroObraController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<RegistroObraResponseDto>>> GetRegistrosObra()
        {
            var registros = await _context.RegistrosObras.Select(r => new RegistroObraResponseDto
            {
                Id = r.Id,
                ProjetoId = r.ProjetoId,
                UsuarioId = r.UsuarioId,
                DataRegistro = r.DataRegistro,
                EtapaProcesso = r.EtapaProcesso,
                TipoOcorrencia = r.TipoOcorrencia,
                Descricao = r.Descricao,
                FotoUrl = r.FotoUrl,
                Resolvido = r.Resolvido
            }).ToListAsync();

            return Ok(registros);
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CriarRegistroObra([FromBody] CriarRegistroObraDto dto)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            var userId = userIdClaim != null ? int.Parse(userIdClaim) : dto.UsuarioId;

            var tipoOcorrencia = string.IsNullOrEmpty(dto.TipoOcorrencia) ? "NENHUMA" : dto.TipoOcorrencia.ToUpper();

            var novoRegistro = new RegistroObra
            {
                ProjetoId = dto.ProjetoId,
                UsuarioId = userId,
                DataRegistro = DateTime.Now,
                EtapaProcesso = dto.EtapaProcesso.ToUpper(),
                TipoOcorrencia = tipoOcorrencia,
                Descricao = dto.Descricao,
                FotoUrl = dto.FotoUrl,
                Resolvido = tipoOcorrencia == "NENHUMA" ? true : dto.Resolvido
            };

            await _context.RegistrosObras.AddAsync(novoRegistro);

            if (novoRegistro.EtapaProcesso == "CONCLUIDO")
            {
                var projeto = await _context.Projetos.FindAsync(dto.ProjetoId);
                if (projeto != null)
                    projeto.StatusProducao = "CONCLUIDO";
            }

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Registro de obra cadastrado com sucesso" });
        }

        [HttpPut("{id}/resolver")]
        public async Task<IActionResult> ResolverOcorrencia(int id)
        {
            var registro = await _context.RegistrosObras.FindAsync(id);

            if (registro == null)
                return NotFound(new { mensagem = "Registro de obra não encontrado" });

            registro.Resolvido = true;
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Ocorrência marcada como resolvida" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "GESTOR")]
        public async Task<IActionResult> DeletarRegistroObra(int id)
        {
            var registro = await _context.RegistrosObras.FindAsync(id);

            if (registro == null)
                return NotFound(new { mensagem = "Registro de obra não encontrado" });

            _context.RegistrosObras.Remove(registro);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Registro de obra removido com sucesso" });
        }
    }
}