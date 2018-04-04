namespace Fly01.EmissaoNFE.Domain
{
    public class Ipi
    {
        public double Aliquota { get; set; }
        public double Base { get; set; }
        public double Valor { get; set; }
        public string Ncm { get; set; }
        public bool AliquotaPeloNcm { get; set; }
        public bool FreteNaBase { get; set; }
        public bool DespesaNaBase { get; set; }
    }
}
