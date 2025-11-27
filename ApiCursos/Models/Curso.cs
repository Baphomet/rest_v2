using System.Text.Json.Serialization;

namespace ApiCursos.Models
{
    public class Curso
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        [JsonIgnore]
        public ICollection<AlunoCurso> AlunoCursos { get; set; }
    }
}
