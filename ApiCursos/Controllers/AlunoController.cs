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
    public class AlunoController : ControllerBase
    {
        private readonly AlunoService _service;

        public AlunoController(AlunoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alunos = await _service.GetAllAlunos();
            return Ok(alunos); // 200 OK — requisição bem-sucedida
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var aluno = await _service.GetAlunoById(id);
                return Ok(aluno); // 200 OK — encontrado com sucesso
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 Not Found — quando o ID não existe no banco
            }
            catch
            {
                return StatusCode(500, "Erro ao buscar aluno."); // 500 Internal Server Error — erro inesperado no servidor
            }
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AlunoDTO dto)
        {
            try
            {
                await _service.AddAluno(dto);
                return StatusCode(201, "Aluno criado com sucesso.");  // 201 Created — recurso criado com sucesso
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // 400 Bad Request — erro de validação (dados inválidos)
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao acessar o banco de dados.");  // 500 — erro ao tentar salvar no banco
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AlunoDTO dto)
        {
            try
            {
                await _service.UpdateAluno(id, dto);
                return Ok("Aluno atualizado com sucesso."); // 200 OK — atualização feita
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 — o aluno não existe
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);  // 400 — validação falhou (ex.: nome vazio, idade inválida)
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao atualizar o aluno."); // 500 — falha no banco durante o update
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteAluno(id);
                return Ok("Aluno deletado com sucesso."); // 200 OK — excluído
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 — o aluno não existe
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); // 409 Conflict — algo impede o delete (ex.: aluno matriculado)
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao acessar o banco de dados."); // 500 — falha no banco durante a remoção
            }
        }
    }
}
