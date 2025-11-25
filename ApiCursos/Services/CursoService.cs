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
 
        public async Task<string> AddCurso(CursoDTO dto)
        {
            Curso curso = new Curso();
            curso.Titulo = dto.Titulo;
            curso.Descricao = dto.Descricao;
 
            await _context.Cursos.AddAsync(curso);
            await _context.SaveChangesAsync();
 
            return "Curso adicionado com sucesso";
        }
 
        public async Task<string> UpdateCurso(Guid id, CursoDTO dto)
        {
            var curso = await _context.Cursos.FirstOrDefaultAsync(c => c.Id == id);
            if (curso == null) return "Curso não encontrado";
 
            curso.Titulo = dto.Titulo;
            curso.Descricao = dto.Descricao;
 
            _context.Cursos.Update(curso);
            await _context.SaveChangesAsync();
 
            return "Curso atualizado com sucesso";
        }
 
        public async Task<string> DeleteCurso(Guid id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) return "Curso não encontrado";
 
            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
 
            return "Curso deletado com sucesso";
        }
    }
}
