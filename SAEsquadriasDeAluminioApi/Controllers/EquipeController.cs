using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAEsquadriasDeAluminioApi.Data;
using SAEsquadriasDeAluminioApi.Models;

namespace SAEsquadriasDeAluminioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EquipeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/equipe
        [HttpGet]
        public async Task<IActionResult> ListarEquipes()
        {
            var equipes = await _context.Equipes
                .Include(e => e.Membros)
                    .ThenInclude(m => m.User)
                .Where(e => e.Ativa)
                .Select(e => new
                {
                    e.Id,
                    e.Nome,
                    e.Especialidade,
                    e.Ativa,
                    Membros = e.Membros.Select(m => new
                    {
                        m.UsuarioId,
                        Nome = m.User != null ? m.User.Nome : "Técnico",
                        m.FuncaoNaEquipe
                    })
                })
                .ToListAsync();

            return Ok(equipes);
        }

        // POST: api/equipe/cadastrar
        [HttpPost("cadastrar")]
        public async Task<IActionResult> CriarEquipe([FromBody] CriarEquipeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                return BadRequest("O nome da equipe é obrigatório.");

            if (dto.TecnicosIds == null || !dto.TecnicosIds.Any())
                return BadRequest("Selecione pelo menos um técnico para formar a equipe.");

            var novaEquipe = new Equipe
            {
                Nome = dto.Nome.Trim(),
                Especialidade = dto.Especialidade?.Trim() ?? "Instalação Geral",
                Ativa = true
            };

            foreach (var techId in dto.TecnicosIds.Distinct())
            {
                novaEquipe.Membros.Add(new EquipeMembro
                {
                    UsuarioId = techId,
                    FuncaoNaEquipe = techId == dto.LiderId ? "Líder" : "Instalador"
                });
            }

            _context.Equipes.Add(novaEquipe);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Equipe criada com sucesso!", id = novaEquipe.Id });
        }

        // DELETE: api/equipe/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DesativarEquipe(int id)
        {
            var equipe = await _context.Equipes.FindAsync(id);
            if (equipe == null) return NotFound();

            equipe.Ativa = false;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Equipe removida." });
        }
    }

    public class CriarEquipeDto
    {
        public string Nome { get; set; } = string.Empty;
        public string? Especialidade { get; set; }
        public int? LiderId { get; set; }
        public List<int> TecnicosIds { get; set; } = new();
    }
}