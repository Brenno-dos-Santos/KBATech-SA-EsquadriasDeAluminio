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
    public class OrcamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrcamentoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrcamentoResponseDto>>> GetOrcamentos()
        {
            var orcamentos = await _context.Orcamentos.Select(o => new OrcamentoResponseDto
            {
                Id = o.Id,
                UsuarioId = o.UsuarioId,
                NomeCliente = o.NomeCliente,
                TelefoneCliente = o.TelefoneCliente,
                EmailCliente = o.EmailCliente,
                EnderecoObra = o.EnderecoObra,
                DataCriacao = o.DataCriacao,
                PrazoEstimado = o.PrazoEstimado,
                ValorTotal = o.ValorTotal,
                Status = o.Status
            }).ToListAsync();

            return Ok(orcamentos);
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CriarOrcamento([FromBody] CriarOrcamentoDto dto)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            var usuarioId = userIdClaim != null ? int.Parse(userIdClaim) : dto.UsuarioId;

            var novoOrcamento = new Orcamento
            {
                UsuarioId = usuarioId,
                NomeCliente = dto.NomeCliente,
                TelefoneCliente = dto.TelefoneCliente,
                EmailCliente = dto.EmailCliente,
                EnderecoObra = dto.EnderecoObra,
                DataCriacao = DateTime.Now,
                PrazoEstimado = dto.PrazoEstimado,
                ValorTotal = dto.ValorTotal,
                Status = string.IsNullOrEmpty(dto.Status) ? "RASCUNHO" : dto.Status.ToUpper()
            };

            await _context.Orcamentos.AddAsync(novoOrcamento);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Orçamento cadastrado com sucesso", id = novoOrcamento.Id });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarOrcamento(int id, [FromBody] AtualizarOrcamentoDto dto)
        {
            var orcamento = await _context.Orcamentos.FindAsync(id);

            if (orcamento == null)
                return NotFound(new { mensagem = "Orçamento não encontrado" });

            orcamento.NomeCliente = dto.NomeCliente;
            orcamento.TelefoneCliente = dto.TelefoneCliente;
            orcamento.EmailCliente = dto.EmailCliente;
            orcamento.EnderecoObra = dto.EnderecoObra;
            orcamento.PrazoEstimado = dto.PrazoEstimado;
            orcamento.ValorTotal = dto.ValorTotal;
            orcamento.Status = string.IsNullOrEmpty(dto.Status) ? orcamento.Status : dto.Status.ToUpper();

            if (orcamento.Status == "APROVADO")
            {
                var existeProjeto = await _context.Projetos.AnyAsync(p => p.OrcamentoId == id);
                if (!existeProjeto)
                {
                    await _context.Projetos.AddAsync(new Projeto
                    {
                        OrcamentoId = orcamento.Id,
                        DataInicioContrato = DateTime.Now,
                        PrazoFinalEntrega = orcamento.PrazoEstimado,
                        StatusProducao = "AGUARDANDO_MEDICAO"
                    });
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Orçamento atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "GESTOR")]
        public async Task<IActionResult> DeletarOrcamento(int id)
        {
            var orcamento = await _context.Orcamentos.FindAsync(id);

            if (orcamento == null)
                return NotFound(new { mensagem = "Orçamento não encontrado" });

            _context.Orcamentos.Remove(orcamento);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Orçamento removido com sucesso" });
        }

        // --- ITENS DO ORÇAMENTO ---

        [HttpGet("{orcamentoId}/itens")]
        public async Task<ActionResult<List<ItemOrcamentoResponseDto>>> GetItens(int orcamentoId)
        {
            var itens = await _context.ItensOrcamentos
                .Where(i => i.OrcamentoId == orcamentoId)
                .Select(i => new ItemOrcamentoResponseDto
                {
                    Id = i.Id,
                    OrcamentoId = i.OrcamentoId,
                    DescricaoItem = i.DescricaoItem,
                    Altura = i.Altura,
                    Largura = i.Largura,
                    TipoAbertura = i.TipoAbertura,
                    LadoAbertura = i.LadoAbertura,
                    PosicaoFechadura = i.PosicaoFechadura,
                    ValorUnitario = i.ValorUnitario,
                    Quantidade = i.Quantidade
                }).ToListAsync();

            return Ok(itens);
        }

        [HttpPost("itens/cadastrar")]
        public async Task<IActionResult> CriarItem([FromBody] CriarItemOrcamentoDto dto)
        {
            var novoItem = new ItemOrcamento
            {
                OrcamentoId = dto.OrcamentoId,
                DescricaoItem = dto.DescricaoItem,
                Altura = dto.Altura,
                Largura = dto.Largura,
                TipoAbertura = dto.TipoAbertura,
                LadoAbertura = dto.LadoAbertura,
                PosicaoFechadura = dto.PosicaoFechadura,
                ValorUnitario = dto.ValorUnitario,
                Quantidade = dto.Quantidade
            };

            await _context.ItensOrcamentos.AddAsync(novoItem);

            var orcamento = await _context.Orcamentos.FindAsync(dto.OrcamentoId);
            if (orcamento != null)
                orcamento.ValorTotal += (dto.ValorUnitario * dto.Quantidade);

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Item adicionado ao orçamento com sucesso" });
        }

        // --- PARCELAS DE PAGAMENTO ---

        [HttpGet("{orcamentoId}/parcelas")]
        public async Task<ActionResult<List<ParcelaPagamentoResponseDto>>> GetParcelas(int orcamentoId)
        {
            var parcelas = await _context.ParcelasPagamentos
                .Where(p => p.OrcamentoId == orcamentoId)
                .Select(p => new ParcelaPagamentoResponseDto
                {
                    Id = p.Id,
                    OrcamentoId = p.OrcamentoId,
                    FormaPagamento = p.FormaPagamento,
                    Valor = p.Valor,
                    DataVencimento = p.DataVencimento,
                    StatusPagamento = p.StatusPagamento
                }).ToListAsync();

            return Ok(parcelas);
        }

        [HttpPost("parcelas/cadastrar")]
        public async Task<IActionResult> CriarParcela([FromBody] CriarParcelaPagamentoDto dto)
        {
            var novaParcela = new ParcelaPagamento
            {
                OrcamentoId = dto.OrcamentoId,
                FormaPagamento = dto.FormaPagamento.ToUpper(),
                Valor = dto.Valor,
                DataVencimento = dto.DataVencimento,
                StatusPagamento = string.IsNullOrEmpty(dto.StatusPagamento) ? "PENDENTE" : dto.StatusPagamento.ToUpper()
            };

            await _context.ParcelasPagamentos.AddAsync(novaParcela);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Parcela cadastrada com sucesso" });
        }
    }
}