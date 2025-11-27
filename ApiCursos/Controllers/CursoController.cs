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
            // 200 OK — requisição bem-sucedida, retorna a lista de cursos
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var curso = await _service.GetCursoById(id);
                return Ok(curso); 
                // 200 OK — curso encontrado e retornado com sucesso
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
                // 404 Not Found — o curso não existe no banco de dados
            }
            catch
            {
                return StatusCode(500, "Erro ao buscar curso."); 
                // 500 Internal Server Error — erro inesperado no servidor
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CursoDTO dto)
        {
            try
            {
                await _service.AddCurso(dto);
                return StatusCode(201, "Curso criado com sucesso."); 
                // 201 Created — novo curso criado com sucesso
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); 
                // 400 Bad Request — dados inválidos enviados pelo cliente
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao acessar o banco de dados."); 
                // 500 Internal Server Error — falha ao salvar no banco
            }
        }
 
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CursoDTO dto)
        {
            try
            {
                await _service.UpdateCurso(id, dto);
                return Ok("Curso atualizado com sucesso."); 
                // 200 OK — curso atualizado corretamente
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
                // 404 Not Found — curso não encontrado para atualização
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); 
                // 400 Bad Request — dados inválidos enviados para atualização
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao atualizar o curso."); 
                // 500 Internal Server Error — erro ao salvar atualização no banco
            }
        }
 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteCurso(id);
                return Ok("Curso deletado com sucesso."); 
                // 200 OK — curso deletado corretamente
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
                // 404 Not Found — curso não encontrado para exclusão
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); 
                // 409 Conflict — não é possível deletar devido a conflito (ex: aluno inscrito)
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao acessar o banco de dados."); 
                // 500 Internal Server Error — falha ao tentar deletar no banco
            }
        }
    }
}
