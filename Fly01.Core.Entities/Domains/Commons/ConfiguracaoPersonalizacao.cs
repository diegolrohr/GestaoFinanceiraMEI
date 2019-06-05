namespace Fly01.Core.Entities.Domains.Commons
{
    public class ConfiguracaoPersonalizacao : PlataformaBase
    {
        public bool EmiteNotaFiscal { get; set; }

        public bool ExibirStepProdutosVendas { get; set; }
        public bool ExibirStepServicosVendas { get; set; }
        public bool ExibirStepTransportadoraVendas { get; set; }

        public bool ExibirStepTransportadoraCompras { get; set; }
    }
}