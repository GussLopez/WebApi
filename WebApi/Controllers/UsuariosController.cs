using Domain.dto;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi103.Helpers;
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

            return response.Succeded ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] UsuarioDto i)
        {
            if (string.IsNullOrWhiteSpace(i.Password))
            {
                ModelState.AddModelError(nameof(i.Password), "La contraseña es obligatoria.");
                return ValidationProblem(ModelState);
            }

            Usuario usuario1 = new Usuario()
            {
                Nombre = i.Nombre,
                UserName = i.UserName,
                Password = i.Password,
                FkRol = i.FkRol
            };
            Response<Usuario> result = await _usuarioService.Create(usuario1);

            return result.Succeded ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Response<Usuario> result = await _usuarioService.Delete(id);
            return result.Succeded ? Ok(result) : NotFound(result);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioDto request)
        {
            Usuario usuarioParaActualizar = new Usuario()
            {
                Nombre = request.Nombre,
                UserName = request.UserName,
                Password = request.Password ?? string.Empty,
                FkRol = request.FkRol
            };

            Response<Usuario> response = await _usuarioService.Update(id, usuarioParaActualizar);

            if (response.ErrorCode == "invalid_role")
            {
                return BadRequest(response);
            }

            return response.Succeded ? Ok(response) : NotFound(response);
        }

    }
}
