using ApiCursos.DTOs;
using ApiCursos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCursos.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _service.GetAllUsuarios();
            return Ok(usuarios); 
            // 200 OK — lista de usuários retornada
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var usuario = await _service.GetUsuarioById(id);
                return Ok(usuario);
                // 200 OK — usuário encontrado e retornado
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
                // 404 Not Found — usuário não existe
            }
            catch
            {
                return StatusCode(500, "Erro ao buscar usuário.");
                // 500 Internal Server Error — problema inesperado
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Add([FromBody] UsuarioDTO dto)
        {
            try
            {
                await _service.AddUsuario(dto);
                return StatusCode(201, "Usuário criado com sucesso.");
                // 201 Created — usuário registrado corretamente
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
                // 400 Bad Request — dados inválidos enviados
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao acessar o banco de dados.");
                // 500 Internal Server Error — erro ao salvar no banco
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UsuarioDTO dto)
        {
            try
            {
                await _service.UpdateUsuario(id, dto);
                return Ok("Usuário atualizado com sucesso.");
                // 200 OK — atualização concluída
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
                // 404 Not Found — usuário não existe
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
                // 400 Bad Request — erro nos dados enviados
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao atualizar o usuário.");
                // 500 Internal Server Error — falha ao salvar no banco
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteUsuario(id);
                return Ok("Usuário deletado com sucesso.");
                // 200 OK — usuário removido
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
                // 404 Not Found — usuário não encontrado
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
                // 409 Conflict — operação não permitida
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao acessar o banco de dados.");
                // 500 Internal Server Error — falha inesperada
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto, [FromServices] IConfiguration config)
        {
            try
            {
                var token = await _service.LoginAsync(dto, config);
                return Ok(new { Token = token });
                // 200 OK — login válido, token gerado
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
                // 401 Unauthorized — credenciais inválidas
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
                // 400 Bad Request — dados incompletos ou inválidos
            }
            catch
            {
                return StatusCode(500, "Erro interno ao gerar token.");
                // 500 Internal Server Error — erro inesperado ao gerar JWT
            }
        }

    }
}
