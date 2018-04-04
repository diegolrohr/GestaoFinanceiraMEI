using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fly01.Utils.Api.Domain;
using Fly01.Financeiro.Domain.Enums;

namespace Fly01.Financeiro.Domain.Entities
{
    public class CategoriaFinanceira : PlataformaBase
    {
        [JsonProperty("descricao")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Descricao { get; set; }

        [JsonIgnore]
        [Display(Name = "Classe")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public CategoriaFinanceiraClasse Classe { get; set; }

        [JsonProperty("categoriaPaiId")]
        [Display(Name = "Cat. Superior")]
        public Guid? CategoriaPaiId { get; set; }

        [JsonIgnore]
        [Display(Name = "Tipo Carteira")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public TipoCarteira TipoCarteira { get; set; }

        [NotMapped]
        [JsonProperty("tipoCarteira")]
        public string TipoCarteiraRest
        {
            get { return ((int)TipoCarteira).ToString(); }
            set { TipoCarteira = (TipoCarteira)Enum.Parse(typeof(TipoCarteira), value); }
        }

        [NotMapped]
        [JsonProperty("classe")]
        public string ClasseRest
        {
            get { return ((int)Classe).ToString(); }
            set { Classe = (CategoriaFinanceiraClasse)Enum.Parse(typeof(CategoriaFinanceiraClasse), value); }
        }

        [JsonIgnore]
        public virtual CategoriaFinanceira CategoriaPai { get; set; }

        [JsonProperty("codigo")]
        [Display(Name = "Código")]
        public string Codigo { get; set; }
    }
}