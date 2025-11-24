namespace ApiCursos.Models
{
    public class Aluno
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public ICollection<AlunoCurso> AlunoCursos { get; set; }
    }
}
