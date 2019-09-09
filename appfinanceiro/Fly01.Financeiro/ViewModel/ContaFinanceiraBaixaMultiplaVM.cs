using System;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Fly01.Financeiro.ViewModel
{
    [Serializable]
    public class ContaFinanceiraBaixaMultiplaVM : EmpresaBaseVM
    {
        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonIgnore]
        public string ContasFinanceirasGuids { get; set; }

        [JsonProperty("contasFinanceirasIds")]
        public List<Guid> ContasFinanceirasIds
        {
            get
            {
                var result = new List<Guid>();
                if (!string.IsNullOrWhiteSpace(ContasFinanceirasGuids))
                {
                    ContasFinanceirasGuids.Split(',').ToList().ForEach(x =>
                    {
                        result.Add(Guid.Parse(x));
                    });
                }
                return result;
            }
        }

        [JsonProperty("contaBancariaId")]
        public Guid ContaBancariaId { get; set; }
        
        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("tipoContaFinanceira")]
        public string TipoContaFinanceira { get; set; }

        [JsonProperty("contaBancaria")]
        public virtual ContaBancariaVM ContaBancaria { get; set; }
    }
}