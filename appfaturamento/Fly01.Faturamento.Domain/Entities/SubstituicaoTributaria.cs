using Fly01.Faturamento.Domain.Enums;
using Fly01.Core.Entities.Domains;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Faturamento.Domain.Entities
{
    public class SubstituicaoTributaria : PlataformaBase
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid NcmId { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid EstadoOrigemId { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid EstadoDestinoId { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public double Mva { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public TipoSubstituicaoTributaria TipoSubstituicaoTributaria { get; set; }

        [NotMapped]
        [JsonProperty("tipoSubstituicaoTributaria")]
        public string TipoSubstituicaoTributariaRest
        {
            get { return ((int)TipoSubstituicaoTributaria).ToString(); }
            set { TipoSubstituicaoTributaria = (TipoSubstituicaoTributaria)Enum.Parse(typeof(TipoSubstituicaoTributaria), value); }
        }

        public Guid? CestId { get; set; }

        #region NavigationProperties

        public virtual NCM Ncm { get; set; }
        public virtual Estado EstadoOrigem { get; set; }
        public virtual Estado EstadoDestino { get; set; }
        public virtual Cest Cest { get; set; }

        #endregion
    }
}
