using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class Identificador
    {
        [Required]
        /// <summary>
        /// informar o código da UF do emitente do Documento Fiscal, utilizar a codificação do IBGE (Ex. SP->35, RS->43, etc.)
        /// </summary>
        [XmlElement(ElementName = "cUF")]
        public int CodigoUF { get; set; }

        [Required]
        /// <summary>
        /// informar o código numérico que compõe a Chave de Acesso. Número aleatório gerado pelo emitente para cada NF-e para evitar acessos indevidos da NF-e.
        /// </summary>
        [XmlElement(ElementName = "cNF")]
        public string CodigoNF { get; set; }

        [Required]
        /// <summary>
        /// informar a natureza da operação de que decorrer a saída ou a entrada, tais como: venda, compra, transferência, devolução, importação, consignação, 
        /// remessa (para fins de demonstração, de industrialização outra), conforme previsto na alínea 'i', inciso I, art. 19 do CONVÊNIO S/Nº, de 15 de dezembro de 1970.
        /// </summary>
        [XmlElement(ElementName = "natOp")]
        public string NaturezaOperacao { get; set; }

        /// <summary>
        /// informar o código do Modelo do Documento Fiscal, código 55 para a NF-e ou código 65 para NFC-e.
        /// </summary>
        [Required]
        [XmlElement(ElementName = "mod")]
        public int ModeloDocumentoFiscal { get; set; }

        [Required]
        /// <summary>
        /// informar a série do Documento Fiscal, informar 0 (zero) para série única. A emissão normal pode utilizar série de 0-889, a emissão em contingência 
        /// SCAN deve utilizar série 900-999.
        /// </summary>
        [XmlElement(ElementName = "serie")]
        public int Serie { get; set; }

        [Required]
        /// <summary>
        /// informar o Número do Documento Fiscal.
        /// </summary>
        [XmlElement(ElementName = "nNF")]
        public int NumeroDocumentoFiscal { get; set; }

        #region dateTime

        /// <summary>
        /// informar a data de emissão do Documento Fiscal
        /// Nota 1: No caso da NF-e, a informação da Hora de Emissão é opcional, podendo ser informada com zeros.
        /// Nota 2: A emissão da NFC-e deve ocorrer de forma on-line, realtime, com uma tolerância de até 5 minutos, devido ao sincronismo de horário do servidor da Empresa e 
        /// o servidor da SEFAZ.
        /// Nota 3: A tolerância acima motivada pelo horário dos servidores, somada ao atraso permitido para a autorização da NFC-e acaba resultando em um atraso máximo de 10 
        /// minutos a ser controlado pela aplicação da SEFAZ.
        /// </summary>
        [XmlIgnore]
        public DateTime Emissao { get; set; }
        [XmlElement("dhEmi")]
        public string EmissaoString
        {
            get { return Emissao.ToString("yyyy-MM-ddTHH:mm:sszzzz"); }
            set { Emissao = DateTime.Parse(value); }
        }

        /// <summary>
        /// informar a data e hora de Saída ou da Entrada da Mercadoria/Produto
        /// Nota: Para a NFC-e este campo não deve existir.
        /// </summary>
        [XmlIgnore]
        public DateTime EntradaSaida  { get; set; }
        
        [XmlElement("dhSaiEnt")]
        public string EntradaSaidaString
        {
            get { return EntradaSaida.ToString("yyyy-MM-ddTHH:mm:sszzzz"); }
            set { EntradaSaida = DateTime.Parse(value); }
        }

        #endregion dateTime

        [Required]
        /// <summary>
        /// informar o código do tipo do Documento Fiscal:
        /// 0 - entrada;
        /// 1 - saída.
        /// </summary>
        [XmlElement(ElementName = "tpNF")]
        public TipoNota TipoDocumentoFiscal { get; set; }

        /// <summary>
        /// informar o identificador de local de destino da operação:
        /// 1 - Operação interna;
        /// 2 - Operação interestadual;
        /// 3 - Operação com exterior
        /// </summary>
        [Required]
        [XmlElement(ElementName = "idDest")]
        public TipoDestinoOperacao DestinoOperacao { get; set; }

        /// <summary>
        /// informar o código do Município de Ocorrência do Fato Gerador do ICMS, que é o local onde ocorre a entrada ou saída da mercadoria, utilizar a Tabela do IBGE
        /// </summary>
        [Required]
        [StringLength(7, ErrorMessage = "Código IBGE inválido")]
        [XmlElement(ElementName = "cMunFG")]
        public string CodigoMunicipio { get; set; }

        [Required]
        /// <summary>
        /// informar o formato de impressão do DANFE: 
        /// 0 - Sem geração de DANFE;
        /// 1 - DANFE normal, Retrato;
        /// 2 - DANFE normal, Paisagem;
        /// 3 - DANFE Simplificado;
        /// 4 - DANFE NFC-e;
        /// 5 - DANFE NFC-e em mensagem eletrônica(o envio de mensagem eletrônica pode ser feita de forma simultânea com a impressão do DANFE; 
        /// usar o tpImp - 5 quando esta for a única forma de disponibilização do DANFE).
        /// </summary>
        [XmlElement(ElementName = "tpImp")]
        public TipoImpressaoDanfe ImpressaoDANFE { get; set; }

        [Required]
        /// <summary>
        /// informar o código da forma de emissão:
        /// 1 - Emissão normal(não em contingência);
        /// 2 - Contingência FS-IA, com impressão do DANFE em formulário de segurança;
        /// 4 - Contingência DPEC(Declaração Prévia da Emissão em Contingência);
        /// 5 - Contingência FS-DA, com impressão do DANFE em formulário de segurança;
        /// 6 - Contingência SVC-AN(SEFAZ Virtual de Contingência do AN);
        /// 7 - Contingência SVC-RS(SEFAZ Virtual de Contingência do RS);
        /// 9 - Contingência off-line da NFC-e(as demais opções de contingência são válidas também para a NFC-e);
        /// Nota 1: Para a NFC-e somente estão disponíveis e são válidas as opções de contingência 5 e 9.
        /// Nota 2: SVC-AN e SVC-RS substituem o SCAN - NT 2013/007.
        /// </summary>
        [XmlElement(ElementName = "tpEmis")]
        public TipoModalidade FormaEmissao { get; set; }

        /// <summary>
        /// informar o código do dígito verificador - DV da Chave de Acesso da NF-e, o DV será calculado com a aplicação do algoritmo 
        /// módulo 11 (base 2,9) da Chave de Acesso.
        /// </summary>
        [Required]
        [XmlElement(ElementName = "cDV")]
        public int ChaveAcessoDV { get; set; }

        [Required]
        /// <summary>
        /// informar o código de identificação do Ambiente:
        /// 1 - Produção;
        /// 2 - Homologação
        /// </summary>
        [XmlElement(ElementName = "tpAmb")]
        public TipoAmbiente Ambiente { get; set; }

        /// <summary>
        /// informar o código da finalidade de emissão da NF-e: 
        /// 1 - NF-e normal;
        /// 2 - NF-e complementar;
        /// 3 - NF-e de ajuste;
        /// 4 - Devolução
        /// </summary>
        [Required]
        [XmlElement(ElementName = "finNFe")]
        public TipoFinalidadeEmissaoNFe FinalidadeEmissaoNFe { get; set; }

        /// <summary>
        /// informar o indicador de operação com Consumidor final:
        /// 0 - Não;
        /// 1 - Consumidor final;
        /// </summary>
        [Required]
        [XmlElement(ElementName = "indFinal")]
        public int ConsumidorFinal { get; set; }

        [Required]
        /// <summary>
        /// Informar o indicador de presença do comprador no estabelecimento comercial no momento da operação: 
        /// 0 - Não se aplica(por exemplo, Nota Fiscal complementar ou de ajuste);
        /// 1 - Operação presencial;
        /// 2 - Operação não presencial, pela Internet;
        /// 3 - Operação não presencial, Teleatendimento;
        /// 4 - NFC-e em operação com entrega a domicílio;
        /// 9 - Operação não presencial, outros.
        /// </summary>
        [XmlElement(ElementName = "indPres")]
        public TipoPresencaComprador PresencaComprador { get; set; }

        /// <summary>
        /// informar o código de identificação do processo de emissão da NF-e: Identificador do processo de emissão da NF-e:
        /// 0 - emissão de NF-e com aplicativo do contribuinte;
        /// 1 - emissão de NF-e avulsa pelo Fisco;
        /// 2 - emissão de NF-e avulsa, pelo contribuinte com seu certificado digital, através do site do Fisco;
        /// 3 - emissão NF-e pelo contribuinte com aplicativo fornecido pelo Fisco.
        /// </summary>
        [Required]
        [XmlElement(ElementName = "procEmi")]
        public int CodigoProcessoEmissaoNFe { get; set; }

        [Required]
        /// <summary>
        /// informar a versão do processo de emissão da NF-e utilizada (aplicativo emissor de NF-e).
        /// </summary>
        [XmlElement(ElementName = "verProc")]
        public string Versao { get; set; }

        /// <summary>
        /// Informar quando a legislação exigir a referência de uma NF-e, como é o caso de uma NF-e complementar, NF-e de devolução, NF-e de retorno, etc.
        /// </summary>
        [XmlElement(ElementName = "NFref")]
        public NFReferenciada NFReferenciada { get; set; }

        public bool ShouldSerializeNFReferenciada()
        {
            return FinalidadeEmissaoNFe == TipoFinalidadeEmissaoNFe.Devolucao && NFReferenciada != null && !String.IsNullOrEmpty(NFReferenciada.ChaveNFeReferenciada);
        }
    }
}
