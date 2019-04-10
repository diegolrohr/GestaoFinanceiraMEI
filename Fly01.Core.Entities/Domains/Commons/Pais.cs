using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    //https://ww2.ibge.gov.br/home/estatistica/populacao/registrocivil/2013/codigo_paises.shtm
    //https://www.bcb.gov.br/acessoinformacao/legado?url=https:%2F%2Fwww.bcb.gov.br%2Frex%2Fcenso2000%2Fport%2Fmanual%2Fpais.asp%3Fidpai%3Dcenso2000inf
    //var string = ""; $('td:first-child').each(function() { string += '"' +$(this).text() + '"' + ',' + '"' + $(this).next().text() + '"' + ';'}); console.log(string);
    public class Pais : DomainBase
    {
        [Required]
        [StringLength(35)]
        public string Nome { get; set; }

        [Required]
        [StringLength(3)]
        public string CodigoIbge { get; set; }

        [Required]
        [StringLength(4)]
        public string CodigoBacen { get; set; }
    }
}
