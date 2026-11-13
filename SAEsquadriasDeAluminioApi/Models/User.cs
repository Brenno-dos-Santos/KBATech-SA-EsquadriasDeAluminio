namespace SAEsquadriasDeAluminioApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Email { get; set; }
        public string Numero { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }
        public Boolean Ativo { get; set; }
    }
}
