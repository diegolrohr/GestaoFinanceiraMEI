﻿using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot(ElementName = "detPag")]
    public class DetalhePagamento
    {
        /// <summary>
        /// Informar o Meio de pagamento:
        /// 01=Dinheiro
        /// 02=Cheque
        /// 03=Cartão de Crédito
        /// 04=Cartão de Débito
        /// 05=Crédito Loja
        /// 10=Vale Alimentação
        /// 11=Vale Refeição
        /// 12=Vale Presente
        /// 13=Vale Combustível
        /// 15=Boleto Bancário
        /// 90=Sem Pagamento;
        /// 99=Outros.
        /// </summary>
        /// //TODO: gerar migration formaPagamento em cada app
        /// 5 trans para 6
        /// 6 boleto para 15
        [XmlElement(ElementName = "tPag")]
        public TipoFormaPagamento TipoFormaPagamento { get; set; }

        /// <summary>
        /// Informar o valor do Pagamento
        /// </summary>
        [XmlIgnore]
        public double ValorPagamento { get; set; }

        [XmlElement(ElementName = "vPag")]
        public string ValorPagamentoString
        {
            get { return ValorPagamento.ToString("0.00").Replace(",", "."); }
            set { ValorPagamento = double.Parse(value.Replace(".", ",")); }
        }
    }
}