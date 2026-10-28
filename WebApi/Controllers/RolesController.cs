using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi103.Helpers;
using WebApi103.Services.Interfaces;

namespace WebApi103.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolesController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerRoles()
        {
            var response = await _rolService.ObtenerRoles();
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ById(int id)
        {
            var response = await _rolService.ById(id);
            return response.Succeded ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Rol request)
        {
            Response<Rol> response = await _rolService.Create(request);
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Rol request)
        {
            Response<Rol> response = await _rolService.Update(id, request);
            return response.Succeded ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _rolService.Delete(id);
            if (response.ErrorCode == "role_in_use")
            {
                return Conflict(response);
            }
            return response.Succeded ? Ok(response) : NotFound(response);
        }
    }
}
