﻿using Fly01.EmissaoNFE.Domain.Enums;
using Newtonsoft.Json;
using System;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class ParametroVM : EntidadeVM
    {   
        public bool IncentivadorCultural { get; set; }

        public bool SimplesNacional { get; set; }

        public string VersaoNFSe { get; set; }

        public string VersaoDPEC { get; set; }

        public string NumeroRetornoNF { get; set; }

        public bool EnviaDanfe { get; set; }

        public bool UsaEPEC { get; set; }
        
        public string VersaoNFe { get; set; }
        
        [JsonIgnore]
        public TipoAmbiente Ambiente { get; set; }

        [JsonProperty("Ambiente")]
        public string TipoAmbiente
        {
            get { return ((int)Ambiente).ToString(); }
            set { Ambiente = (TipoAmbiente)Enum.Parse(typeof(TipoAmbiente), value); }
        }

        [JsonIgnore]
        public TipoModalidade Modalidade { get; set; }
        
        [JsonProperty("Modalidade")]
        public string TipoModalidade
        {
            get { return ((int)Modalidade).ToString(); }
            set { Modalidade = (TipoModalidade)Enum.Parse(typeof(TipoModalidade), value); }
        }

        [JsonIgnore]
        public TipoRegimeTributario RegimeTributario { get; set; }
        
        [JsonProperty("RegimeTributario")]
        public string TipoRegimeTributario
        {
            get { return ((int)RegimeTributario).ToString(); }
            set { RegimeTributario = (TipoRegimeTributario)Enum.Parse(typeof(TipoRegimeTributario), value); }
        }
    }
}
