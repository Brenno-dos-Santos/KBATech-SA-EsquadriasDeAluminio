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
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioResponseDto>>> GetUsers()
        {
            var users = await _context.Users.Select(u => new UsuarioResponseDto
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email ?? string.Empty,
                Perfil = u.Roles,
                Ativo = u.Ativo
            }).ToListAsync();

            return Ok(users);
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CriarUser([FromBody] User dto)
        {
            var novoUser = new User
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Numero = dto.Numero,
                Password = dto.Password,
                Roles = string.IsNullOrEmpty(dto.Roles) ? "TECNICO_CAMPO" : dto.Roles.ToUpper(),
                Ativo = true
            };

            await _context.Users.AddAsync(novoUser);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Utilizador cadastrado com sucesso" });
        }

        // ==========================================
        // PERFIL DO PRÓPRIO USUÁRIO LOGADO
        // (o id vem do token JWT, então ninguém edita o perfil de outra pessoa)
        // ==========================================

        private int? ObterIdLogado()
        {
            var claim = User.FindFirst("id")?.Value;
            if (int.TryParse(claim, out var id))
                return id;
            return null;
        }

        [HttpPut("meu-perfil")]
        [Authorize]
        public async Task<IActionResult> AtualizarMeuPerfil([FromBody] AtualizarMeuPerfilDto dto)
        {
            var userId = ObterIdLogado();
            if (userId == null)
                return Unauthorized(new { mensagem = "Sessão inválida. Faça login novamente." });

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null)
                return NotFound(new { mensagem = "Utilizador não encontrado" });

            if (string.IsNullOrWhiteSpace(dto.Nome))
                return BadRequest(new { mensagem = "O nome é obrigatório." });

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { mensagem = "O e-mail é obrigatório." });

            var email = dto.Email.Trim();
            var numero = dto.Numero?.Trim() ?? string.Empty;

            var emailEmUso = await _context.Users.AnyAsync(u => u.Id != user.Id && u.Email == email);
            if (emailEmUso)
                return Conflict(new { mensagem = "Este e-mail já está em uso por outro usuário." });

            if (!string.IsNullOrEmpty(numero))
            {
                var numeroEmUso = await _context.Users.AnyAsync(u => u.Id != user.Id && u.Numero == numero);
                if (numeroEmUso)
                    return Conflict(new { mensagem = "Este número já está em uso por outro usuário." });
            }

            user.Nome = dto.Nome.Trim();
            user.Email = email;
            user.Numero = numero;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Perfil atualizado com sucesso",
                user.Id,
                user.Nome,
                user.Email,
                user.Numero
            });
        }

        [HttpPut("minha-senha")]
        [Authorize]
        public async Task<IActionResult> AlterarMinhaSenha([FromBody] AlterarSenhaDto dto)
        {
            var userId = ObterIdLogado();
            if (userId == null)
                return Unauthorized(new { mensagem = "Sessão inválida. Faça login novamente." });

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null)
                return NotFound(new { mensagem = "Utilizador não encontrado" });

            if (string.IsNullOrEmpty(dto.SenhaAtual) || string.IsNullOrEmpty(dto.NovaSenha))
                return BadRequest(new { mensagem = "Informe a senha atual e a nova senha." });

            // Mesma comparação que o login usa hoje (texto puro)
            if (user.Password != dto.SenhaAtual)
                return BadRequest(new { mensagem = "A senha atual está incorreta." });

            if (dto.NovaSenha.Length < 6)
                return BadRequest(new { mensagem = "A nova senha precisa ter pelo menos 6 caracteres." });

            if (dto.NovaSenha == dto.SenhaAtual)
                return BadRequest(new { mensagem = "A nova senha deve ser diferente da atual." });

            user.Password = dto.NovaSenha;
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Senha alterada com sucesso" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUser(int id, [FromBody] AtualizarUsuarioDto dto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound(new { mensagem = "Utilizador não encontrado" });

            user.Nome = dto.Nome;
            user.Email = dto.Email;
            user.Roles = string.IsNullOrEmpty(dto.Perfil) ? "TECNICO_CAMPO" : dto.Perfil.ToUpper();
            user.Ativo = dto.Ativo;

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Utilizador atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "GESTOR")]
        public async Task<IActionResult> DeletarUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound(new { mensagem = "Utilizador não encontrado" });

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Utilizador removido com sucesso" });
        }
    }

    public class AtualizarMeuPerfilDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Numero { get; set; }
    }

    public class AlterarSenhaDto
    {
        public string SenhaAtual { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
    }
}
