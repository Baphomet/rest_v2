using ApiCursos.DTOs;
using ApiCursos.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiCursos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculaController : ControllerBase
    {
        private readonly MatriculaService _service;

        public MatriculaController(MatriculaService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Matricular([FromBody] MatriculaDTO dto)
        {
            try
            {
                await _service.MatricularAsync(dto.AlunoId, dto.CursoId);
                return Ok("Aluno matriculado com sucesso!");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); 
            }
            catch
            {
                return StatusCode(500, "Erro ao realizar matrícula."); 
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Remover([FromBody] MatriculaDTO dto)
        {
            try
            {
                await _service.RemoverMatriculaAsync(dto.AlunoId, dto.CursoId);
                return Ok("Matrícula removida com sucesso!");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch
            {
                return StatusCode(500, "Erro ao remover matrícula."); 
            }
        }

        [HttpGet("aluno/{alunoId}/cursos")]
        public async Task<IActionResult> GetCursosDoAluno(Guid alunoId)
        {
            try
            {
                var cursos = await _service.GetCursosDoAlunoAsync(alunoId);
                return Ok(cursos);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch
            {
                return StatusCode(500, "Erro ao buscar cursos do aluno.");
            }
        }

        [HttpGet("curso/{cursoId}/alunos")]
        public async Task<IActionResult> GetAlunosDoCurso(Guid cursoId)
        {
            try
            {
                var alunos = await _service.GetAlunosDoCursoAsync(cursoId);
                return Ok(alunos);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch
            {
                return StatusCode(500, "Erro ao buscar alunos do curso."); 
            }
        }


    }
}
