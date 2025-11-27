using ApiCursos.DTOs;
using ApiCursos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiCursos.Controllers
{
    [Authorize]
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
                // 200 OK — matrícula realizada corretamente
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
                // 404 Not Found — aluno ou curso não encontrado para matrícula
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
                // 409 Conflict — aluno já matriculado ou regra de negócio impedindo a operação
            }
            catch
            {
                return StatusCode(500, "Erro ao realizar matrícula.");
                // 500 Internal Server Error — erro inesperado no servidor
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Remover([FromBody] MatriculaDTO dto)
        {
            try
            {
                await _service.RemoverMatriculaAsync(dto.AlunoId, dto.CursoId);
                return Ok("Matrícula removida com sucesso!");
                // 200 OK — matrícula removida corretamente
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
                // 404 Not Found — matrícula não encontrada para remoção
            }
            catch
            {
                return StatusCode(500, "Erro ao remover matrícula.");
                // 500 Internal Server Error — falha inesperada ao remover
            }
        }

        [HttpGet("aluno/{alunoId}/cursos")]
        public async Task<IActionResult> GetCursosDoAluno(Guid alunoId)
        {
            try
            {
                var cursos = await _service.GetCursosDoAlunoAsync(alunoId);
                return Ok(cursos);
                // 200 OK — lista de cursos do aluno retornada com sucesso
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
                // 404 Not Found — aluno não encontrado
            }
            catch
            {
                return StatusCode(500, "Erro ao buscar cursos do aluno.");
                // 500 Internal Server Error — erro inesperado ao consultar dados
            }
        }

        [HttpGet("curso/{cursoId}/alunos")]
        public async Task<IActionResult> GetAlunosDoCurso(Guid cursoId)
        {
            try
            {
                var alunos = await _service.GetAlunosDoCursoAsync(cursoId);
                return Ok(alunos);
                // 200 OK — lista de alunos do curso retornada com sucesso
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
                // 404 Not Found — curso não encontrado
            }
            catch
            {
                return StatusCode(500, "Erro ao buscar alunos do curso.");
                // 500 Internal Server Error — falha inesperada ao carregar alunos
            }
        }
    }
}
