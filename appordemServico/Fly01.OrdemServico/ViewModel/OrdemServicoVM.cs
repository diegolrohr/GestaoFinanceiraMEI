using Fly01.Core.Helpers.Attribute;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.OrdemServico.ViewModel
{
    public class OrdemServicoVM : DomainBaseVM
    {
        [JsonProperty("status")]
        [APIEnum("StatusOrdemServico")]
        public string Status { get; set; }

        [JsonProperty("pessoaId")]
        public Guid PessoaId { get; set; }

        [JsonProperty("numero")]
        public int Numero { get; set; }

        [JsonProperty("dataEmissao")]
        public DateTime DataEmissao { get; set; }

        [JsonProperty("dataEntrega")]
        public DateTime DataEntrega { get; set; }

        [JsonProperty("responsavelId")]
        public Guid ResponsavelId { get; set; }

        [JsonProperty("aprovado")]
        public bool Aprovado { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("valorTotal")]
        public bool ValorTotal { get; set; }

        #region Navigation
        [JsonProperty("pessoa")]
        public virtual PessoaVM Pessoa { get; set; }

        [JsonProperty("responsavel")]
        public virtual PessoaVM Responsavel { get; set; }
        #endregion
    }
}
