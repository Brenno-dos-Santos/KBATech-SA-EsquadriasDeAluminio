namespace SAEsquadriasDeAluminioApi.Models
{
    public class Ocorrencia
    {
        public int Id { get; set; }
        public int ProjetoId { get; set; }
        public Projeto? Projeto { get; set; }

        public int ReportadoPorId { get; set; }
        public User? ReportadoPor { get; set; }

        public string Titulo { get; set; } = string.Empty;
        public string Tipo { get; set; } = "TECNICA"; // TECNICA, MATERIAL, EMBARGO, CLIENTE
        public string Severidade { get; set; } = "MEDIA"; // BAIXA, MEDIA, ALTA, CRITICA
        public string Descricao { get; set; } = string.Empty;
        public string? FotoUrl { get; set; }

        public bool BloqueiaObra { get; set; } = false;
        public bool Resolvida { get; set; } = false;
        public string? SolucaoAplicada { get; set; }

        public DateTime DataAbertura { get; set; } = DateTime.UtcNow;
        public DateTime? DataResolucao { get; set; }
    }
}