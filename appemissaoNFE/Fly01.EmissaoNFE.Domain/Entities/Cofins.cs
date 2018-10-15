namespace Fly01.EmissaoNFE.Domain
{
    public class Cofins
    {
        public double Aliquota { get; set; }
        public bool FreteNaBase { get; set; }
        public bool RetemCofins { get; set; }
        public bool CalculaCofins { get; set; }
    }
}