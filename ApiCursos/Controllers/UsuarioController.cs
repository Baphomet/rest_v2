using ApiCursos.DTOs;
using ApiCursos.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCursos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public readonly UsuarioService _service;

        public UsuarioController(UsuarioService service) 
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _service.GetAllUsuarios());
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] UsuarioDTO dto) 
        {
        string resultado = await _service.AddUsuario(dto);
            if (resultado.Contains("Não")) return BadRequest(resultado);
            return Ok(resultado);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UsuarioDTO dto) 
        {
        string resultado = await _service.UpdateUsuario(id, dto);
            if (resultado.Contains("Não")) return BadRequest(resultado);
            return Ok(resultado);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) 
        {
            string resultado = await _service.DeleteUsuario(id);
            if (resultado.Contains("Não")) return BadRequest(resultado);
            return Ok(resultado);
        }
    }
}
