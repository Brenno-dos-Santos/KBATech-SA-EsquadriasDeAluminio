namespace SAEsquadriasDeAluminioApi.Models
{
    public class HistoricoAcompanhamento
    {
        public int Id { get; set; }
        public int ProjetoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataHora { get; set; }
        public string Acao { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }

    public class Orcamento
    {
        public int Id { get; set; }
        public int VendedorId { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string? TelefoneCliente { get; set; }
        public string? EmailCliente { get; set; }
        public string? EnderecoObra { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? PrazoEstimado { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; } = string.Empty; // RASCUNHO | PENDENTE | APROVADO | REJEITADO
    }

    public class ParcelaPagamento
    {
        public int Id { get; set; }
        public int OrcamentoId { get; set; }
        public string FormaPagamento { get; set; } = string.Empty; // CARTAO_CREDITO | BOLETO | CHEQUE
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public string StatusPagamento { get; set; } = string.Empty; // PENDENTE | PAGO | CANCELADO
    }

    public class ItemOrcamento
    {
        public int Id { get; set; }
        public int OrcamentoId { get; set; }
        public string DescricaoItem { get; set; } = string.Empty;
        public float Altura { get; set; }
        public float Largura { get; set; }
        public string? TipoAbertura { get; set; }
        public string? LadoAbertura { get; set; }
        public string? PosicaoFechadura { get; set; }
        public decimal ValorUnitario { get; set; }
        public int Quantidade { get; set; }
    }

    public class Projeto
    {
        public int Id { get; set; }
        public int OrcamentoId { get; set; }
        public DateTime? DataMedicaoFinal { get; set; }
        public DateTime? DataInicioContrato { get; set; }
        public DateTime? PrazoFinalEntrega { get; set; }
        public string StatusProducao { get; set; } = string.Empty; // AGUARDANDO_MEDICAO | EM_PRODUCAO | FABRICADO | EM_INSTALACAO | CONCLUIDO | EMBARGADO
    }

    public class RegistroObra
    {
        public int Id { get; set; }
        public int ProjetoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataRegistro { get; set; }
        public string EtapaProcesso { get; set; } = string.Empty; // INICIO_OBRA | EM_ANDAMENTO | CONCLUIDO | OCORRENCIA_ERRO
        public string TipoOcorrencia { get; set; } = string.Empty; // NENHUMA | DIFICULDADE_TECNICA | ITEM_INACABADO | DIVERGENCIA_ESCOPO | DANO_MATERIAL
        public string? Descricao { get; set; }
        public string? FotoUrl { get; set; }
    }

    public class DisparoFeedback
    {
        public int Id { get; set; }
        public int ProjetoId { get; set; }
        public string Canal { get; set; } = string.Empty; // WHATSAPP | EMAIL | SMS
        public string TokenAcesso { get; set; } = string.Empty;
        public DateTime? DataEnvio { get; set; }
        public string StatusEnvio { get; set; } = string.Empty; // PENDENTE | ENVIADO | ERRO
    }

    public class LiberacaoInstalacao
    {
        public int Id { get; set; }
        public int ProjetoId { get; set; }
        public int VendedorId { get; set; }
        public DateTime DataLiberacao { get; set; }
        public string? EscopoLiberado { get; set; }
        public string? Observacoes { get; set; }
        public bool Liberado { get; set; }
    }

    public class AvaliacaoPosVenda
    {
        public int Id { get; set; }
        public int DisparoId { get; set; }
        public int ProjetoId { get; set; }
        public int Nota { get; set; }
        public string? Comentario { get; set; }
        public DateTime DataResposta { get; set; }
    }
}
    

