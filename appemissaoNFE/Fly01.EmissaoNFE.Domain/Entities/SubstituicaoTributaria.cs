namespace Fly01.EmissaoNFE.Domain
{
    public class SubstituicaoTributaria
    {
        public string EstadoOrigem { get; set; }
        public string EstadoDestino { get; set; }
        public double Aliquota { get; set; }
        public double Base { get; set; }
        public double Valor { get; set; }
        public double Mva { get; set; }
        public int EntradaSaida { get; set; }
        public bool IpiNaBase { get; set; }
        public bool FreteNaBase { get; set; }
        public bool DespesaNaBase { get; set; }
        public double AliquotaIntraEstadual { get; set; }
        public double AliquotaInterEstadual { get; set; }
        public double? PercentualReducaoBCST { get; set; }
    }
}
