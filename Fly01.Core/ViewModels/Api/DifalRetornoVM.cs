namespace Fly01.Core.ViewModels.Api

{
    public class DifalRetornoVM
    {
        public double AliquotaIntraestadual { get; set; }
        public double AliquotaInterestadual { get; set; }
        public double DiferencialDeAliquota { get; set; }
        public double ValorDifalOrigem { get; set; }
        public double ValorDifalDestino { get; set; }
        public bool AgregaTotalNota { get; set; }
    }
}
