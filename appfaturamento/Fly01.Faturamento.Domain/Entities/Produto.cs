﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fly01.Core.Entities.Domains;
using Newtonsoft.Json;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.Domain.Entities
{
    [Serializable]
    public class Produto : PlataformaBase
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Descricao { get; set; }

        public Guid? GrupoProdutoId { get; set; }

        public Guid? UnidadeMedidaId { get; set; }

        public Guid? NcmId { get; set; }

        [JsonIgnore]
        public TipoProduto TipoProduto { get; set; }

        [NotMapped]
        [JsonProperty("tipoProduto")]
        public string TipoProdutoRest
        {
            get { return ((int)TipoProduto).ToString(); }
            set { TipoProduto = (TipoProduto)System.Enum.Parse(typeof(TipoProduto), value); }
        }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]

        public double? SaldoProduto { get; set; }

        public string CodigoProduto { get; set; }

        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CodigoBarras { get; set; }

        public double ValorVenda { get; set; }

        public double ValorCusto { get; set; }

        public double SaldoMinimo { get; set; }

        [StringLength(200, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Observacao { get; set; }

        public double AliquotaIpi { get; set; }

        public Guid? CestId { get; set; }

        public Guid? EnquadramentoLegalIPIId { get; set; }

        #region Navigations Properties
        public virtual GrupoProduto GrupoProduto { get; set; }
        public virtual UnidadeMedida UnidadeMedida { get; set; }
        public virtual Ncm Ncm { get; set; }
        public virtual Cest Cest { get; set; }
        public virtual EnquadramentoLegalIPI EnquadramentoLegalIPI { get; set; }
        #endregion
    }
}