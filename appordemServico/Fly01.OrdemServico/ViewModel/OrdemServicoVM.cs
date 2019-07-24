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

        [JsonProperty("clienteId")]
        public Guid ClienteId { get; set; }

        [JsonProperty("numero")]
        public int Numero { get; set; }

        [JsonProperty("dataEmissao")]
        public DateTime DataEmissao { get; set; }

        [JsonProperty("dataEntrega")]
        public DateTime DataEntrega { get; set; }

        [JsonProperty("horaEntrega")]
        public TimeSpan HoraEntrega { get; set; }

        [JsonProperty("duracao")]
        public string Duracao { get; set; }
        //public string Duracao { get { return this.Duracao == "00:00:00.0000000" ? TimeSpan.FromMinutes(this.Tempo.GetValueOrDefault()).ToString() : this.Duracao; } set { this.Duracao = value; } }
        [JsonProperty("tempo")]
        public double? Tempo { get; set; }

        [JsonProperty("responsavelId")]
        public Guid? ResponsavelId { get; set; }

        [JsonProperty("aprovado")]
        public bool Aprovado { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("geraOrdemVenda")]
        public bool GeraOrdemVenda { get; set; }

        #region Navigation
        [JsonProperty("cliente")]
        public virtual PessoaVM Cliente { get; set; }

        [JsonProperty("responsavel")]
        public virtual PessoaVM Responsavel { get; set; }
        #endregion
    }
}
