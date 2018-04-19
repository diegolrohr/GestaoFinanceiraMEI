using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Entities.Domains.Enum;

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
            get { return ((int)TipoTributacaoISS).ToString(); }
            set { TipoTributacaoISS = (TipoTributacaoISS)System.Enum.Parse(typeof(TipoTributacaoISS), value); }
        }

        [JsonIgnore]
        public TipoPagamentoImpostoISS? TipoPagamentoImpostoISS { get; set; }

        [NotMapped]
        [JsonProperty("tipoPagamentoImpostoISS")]
        public string TipoPagamentoImpostoISSRest
        {
            get { return ((int)TipoPagamentoImpostoISS).ToString(); }
            set { TipoPagamentoImpostoISS = (TipoPagamentoImpostoISS)System.Enum.Parse(typeof(TipoPagamentoImpostoISS), value); }
        }

        public virtual Nbs Nbs { get; set; }
    }
}
