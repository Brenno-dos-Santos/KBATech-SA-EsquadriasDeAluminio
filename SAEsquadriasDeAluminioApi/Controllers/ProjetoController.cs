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
    public class ProjetoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjetoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjetoResponseDto>>> GetProjetos()
        {
            var projetos = await _context.Projetos.Select(p => new ProjetoResponseDto
            {
                Id = p.Id,
                OrcamentoId = p.OrcamentoId,
                TecnicoId = p.TecnicoId,
                DataMedicaoFinal = p.DataMedicaoFinal,
                DataInicioContrato = p.DataInicioContrato,
                PrazoFinalEntrega = p.PrazoFinalEntrega,
                StatusProducao = p.StatusProducao
            }).ToListAsync();

            return Ok(projetos);
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CriarProjeto([FromBody] CriarProjetoDto dto)
        {
            var novoProjeto = new Projeto
            {
                OrcamentoId = dto.OrcamentoId,
                TecnicoId = dto.TecnicoId,
                DataMedicaoFinal = dto.DataMedicaoFinal,
                DataInicioContrato = dto.DataInicioContrato ?? DateTime.Now,
                PrazoFinalEntrega = dto.PrazoFinalEntrega,
                StatusProducao = string.IsNullOrEmpty(dto.StatusProducao) ? "AGUARDANDO_MEDICAO" : dto.StatusProducao.ToUpper()
            };

            await _context.Projetos.AddAsync(novoProjeto);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Projeto cadastrado com sucesso" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProjeto(int id, [FromBody] AtualizarStatusProjetoDto dto)
        {
            var projeto = await _context.Projetos.FindAsync(id);

            if (projeto == null)
                return NotFound(new { mensagem = "Projeto não encontrado" });

            var statusAntigo = projeto.StatusProducao;
            projeto.TecnicoId = dto.TecnicoId ?? projeto.TecnicoId;
            projeto.StatusProducao = string.IsNullOrEmpty(dto.StatusProducao) ? projeto.StatusProducao : dto.StatusProducao.ToUpper();
            projeto.DataMedicaoFinal = dto.DataMedicaoFinal ?? projeto.DataMedicaoFinal;
            projeto.PrazoFinalEntrega = dto.PrazoFinalEntrega ?? projeto.PrazoFinalEntrega;

            var userIdClaim = User.FindFirst("id")?.Value;
            var userId = userIdClaim != null ? int.Parse(userIdClaim) : 1;

            await _context.HistoricosAcompanhamentos.AddAsync(new HistoricoAcompanhamento
            {
                ProjetoId = projeto.Id,
                UsuarioId = userId,
                DataHora = DateTime.Now,
                Acao = "ATUALIZACAO_PROJETO",
                Descricao = $"Status alterado de {statusAntigo} para {projeto.StatusProducao}"
            });

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Projeto atualizado com sucesso" });
        }

        [HttpPut("{id}/designar-tecnico/{tecnicoId}")]
        [Authorize(Roles = "GESTOR")]
        public async Task<IActionResult> DesignarTecnico(int id, int tecnicoId)
        {
            var projeto = await _context.Projetos.FindAsync(id);
            if (projeto == null)
                return NotFound(new { mensagem = "Projeto não encontrado" });

            var tecnico = await _context.Users.FindAsync(tecnicoId);
            if (tecnico == null || !tecnico.Ativo)
                return BadRequest(new { mensagem = "Técnico inválido ou inativo" });

            projeto.TecnicoId = tecnicoId;

            var userIdClaim = User.FindFirst("id")?.Value;
            var userId = userIdClaim != null ? int.Parse(userIdClaim) : 1;

            await _context.HistoricosAcompanhamentos.AddAsync(new HistoricoAcompanhamento
            {
                ProjetoId = projeto.Id,
                UsuarioId = userId,
                DataHora = DateTime.Now,
                Acao = "DESIGNACAO_EQUIPE",
                Descricao = $"Técnico {tecnico.Nome} designado para o projeto"
            });

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Técnico designado com sucesso" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "GESTOR")]
        public async Task<IActionResult> DeletarProjeto(int id)
        {
            var projeto = await _context.Projetos.FindAsync(id);

            if (projeto == null)
                return NotFound(new { mensagem = "Projeto não encontrado" });

            _context.Projetos.Remove(projeto);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Projeto removido com sucesso" });
        }

       
    }
}