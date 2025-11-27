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
                return Ok(aluno); // 200 OK — recurso encontrado
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 Not Found — o ID não existe no banco
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
                return StatusCode(201, "Aluno criado com sucesso."); // 201 Created — recurso criado com sucesso
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // 400 Bad Request — dados inválidos enviados pelo cliente
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao acessar o banco de dados."); // 500 Internal Server Error — falha ao salvar no banco
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AlunoDTO dto)
        {
            try
            {
                await _service.UpdateAluno(id, dto);
                return Ok("Aluno atualizado com sucesso."); // 200 OK — atualização concluída
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 Not Found — o aluno não existe
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // 400 Bad Request — falha de validação no corpo da requisição
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao atualizar o aluno."); // 500 Internal Server Error — erro ao persistir no banco
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteAluno(id);
                return Ok("Aluno deletado com sucesso."); // 200 OK — recurso removido corretamente
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 Not Found — não existe um aluno com esse ID
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); // 409 Conflict — conflito, o aluno não pode ser deletado
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao acessar o banco de dados."); // 500 Internal Server Error — falha ao remover no banco
            }
        }
    }
}
