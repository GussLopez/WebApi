using Domain.dto;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi103.Services.Interfaces;

namespace WebApi103.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var response = await _usuarioService.ObtenerUsuario();

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ById(int id)
        {
            var response = await _usuarioService.ById(id);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] UsuarioDto i)
        {
            Usuario usuario1 = new Usuario()
            {
                Nombre = i.Nombre,
                UserName = i.UserName,
                Password = i.Password,
                FkRol = i.FkRol
            };
            var result = await _usuarioService.Create(usuario1);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _usuarioService.Delete(id);
            return Ok(result);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioDto request)
        {
            Usuario usuarioParaActualizar = new Usuario()
            {
                Nombre = request.Nombre,
                UserName = request.UserName,
                Password = request.Password,
                FkRol = request.FkRol
            };

            var response = await _usuarioService.Update(id, usuarioParaActualizar);

            if (!response.Succeded && response.Message == "El usuario no existe.")
            {
                return NotFound(response);
            }

            return Ok(response);
        }

    }
}