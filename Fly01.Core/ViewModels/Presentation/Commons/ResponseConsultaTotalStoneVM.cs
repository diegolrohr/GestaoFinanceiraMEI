using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ResponseConsultaTotalStoneVM
    {
        [JsonProperty("saldoDevedor")]
        public double SaldoDevedor { get; set; }

        [JsonProperty("totalBrutoAntecipavel")]
        public double TotalBrutoAntecipavel { get; set; }

        [JsonProperty("totalAntecipavel")]
        public string TotalAntecipavel
        {
            get
            {
                return TotalBrutoAntecipavel.ToString("C", AppDefaults.CultureInfoDefault);
            }
            set
            { }
        }
    }
}