using Fly01.Core.VM;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class ConfiguracaoNotificacaoVM : DomainBaseVM
    {
        [JsonProperty("notificaViaEmail")]
        public bool NotificaViaEmail { get; set; }

        [JsonProperty("notificaViaSMS")]
        public bool NotificaViaSMS { get; set; }

        [JsonIgnore]
        public DayOfWeek DiaSemana { get; set; }

        //[APIEnum("diaSemana")]
        //[JsonProperty("diaSemana")]
        //public string DiaSemanaRest { get; set; }

        //[APIEnum("diaSemana")]
        [JsonProperty("diaSemana")]
        public string DiaSemanaRest
        {
            get { return ((int)DiaSemana).ToString(); }
            set { DiaSemana = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), value, true); }
            //set { DiaSemana = (DayOfWeek)Enum.ToObject(typeof(DayOfWeek), (int)DiaSemana + 1); }
        }

        [JsonProperty("horaEnvio")]
        public TimeSpan HoraEnvio { get; set; }

        [JsonProperty("contasAPagar")]
        public bool ContasAPagar { get; set; }

        [JsonProperty("contasAReceber")]
        public bool ContasAReceber { get; set; }

        [Display(Name = "E-mail")]
        [StringLength(70, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        [JsonProperty("emailDestino")]
        public string EmailDestino { get; set; }

        [StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        [JsonProperty("contatoDestino")]
        public string ContatoDestino { get; set; }

    }
}