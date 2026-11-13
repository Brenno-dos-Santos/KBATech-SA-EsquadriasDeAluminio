namespace SAEsquadriasDeAluminioApi.Models
{
    public class DTOs
    {
        // ==========================================
        // USUÁRIOS
        // ==========================================
        public class UsuarioResponseDto
        {
            public int Id { get; set; }
            public string Nome { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string? Numero { get; set; }
            public string Perfil { get; set; } = string.Empty;
            public bool Ativo { get; set; }
        }

        public class AtualizarUsuarioDto
        {
            public string Nome { get; set; } = string.Empty;
            public string? Email { get; set; }
            public string? Numero { get; set; }
            public string Perfil { get; set; } = string.Empty;
            public bool Ativo { get; set; }
        }

        // ==========================================
        // ORÇAMENTOS E CRM
        // ==========================================
        public class CriarOrcamentoDto
        {
            public int UsuarioId { get; set; }
            public string NomeCliente { get; set; } = string.Empty;
            public string TelefoneCliente { get; set; } = string.Empty;
            public string? EmailCliente { get; set; }
            public string EnderecoObra { get; set; } = string.Empty;
            public DateTime? PrazoEstimado { get; set; }
            public decimal ValorTotal { get; set; }
            public string Status { get; set; } = string.Empty;
        }

        public class AtualizarOrcamentoDto
        {
            public string NomeCliente { get; set; } = string.Empty;
            public string TelefoneCliente { get; set; } = string.Empty;
            public string? EmailCliente { get; set; }
            public string EnderecoObra { get; set; } = string.Empty;
            public DateTime? PrazoEstimado { get; set; }
            public decimal ValorTotal { get; set; }
            public string Status { get; set; } = string.Empty;
        }

        public class OrcamentoResponseDto
        {
            public int Id { get; set; }
            public int UsuarioId { get; set; }
            public string NomeCliente { get; set; } = string.Empty;
            public string TelefoneCliente { get; set; } = string.Empty;
            public string? EmailCliente { get; set; }
            public string EnderecoObra { get; set; } = string.Empty;
            public DateTime DataCriacao { get; set; }
            public DateTime? PrazoEstimado { get; set; }
            public decimal ValorTotal { get; set; }
            public string Status { get; set; } = string.Empty;
        }

        public class ClienteCrmResponseDto
        {
            public string NomeCliente { get; set; } = string.Empty;
            public string TelefoneCliente { get; set; } = string.Empty;
            public string? EmailCliente { get; set; }
            public string EnderecoObra { get; set; } = string.Empty;
            public int TotalOrcamentos { get; set; }
            public decimal ValorTotalComprado { get; set; }
            public DateTime UltimoContato { get; set; }
        }

        // ==========================================
        // ITENS DO ORÇAMENTO
        // ==========================================
        // ==========================================
        // ITENS DO ORÇAMENTO
        // ==========================================
        public class CriarItemOrcamentoDto
        {
            public int OrcamentoId { get; set; }
            public string DescricaoItem { get; set; } = string.Empty;
            public float Altura { get; set; }  // <-- Alterado de decimal para float
            public float Largura { get; set; } // <-- Alterado de decimal para float
            public string TipoAbertura { get; set; } = string.Empty;
            public string LadoAbertura { get; set; } = string.Empty;
            public string PosicaoFechadura { get; set; } = string.Empty;
            public decimal ValorUnitario { get; set; }
            public int Quantidade { get; set; }
        }

        public class ItemOrcamentoResponseDto
        {
            public int Id { get; set; }
            public int OrcamentoId { get; set; }
            public string DescricaoItem { get; set; } = string.Empty;
            public float Altura { get; set; }  // <-- Alterado de decimal para float
            public float Largura { get; set; } // <-- Alterado de decimal para float
            public string TipoAbertura { get; set; } = string.Empty;
            public string LadoAbertura { get; set; } = string.Empty;
            public string PosicaoFechadura { get; set; } = string.Empty;
            public decimal ValorUnitario { get; set; }
            public int Quantidade { get; set; }
        }
        // ==========================================
        // PARCELAS DE PAGAMENTO
        // ==========================================
        public class CriarParcelaPagamentoDto
        {
            public int OrcamentoId { get; set; }
            public string FormaPagamento { get; set; } = string.Empty;
            public decimal Valor { get; set; }
            public DateTime DataVencimento { get; set; }
            public string StatusPagamento { get; set; } = string.Empty;
        }

        public class ParcelaPagamentoResponseDto
        {
            public int Id { get; set; }
            public int OrcamentoId { get; set; }
            public string FormaPagamento { get; set; } = string.Empty;
            public decimal Valor { get; set; }
            public DateTime DataVencimento { get; set; }
            public string StatusPagamento { get; set; } = string.Empty;
        }

        // ==========================================
        // PROJETOS E HISTÓRICO
        // ==========================================
        public class CriarProjetoDto
        {
            public int OrcamentoId { get; set; }
            public int? TecnicoId { get; set; }
            public DateTime? DataMedicaoFinal { get; set; }
            public DateTime? DataInicioContrato { get; set; }
            public DateTime? PrazoFinalEntrega { get; set; }
            public string StatusProducao { get; set; } = string.Empty;
        }

        public class AtualizarStatusProjetoDto
        {
            public int? TecnicoId { get; set; }
            public string StatusProducao { get; set; } = string.Empty;
            public DateTime? DataMedicaoFinal { get; set; }
            public DateTime? PrazoFinalEntrega { get; set; }
        }

        public class ProjetoResponseDto
        {
            public int Id { get; set; }
            public int OrcamentoId { get; set; }
            public int? TecnicoId { get; set; }
            public DateTime? DataMedicaoFinal { get; set; }
            public DateTime DataInicioContrato { get; set; }
            public DateTime? PrazoFinalEntrega { get; set; }
            public string StatusProducao { get; set; } = string.Empty;
        }

        public class HistoricoAcompanhamentoResponseDto
        {
            public int Id { get; set; }
            public int ProjetoId { get; set; }
            public int UsuarioId { get; set; }
            public DateTime DataHora { get; set; }
            public string Acao { get; set; } = string.Empty;
            public string Descricao { get; set; } = string.Empty;
        }

        // ==========================================
        // REGISTRO DE OBRA E OCORRÊNCIAS
        // ==========================================
        public class CriarRegistroObraDto
        {
            public int ProjetoId { get; set; }
            public int UsuarioId { get; set; }
            public string EtapaProcesso { get; set; } = string.Empty;
            public string TipoOcorrencia { get; set; } = string.Empty;
            public string Descricao { get; set; } = string.Empty;
            public string? FotoUrl { get; set; }
            public bool Resolvido { get; set; }
        }

        public class RegistroObraResponseDto
        {
            public int Id { get; set; }
            public int ProjetoId { get; set; }
            public int UsuarioId { get; set; }
            public DateTime DataRegistro { get; set; }
            public string EtapaProcesso { get; set; } = string.Empty;
            public string TipoOcorrencia { get; set; } = string.Empty;
            public string Descricao { get; set; } = string.Empty;
            public string? FotoUrl { get; set; }
            public bool Resolvido { get; set; }
        }

        // ==========================================
        // LIBERAÇÃO DE INSTALAÇÃO
        // ==========================================
        public class CriarLiberacaoInstalacaoDto
        {
            public int ProjetoId { get; set; }
            public int UsuarioId { get; set; }
            public string EscopoLiberado { get; set; } = string.Empty;
            public string? Observacoes { get; set; }
            public bool Liberado { get; set; }
        }

        public class LiberacaoInstalacaoResponseDto
        {
            public int Id { get; set; }
            public int ProjetoId { get; set; }
            public int UsuarioId { get; set; }
            public DateTime DataLiberacao { get; set; }
            public string EscopoLiberado { get; set; } = string.Empty;
            public string? Observacoes { get; set; }
            public bool Liberado { get; set; }
        }

        // ==========================================
        // FEEDBACK E PÓS-VENDA
        // ==========================================
        public class CriarDisparoFeedbackDto
        {
            public int ProjetoId { get; set; }
            public string Canal { get; set; } = string.Empty;
        }

        public class DisparoFeedbackResponseDto
        {
            public int Id { get; set; }
            public int ProjetoId { get; set; }
            public string Canal { get; set; } = string.Empty;
            public string TokenAcesso { get; set; } = string.Empty;
            public DateTime DataEnvio { get; set; }
            public string StatusEnvio { get; set; } = string.Empty;
        }

        public class CriarAvaliacaoPosVendaDto
        {
            public int DisparoId { get; set; }
            public int ProjetoId { get; set; }
            public int Nota { get; set; }
            public string? Comentario { get; set; }
        }

        public class AvaliacaoPosVendaResponseDto
        {
            public int Id { get; set; }
            public int DisparoId { get; set; }
            public int ProjetoId { get; set; }
            public int Nota { get; set; }
            public string? Comentario { get; set; }
            public DateTime DataResposta { get; set; }
        }
    }
}