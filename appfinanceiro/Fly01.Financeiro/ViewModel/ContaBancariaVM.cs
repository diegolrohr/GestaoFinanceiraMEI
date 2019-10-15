using System;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.ViewModel
{
    [Serializable]
    public class ContaBancariaVM : EmpresaBaseVM
    {
        [JsonProperty("nomeConta")]
        public string NomeConta { get; set; }

        [JsonProperty("agencia")]
        public string Agencia { get; set; }

        [JsonProperty("digitoAgencia")]
        public string DigitoAgencia { get; set; }

        [JsonProperty("conta")]
        public string Conta { get; set; }

        [JsonProperty("digitoConta")]
        public string DigitoConta { get; set; }

        [JsonProperty("bancoId")]
        public Guid? BancoId { get; set; }

        [JsonProperty("banco")]
        public virtual BancoVM Banco { get; set; }

        [JsonProperty("valorInicial")]
        public double? ValorInicial { get; set; }
    }
}