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
        public async Task<IActionResult> Usuario(UsuarioDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _appService.Insert(dto);
            
            return CreatedAtAction("GetByIdUsuario", new { id = result.Id }, result);
        }

        // GET: api/v1/Produto/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdUsuario([FromRoute] int id)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //var usuario = await _context.Usuarios.FindAsync(id);

            //if (usuario == null)
            //{
            //    return NotFound();
            //}

            return Ok(id);
        }

    }
}