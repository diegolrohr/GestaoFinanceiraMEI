using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class Detalhe
    {
        /// <summary>
        /// informar o número do item do detalhe, deve ser um valor único crescente compreendido na faixa de 1 a 990.
        /// </summary>
        /// 
        [JsonProperty("nItem")]
        [XmlAttribute("nItem")]
        public int NumeroItem { get; set; }

        /// <summary>
        /// informar o grupo XML prod com o detalhamento do produto/serviço do item.
        /// </summary>
        /// 
        [JsonProperty("prod")]
        [XmlElement("prod")]
        public Produto Produto { get; set; }

        /// <summary>
        /// informar o grupo XML imposto com as informações dos tributos incidentes no item.
        /// </summary>
        /// 
        [JsonProperty("imposto")]
        [XmlElement("imposto")]
        public Imposto Imposto { get; set; }

        /// <summary>
        /// pode ser utilizado para complementar a descrição e informações adicionais do produto. Não é permitido informação de caracteres de formatação(CR, LF, TAB, etc.), assim o usuário pode colocar caracteres que identificam o final linha para melhorar a visualização e a aplicação de impressão do DANFE tratar como quebra de linha, ex.: ***, /, |, etc. A consulta web da NF-e ainda não está mostrando as informações adicionais do produto, necessário reportar o problema para a SEFAZ resolver.
        /// </summary>
        /// 
        [JsonProperty("infAdprod")]
        [XmlElement(ElementName = "infAdprod", IsNullable = true)]
        public string InformacoesAdicionais { get; set; }

        public bool ShouldSerializeInformacoesAdicionais()
        {
            return InformacoesAdicionais != null && InformacoesAdicionais.Length > 0;
        }
    }
}
