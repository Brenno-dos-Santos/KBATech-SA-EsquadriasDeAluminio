using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadriasDeAluminioApi.Data;
using SAEsquadriasDeAluminioApi.Models;

namespace SAEsquadriasDeAluminioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OcorrenciaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OcorrenciaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ocorrencia?resolvida=false&projetoId=1
        [HttpGet]
        public async Task<IActionResult> ListarOcorrencias(
            [FromQuery] bool? resolvida = null,
            [FromQuery] int? projetoId = null)
        {
            var query = _context.Ocorrencias
                .Include(o => o.Projeto)
                .Include(o => o.ReportadoPor)
                .AsQueryable();

            if (resolvida.HasValue)
                query = query.Where(o => o.Resolvida == resolvida.Value);

            if (projetoId.HasValue)
                query = query.Where(o => o.ProjetoId == projetoId.Value);

            var lista = await query
                .OrderByDescending(o => o.DataAbertura)
                .Select(o => new
                {
                    o.Id,
                    o.ProjetoId,
                    OrcamentoId = o.Projeto != null ? o.Projeto.OrcamentoId : 0,
                    o.ReportadoPorId,
                    ReportadoPorNome = o.ReportadoPor != null ? o.ReportadoPor.Nome : "Usuário",
                    o.Titulo,
                    o.Tipo,
                    o.Severidade,
                    o.Descricao,
                    o.FotoUrl,
                    o.BloqueiaObra,
                    o.Resolvida,
                    o.SolucaoAplicada,
                    o.DataAbertura,
                    o.DataResolucao
                })
                .ToListAsync();

            return Ok(lista);
        }

        // GET: api/ocorrencia/resumo (Para cards de KPI e alertas da Topbar)
        [HttpGet("resumo")]
        public async Task<IActionResult> ObterResumo()
        {
            var abertas = await _context.Ocorrencias.CountAsync(o => !o.Resolvida);
            var embargosAtivos = await _context.Ocorrencias.CountAsync(o => !o.Resolvida && o.BloqueiaObra);
            var criticas = await _context.Ocorrencias.CountAsync(o => !o.Resolvida && o.Severidade == "CRITICA");
            var resolvidas = await _context.Ocorrencias.CountAsync(o => o.Resolvida);

            return Ok(new
            {
                abertas,
                embargosAtivos,
                criticas,
                resolvidas
            });
        }

        // POST: api/ocorrencia/cadastrar
        [HttpPost("cadastrar")]
        public async Task<IActionResult> CriarOcorrencia([FromBody] CriarOcorrenciaDto dto)
        {
            if (dto.ProjetoId <= 0)
                return BadRequest(new { mensagem = "Selecione um projeto válido." });

            if (string.IsNullOrWhiteSpace(dto.Titulo) || string.IsNullOrWhiteSpace(dto.Descricao))
                return BadRequest(new { mensagem = "Título e descrição são obrigatórios." });

            var projetoExiste = await _context.Projetos.AnyAsync(p => p.Id == dto.ProjetoId);
            if (!projetoExiste)
                return NotFound(new { mensagem = "Projeto não encontrado." });

            var nova = new Ocorrencia
            {
                ProjetoId = dto.ProjetoId,
                ReportadoPorId = dto.ReportadoPorId > 0 ? dto.ReportadoPorId : 1,
                Titulo = dto.Titulo.Trim(),
                Tipo = string.IsNullOrWhiteSpace(dto.Tipo) ? "TECNICA" : dto.Tipo.ToUpper(),
                Severidade = string.IsNullOrWhiteSpace(dto.Severidade) ? "MEDIA" : dto.Severidade.ToUpper(),
                Descricao = dto.Descricao.Trim(),
                FotoUrl = dto.FotoUrl,
                BloqueiaObra = dto.BloqueiaObra,
                Resolvida = false,
                DataAbertura = DateTime.UtcNow
            };

            _context.Ocorrencias.Add(nova);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Ocorrência registrada com sucesso!", id = nova.Id });
        }

        // PATCH: api/ocorrencia/{id}/resolver
        [HttpPatch("{id}/resolver")]
        public async Task<IActionResult> ResolverOcorrencia(int id, [FromBody] ResolverOcorrenciaDto dto)
        {
            var ocorrencia = await _context.Ocorrencias.FindAsync(id);
            if (ocorrencia == null)
                return NotFound(new { mensagem = "Ocorrência não encontrada." });

            ocorrencia.Resolvida = true;
            ocorrencia.BloqueiaObra = false;
            ocorrencia.SolucaoAplicada = string.IsNullOrWhiteSpace(dto.SolucaoAplicada)
                ? "Resolvido pela equipe técnica."
                : dto.SolucaoAplicada.Trim();
            ocorrencia.DataResolucao = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Ocorrência marcada como resolvida!" });
        }

        // DELETE: api/ocorrencia/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "GESTOR")]
        public async Task<IActionResult> DeletarOcorrencia(int id)
        {
            var ocorrencia = await _context.Ocorrencias.FindAsync(id);
            if (ocorrencia == null)
                return NotFound(new { mensagem = "Ocorrência não encontrada." });

            _context.Ocorrencias.Remove(ocorrencia);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Ocorrência removida." });
        }
    }

    public class CriarOcorrenciaDto
    {
        public int ProjetoId { get; set; }
        public int ReportadoPorId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Tipo { get; set; }
        public string? Severidade { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? FotoUrl { get; set; }
        public bool BloqueiaObra { get; set; }
    }

    public class ResolverOcorrenciaDto
    {
        public string? SolucaoAplicada { get; set; }
    }
}