using ApiCursos.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCursos.Context
{
    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options) : base(options){ }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<AlunoCurso> AlunosCursos { get; set; }

    }
}
