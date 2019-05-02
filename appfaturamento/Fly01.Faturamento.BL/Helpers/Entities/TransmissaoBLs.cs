namespace Fly01.Faturamento.BL.Helpers.EntitiesBL
{
    public class TransmissaoBLs
    {
        public SerieNotaFiscalBL SerieNotaFiscalBL { get; set; }
        public NFeProdutoBL NFeProdutoBL { get; set; }
        public TotalTributacaoBL TotalTributacaoBL { get; set; }
        public PessoaBL PessoaBL { get; set; }
        public CondicaoParcelamentoBL CondicaoParcelamentoBL { get; set; }
        public FormaPagamentoBL FormaPagamentoBL { get; set; }
        public SubstituicaoTributariaBL SubstituicaoTributariaBL { get; set; }
        public NotaFiscalItemTributacaoBL NotaFiscalItemTributacaoBL { get; set; }
        public CertificadoDigitalBL CertificadoDigitalBL { get; set; }
        public NotaFiscalInutilizadaBL NotaFiscalInutilizadaBL { get; set; }
        public string PlataformaUrl { get; set; }
        public string AppUser { get; set; }
    }
}
