namespace ApiCursos.Models
{
    public class AlunoCurso
    {
        public Guid AlunoId { get; set; }
        public Aluno Aluno { get; set; }
        public Guid CursoId { get; set; }
        public Curso Curso { get; set; }
    }
}
