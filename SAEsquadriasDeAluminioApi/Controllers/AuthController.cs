using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SAEsquadriasDeAluminioApi.Data;
using SAEsquadriasDeAluminioApi.Models;

namespace SAEsquadriasDeAluminioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                (!string.IsNullOrEmpty(dto.Email) && u.Email == dto.Email) ||
                (!string.IsNullOrEmpty(dto.Numero) && u.Numero == dto.Numero));

            if (user == null || !user.Ativo)
                return Unauthorized(new { mensagem = "Utilizador não encontrado ou inativo" });

            if (user.Password != dto.Password)
                return Unauthorized(new { mensagem = "Senha incorreta" });

            var jwtSecretKey = _configuration["Jwt:SecretKey"] ?? "minha-chave-super-segura-com-32-caracteres";
            var key = Encoding.ASCII.GetBytes(jwtSecretKey);

            var roleFormatada = string.IsNullOrEmpty(user.Roles) ? "TECNICO_CAMPO" : user.Roles.ToUpper();

            var claims = new[]
            {
                new System.Security.Claims.Claim("id", user.Id.ToString()),
                new System.Security.Claims.Claim(ClaimTypes.Name, user.Nome),
                new System.Security.Claims.Claim(ClaimTypes.Role, roleFormatada)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return Ok(new
            {
                mensagem = "Login realizado com sucesso",
                token,
                usuario = new
                {
                    user.Id,
                    user.Nome,
                    user.Email,
                    user.Numero,
                    Roles = roleFormatada
                }
            });
        }
    }
}