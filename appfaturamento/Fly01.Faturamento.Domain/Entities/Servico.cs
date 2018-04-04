using System;
using Newtonsoft.Json;
using Fly01.Core.Api.Domain;
using Fly01.Faturamento.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Faturamento.Domain.Entities
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

        //[JsonIgnore]
        //public TipoServico TipoServico { get; set; }

        //[NotMapped]
        //[JsonProperty("tipoServico")]
        //public string TipoServicoRest
        //{
        //    get { return ((int)TipoServico).ToString(); }
        //    set { TipoServico = (TipoServico)Enum.Parse(typeof(TipoServico), value); }
        //}

        [JsonIgnore]
        public TipoTributacaoISS? TipoTributacaoISS { get; set; }

        [NotMapped]
        [JsonProperty("tipoTributacaoISS")]
        public string TipoTributacaoISSRest
        {
            get { return ((int)TipoTributacaoISS).ToString(); }
            set { TipoTributacaoISS = (TipoTributacaoISS)Enum.Parse(typeof(TipoTributacaoISS), value); }
        }

        [JsonIgnore]
        public TipoPagamentoImpostoISS? TipoPagamentoImpostoISS { get; set; }

        [NotMapped]
        [JsonProperty("tipoPagamentoImpostoISS")]
        public string TipoPagamentoImpostoISSRest
        {
            get { return ((int)TipoPagamentoImpostoISS).ToString(); }
            set { TipoPagamentoImpostoISS = (TipoPagamentoImpostoISS)Enum.Parse(typeof(TipoPagamentoImpostoISS), value); }
        }

        #region Navigations Properties
        public virtual NBS Nbs { get; set; }
        #endregion
    }
}
