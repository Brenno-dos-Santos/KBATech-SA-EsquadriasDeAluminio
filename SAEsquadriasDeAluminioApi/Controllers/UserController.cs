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
}