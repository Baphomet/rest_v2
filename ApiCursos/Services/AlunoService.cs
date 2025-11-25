using ApiCursos.Context;
using ApiCursos.DTOs;
using ApiCursos.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCursos.Services
{
    public class AlunoService
    {
        private readonly Db _context;

        public AlunoService(Db context)
        {
            _context = context;
        }

        public async Task<List<Aluno>> GetAllAlunos()
        {
            return await _context.Alunos
                .Include(a => a.AlunoCursos)
                .ThenInclude(ac => ac.Curso)
                .ToListAsync();
        }

        public async Task<string> AddAluno(AlunoDTO dto)
        {
            Aluno aluno = new Aluno();
            aluno.Nome = dto.Nome;
            aluno.Email = dto.Email;
            aluno.DataNascimento = dto.DataNascimento;

            await _context.Alunos.AddAsync(aluno);
            await _context.SaveChangesAsync();

            return "Aluno adicionado com sucesso";
        }

        public async Task<string> UpdateAluno(Guid id, AlunoDTO dto)
        {
            var aluno = await _context.Alunos.FirstOrDefaultAsync(a => a.Id == id);
            if (aluno == null) return "Aluno não encontrado";

            aluno.Nome = dto.Nome;
            aluno.Email = dto.Email;
            aluno.DataNascimento = dto.DataNascimento;

            _context.Alunos.Update(aluno);
            await _context.SaveChangesAsync();

            return "Aluno atualizado com sucesso";
        }

        public async Task<string> DeleteAluno(Guid id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null) return "Aluno não encontrado";

            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();

            return "Aluno deletado com sucesso";
        }
    }
}
