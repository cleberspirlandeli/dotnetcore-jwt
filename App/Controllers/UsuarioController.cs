using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using App.Data;
using App.Data.Domain;
using App.Infrastructure.Dto;
using App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly UsuarioService _appService;

        public UsuarioController(UsuarioService appService)
        {
            _appService = appService;
        }

        // POST: api/v1/Usuario
        [HttpPost]
        [Allowanonymous]
        public async Task<IActionResult> CreateNewUser(UsuarioDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _appService.CreateNewUser(dto);
            
            return Ok(result);
        }

        // GET: api/v1/Usuario/5
        [HttpGet("{idUsuario}")]
        public async Task<IActionResult> GetUsuarioById([FromRoute] int idUsuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _appService.GetUsuarioById(idUsuario);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // GET: api/v1/Usuario/5
        [HttpGet("{idUsuario}")]
        public async Task<IActionResult> GetUsuarioByEmail([FromBody] string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _appService.GetUsuarioByEmail(email);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // GET: api/v1/confirm-email
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string codigo)
        {
            _appService.ConfirmEmail(email, codigo);

            return Ok();
        }
    }
}