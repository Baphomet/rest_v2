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
    public class CursoController : ControllerBase
    {
        private readonly CursoService _service;
 
        public CursoController(CursoService service)
        {
            _service = service;
        }
 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cursos = await _service.GetAllCursos();
            return Ok(cursos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var curso = await _service.GetCursoById(id);
                return Ok(curso);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch
            {
                return StatusCode(500, "Erro ao buscar curso."); 
            }
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CursoDTO dto)
        {
            try
            {
                await _service.AddCurso(dto);
                return StatusCode(201, "Curso criado com sucesso.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao acessar o banco de dados.");
            }
        }
 
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CursoDTO dto)
        {
            try
            {
                await _service.UpdateCurso(id, dto);
                return Ok("Curso atualizado com sucesso.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao atualizar o curso.");
            }
        }
 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteCurso(id);
                return Ok("Curso deletado com sucesso.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao acessar o banco de dados.");
            }
        }
    }
}
