// Models/Equipe.cs
namespace SAEsquadriasDeAluminioApi.Models
{
    public class Equipe
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Especialidade { get; set; }
        public bool Ativa { get; set; } = true;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public ICollection<EquipeMembro> Membros { get; set; } = new List<EquipeMembro>();
    }
}