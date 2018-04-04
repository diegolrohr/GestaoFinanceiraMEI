using Fly01.EmissaoNFE.Domain.Enums;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class CceVM : EntidadeVM
    {
        public string VersaoLayoutCCE { get; set; }
        public string VersaoLayoutEventoCCE { get; set; }
        public string VersaoEventoCCE { get; set; }
        public string VersaoCCE { get; set; }
        public string VersaoLayoutEPP { get; set; }
        public string VersaoLayoutEventoEPP { get; set; }
        public string VersaoEventoEPP { get; set; }
        public string VersaoEPP { get; set; }
        public bool HorarioDeVerao { get; set; }

        [JsonIgnore]
        public TipoAmbiente AmbienteCCE { get; set; }
        [NotMapped]
        [JsonProperty("AmbienteCCE")]
        public string TipoAmbienteCCE
        {
            get { return ((int)AmbienteCCE).ToString(); }
            set { AmbienteCCE = (TipoAmbiente)Enum.Parse(typeof(TipoAmbiente), value); }
        }

        [JsonIgnore]
        public TipoAmbiente AmbienteEPP { get; set; }
        [NotMapped]
        [JsonProperty("AmbienteEPP")]
        public string TipoAmbienteEPP
        {
            get { return ((int)AmbienteEPP).ToString(); }
            set { AmbienteEPP = (TipoAmbiente)Enum.Parse(typeof(TipoAmbiente), value); }
        }

        [JsonIgnore]
        public TipoFusoHorario FusoHorario { get; set; }
        [NotMapped]
        [JsonProperty("FusoHorario")]
        public string TipoFusoHorario
        {
            get { return ((int)FusoHorario).ToString(); }
            set { FusoHorario = (TipoFusoHorario)Enum.Parse(typeof(TipoFusoHorario), value); }
        }
    }
}
