using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.EmissaoNFE.Domain
{
    public class Icms
    {
        public string EstadoOrigem { get; set; }
        public string EstadoDestino { get; set; }
        public double? Aliquota { get; set; }
        public double? PercentualReducaoBC { get; set; }
        public double Base { get; set; }
        public double Valor { get; set; }
        public bool IpiNaBase { get; set; }
        public bool FreteNaBase { get; set; }
        public bool DespesaNaBase { get; set; }
        public bool Difal { get; set; }
        public TipoTributacaoICMS CSOSN { get; set; }
    }
}