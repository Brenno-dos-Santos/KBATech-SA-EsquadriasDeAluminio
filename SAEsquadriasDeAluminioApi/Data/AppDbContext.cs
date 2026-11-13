using Microsoft.EntityFrameworkCore;
using SAEsquadriasDeAluminioApi.Models;
namespace SAEsquadriasDeAluminioApi.Data
{
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions options) : base(options)
            {
            }
            public DbSet<User> Users { get; set; }
            public DbSet<HistoricoAcompanhamento> HistoricosAcompanhamentos { get; set; }
            public DbSet<Orcamento> Orcamentos { get; set; }
            public DbSet<ParcelaPagamento> ParcelasPagamentos { get; set; }
            public DbSet<ItemOrcamento> ItensOrcamentos { get; set; }
            public DbSet<Projeto> Projetos { get; set; }
            public DbSet<RegistroObra> RegistrosObras { get; set; }
            public DbSet<DisparoFeedback> DisparosFeedbacks { get; set; }
            public DbSet<LiberacaoInstalacao> LiberacoesInstalacaos { get; set; }
            public DbSet<AvaliacaoPosVenda> AvaliacoesPosVendas { get; set; }
            public DbSet<Equipe> Equipes { get; set; }
            public DbSet<EquipeMembro> EquipeMembros { get; set; }
    }
    }