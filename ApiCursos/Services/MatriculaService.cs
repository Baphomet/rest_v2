using ApiCursos.Context;
using ApiCursos.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCursos.Services
{
    public class MatriculaService
    {
        private readonly Db _context;

        public MatriculaService(Db context)
        {
            _context = context;
        }

        public async Task MatricularAsync(Guid alunoId, Guid cursoId)
        {
            var aluno = await _context.Alunos.FindAsync(alunoId);
            if (aluno == null)
                throw new KeyNotFoundException("Aluno não encontrado."); 
                // gera 404 Not Found — aluno inexistente

            var curso = await _context.Cursos.FindAsync(cursoId);
            if (curso == null)
                throw new KeyNotFoundException("Curso não encontrado."); 
                // gera 404 Not Found — curso inexistente

            var existe = await _context.AlunosCursos
                .AnyAsync(ac => ac.AlunoId == alunoId && ac.CursoId == cursoId);

            if (existe)
                throw new InvalidOperationException("O aluno já está matriculado neste curso."); 
                // gera 409 Conflict — matrícula duplicada

            var matricula = new AlunoCurso
            {
                AlunoId = alunoId,
                CursoId = cursoId
            };

            _context.AlunosCursos.Add(matricula);
            await _context.SaveChangesAsync(); 
            // sucesso → normalmente o controller retorna 201 Created
        }

        public async Task RemoverMatriculaAsync(Guid alunoId, Guid cursoId)
        {
            var matricula = await _context.AlunosCursos
                .FirstOrDefaultAsync(ac => ac.AlunoId == alunoId && ac.CursoId == cursoId);

            if (matricula == null)
                throw new KeyNotFoundException("Matrícula não encontrada."); 
                // gera 404 Not Found — não existe essa matrícula

            _context.AlunosCursos.Remove(matricula);
            await _context.SaveChangesAsync();
            // sucesso → controller retorna 200 OK
        }

        public async Task<List<Curso>> GetCursosDoAlunoAsync(Guid alunoId)
        {
            var aluno = await _context.Alunos
                .Include(a => a.AlunoCursos)
                .ThenInclude(ac => ac.Curso)
                .FirstOrDefaultAsync(a => a.Id == alunoId);

            if (aluno == null)
                throw new KeyNotFoundException("Aluno não encontrado."); 
                // gera 404 Not Found

            return aluno.AlunoCursos
                .Select(ac => ac.Curso)
                .ToList(); 
            // sucesso → controller retorna 200 OK
        }

        public async Task<List<Aluno>> GetAlunosDoCursoAsync(Guid cursoId)
        {
            var curso = await _context.Cursos
                .Include(c => c.AlunoCursos)
                .ThenInclude(ac => ac.Aluno)
                .FirstOrDefaultAsync(c => c.Id == cursoId);

            if (curso == null)
                throw new KeyNotFoundException("Curso não encontrado."); 
                // gera 404 Not Found

            return curso.AlunoCursos
                .Select(ac => ac.Aluno)
                .ToList(); 
            // sucesso → controller retorna 200 OK
        }
    }
}
