namespace ApiCursos.Models
{
    public class Usuario
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }

    }
}
