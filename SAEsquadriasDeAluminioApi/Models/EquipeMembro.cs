// Models/EquipeMembro.cs
namespace SAEsquadriasDeAluminioApi.Models
{
    public class EquipeMembro
    {
        public int Id { get; set; }
        public int EquipeId { get; set; }
        public Equipe? Equipe { get; set; }

        public int UsuarioId { get; set; }
        public User? User { get; set; }

        public string FuncaoNaEquipe { get; set; } = "Instalador";
    }
}