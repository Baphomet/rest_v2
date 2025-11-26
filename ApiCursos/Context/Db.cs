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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // chave composta
            modelBuilder.Entity<AlunoCurso>()
                .HasKey(ac => new { ac.AlunoId, ac.CursoId });

            // relacionamento N:N
            modelBuilder.Entity<AlunoCurso>()
                .HasOne(ac => ac.Aluno)
                .WithMany(a => a.AlunoCursos)
                .HasForeignKey(ac => ac.AlunoId);

            modelBuilder.Entity<AlunoCurso>()
                .HasOne(ac => ac.Curso)
                .WithMany(c => c.AlunoCursos)
                .HasForeignKey(ac => ac.CursoId);
        }
    }
}
