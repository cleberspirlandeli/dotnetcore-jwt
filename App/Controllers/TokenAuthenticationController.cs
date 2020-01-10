using App.Data;
using App.Data.Domain;
using App.Infrastructure.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace App.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/login")]
    [ApiController]
    public class TokenAuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationServiceContext _context;

        public TokenAuthenticationController(IConfiguration configuration, ApplicationServiceContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // POST: api/v1/login
        [HttpPost]
        public IActionResult Authenticated(TokenAuthenticationDto dto)
        {
            var result = _context.Usuarios
                            .Where(x =>
                                x.Email == dto.Email &&
                                x.Senha == dto.Senha &&
                                x.Ativo == 1 &&
                                x.EmailAtivo == 1)
                            .FirstOrDefault();

            if(result == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, result.Nome),
                new Claim(ClaimTypes.Email, result.Email),
                new Claim(ClaimTypes.Role, result.Perfil)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKeyToken"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                    issuer: "CrspirlandeliIssuerBackendV1",
                    audience: "CrspirlandeliAudienceBackendV1",
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: credentials
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

    }
}
