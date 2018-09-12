using Fly01.Core.Entities.Domains.Enum;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [Serializable]
    public class Servico : PlataformaBase
    {
        public string CodigoServico { get; set; }

        public string Descricao { get; set; }

        public Guid? NbsId { get; set; }

        public double ValorServico { get; set; }

        [StringLength(200, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Observacao { get; set; }

        [JsonIgnore]
        public TipoTributacaoISS? TipoTributacaoISS { get; set; }

        [NotMapped]
        [JsonProperty("tipoTributacaoISS")]
        public string TipoTributacaoISSRest
        {
            get { return TipoTributacaoISS.HasValue ? ((int)TipoTributacaoISS).ToString() : null; }
            set { TipoTributacaoISS = (TipoTributacaoISS)System.Enum.Parse(typeof(TipoTributacaoISS), value); }
        }

        [JsonIgnore]
        public TipoPagamentoImpostoISS? TipoPagamentoImpostoISS { get; set; }

        [NotMapped]
        [JsonProperty("tipoPagamentoImpostoISS")]
        public string TipoPagamentoImpostoISSRest
        {
            get { return TipoPagamentoImpostoISS.HasValue ? ((int)TipoPagamentoImpostoISS).ToString() : null; }
            set { TipoPagamentoImpostoISS = (TipoPagamentoImpostoISS)System.Enum.Parse(typeof(TipoPagamentoImpostoISS), value); }
        }

        public virtual Nbs Nbs { get; set; }
    }
}
