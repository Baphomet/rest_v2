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

        public async Task AddAluno(AlunoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new ArgumentException("O nome do aluno é obrigatório.");

            var aluno = new Aluno
            {
                Nome = dto.Nome,
                DataNascimento = dto.DataNascimento
            };

            try
            {
                await _context.Alunos.AddAsync(aluno);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Erro ao inserir aluno no banco de dados.");
            }
        }

        public async Task UpdateAluno(Guid id, AlunoDTO dto)
        {
            var aluno = await _context.Alunos.FirstOrDefaultAsync(a => a.Id == id);

            if (aluno == null)
                throw new KeyNotFoundException("Aluno não encontrado.");

            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new ArgumentException("O nome do aluno é obrigatório.");

            aluno.Nome = dto.Nome;
            aluno.DataNascimento = dto.DataNascimento;

            try
            {
                _context.Alunos.Update(aluno);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Erro ao atualizar aluno no banco de dados.");
            }
        }

        public async Task DeleteAluno(Guid id)
        {
            var aluno = await _context.Alunos.FindAsync(id);

            if (aluno == null)
                throw new KeyNotFoundException("Aluno não encontrado.");

            try
            {
                _context.Alunos.Remove(aluno);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Erro de integridade ao tentar deletar o aluno.");
            }
        }
    }
}
