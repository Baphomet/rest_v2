using ApiCursos.Context;
using ApiCursos.DTOs;
using ApiCursos.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCursos.Services
{
    public class CursoService
    {
        private readonly Db _context;

        public CursoService(Db context)
        {
            _context = context;
        }

        public async Task<List<Curso>> GetAllCursos()
        {
            return await _context.Cursos
                .Include(c => c.AlunoCursos)
                .ThenInclude(ac => ac.Aluno)
                .ToListAsync();
        }

        public async Task<Curso> GetCursoById(Guid id)
        {
            var curso = await _context.Cursos
                .Include(c => c.AlunoCursos)
                .ThenInclude(ac => ac.Aluno)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null)
                throw new KeyNotFoundException("Curso não encontrado."); 
                // gera 404 Not Found no controller

            return curso;
        }

        public async Task AddCurso(CursoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Titulo))
                throw new ArgumentException("O título do curso é obrigatório."); 
                // gera 400 Bad Request

            var curso = new Curso
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao
            };

            try
            {
                await _context.Cursos.AddAsync(curso);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Erro ao inserir curso no banco de dados."); 
                // gera 500 Internal Server Error
            }
        }

        public async Task UpdateCurso(Guid id, CursoDTO dto)
        {
            var curso = await _context.Cursos.FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null)
                throw new KeyNotFoundException("Curso não encontrado."); 
                // gera 404 Not Found

            if (string.IsNullOrWhiteSpace(dto.Titulo))
                throw new ArgumentException("O título do curso é obrigatório."); 
                // gera 400 Bad Request

            curso.Titulo = dto.Titulo;
            curso.Descricao = dto.Descricao;

            try
            {
                _context.Cursos.Update(curso);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Erro ao atualizar curso no banco de dados."); 
                // gera 500 Internal Server Error
            }
        }

        public async Task DeleteCurso(Guid id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
                throw new KeyNotFoundException("Curso não encontrado."); 
                // gera 404 Not Found

            try
            {
                _context.Cursos.Remove(curso);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Erro de integridade ao tentar deletar o curso."); 
                // gera 409 Conflict no controller
            }
        }
    }
}
