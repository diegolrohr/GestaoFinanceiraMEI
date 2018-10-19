using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    /// <summary>
    /// TDN XMl Único TSS http://tdn.totvs.com.br/pages/viewpage.action?pageId=243641191
    /// </summary>
    [XmlRoot(ElementName = "rps")]
    public class Identificacao
    {
        [XmlIgnore]
        public string CodigoIBGEPrestador { get; set; }

        [XmlIgnore]
        public DateTime DataHoraEmissao { get; set; }

        [XmlElement(ElementName = "dthremissao")]
        public string DataHoraEmissaoString
        {
            get { return DataHoraEmissao.ToString("yyyy-MM-ddTHH:mm:sszzzz"); }
            set { DataHoraEmissao = DateTime.Parse(value); }
        }

        [XmlElement(ElementName = "serierps")]
        public string SerieRPS { get; set; }

        [XmlElement(ElementName = "numerorps")]
        public int NumeroRPS { get; set; }

        /// <summary>
        /// O tipo é fixo como RPS (1)
        /// 1 – RPS, 2 – Nota Fiscal Conjugada(Mista), 3 – Cupom., 4 - NFTS
        /// </summary>
        [XmlElement(ElementName = "tipo")] //TipoNFs
        public TipoSimNao TipoNFs
        {
            get
            {
                return TipoSimNao.Sim;
            }
            set { }
        }

        /// <summary>
        /// O tipo é fixo, pois tanto ABRASF como DSFNET, utilizam esta tag como Normal (1)
        /// 1 – Normal; 2 – Cancelado; 3 – Extraviado; 4 – Lote
        /// </summary>
        [XmlElement(ElementName = "situacaorps")]
        public TipoSimNao TipoSituacaoRPS
        {
            get
            {
                return TipoSimNao.Sim;
            }
            set { }
        }

        /// <summary>
        /// O tipo é fixo pois sempre recolhe ISS
        /// 1 – A receber; 2 – Retido na fonte.
        /// </summary>
        [XmlElement(ElementName = "tiporecolhe")]
        public TipoSimNao TipoRecolhimento
        {
            get
            {
                return TipoSimNao.Sim;
            }
            set { }
        }


        /// <summary>
        /// O tipo é fixo 1
        /// 1 – Sem dedução; 2 – Com dedução/Materiais; 3 – Imune/Isenta de ISSQN; 4 – Devolução/Simples remessa; 5 – Intermediação.
        /// </summary>
        [XmlElement(ElementName = "tipooper")]
        public TipoSimNao TipoOperacao
        {
            get
            {
                return TipoSimNao.Sim;
            }
            set { }
        }

        /// <summary>//TODO: Ver Machado se vamos usar as outras possibilidades
        /// 1 – Isenta de ISS; 2 – Não incidência no município; 3 – Imune; 4 – Exigibilidade Susp.Dec.J. 5 – Não tributável;
        ///6 – Tributável; 7 – Tributável fixo; 8 – Tributável S.N; 9 – Cancelado; 10 – Extraviado. 11 – Micro Empreendedor Individual(MEI)
        ///12 – Exigibilidade Susp.Proc.A. 13 –  Sem recolhimento 14 –  Devido a outro município
        /// </summary>
        [XmlElement(ElementName = "tipotrib")]
        public TipoTributacaoNFS TipoTributacao { get; set; }

        /// <summary>
        /// Tipo de tributação do local onde será tributado o serviço:
        /// 1 – Dentro do Município; 2 – Fora do Município.
        /// </summary>
        //[XmlElement(ElementName = "localServ")] Disponível xml único, não utilizado ainda
        [XmlIgnore]
        public TipoSimNao LocalTributacaoServico { get; set; }

        /// <summary>
        /// 1 true 2 false
        /// Regime especial de tributação do documento.
        /// 0 – Tributação Normal
        /// 1 – Microempresa Municipal(ME);
        /// 2 – Estimativa;
        /// 3 – Sociedade de Profissionais;
        /// 4 – Cooperativa;
        /// 5 – Microempresário Individual(MEI);
        /// 6 – Microempresário e Empresa de Pequeno Porte(ME EPP).
        /// 7 - Movimento Mensal/ISS/Fixo Autônomo;
        /// 8 - Sociedade Limitada/Média Empresa;
        /// 9 - Sociedade Anônima/Grande Empresa;
        /// 11 - Empresa Individual;
        /// 10 - Empresa Individual de Responsabilidade Limitada(EIRELI);
        /// 12 - Empresa de Pequeno Porte(EPP);
        /// 13 - Microempresário;
        /// 14 - Outros/Sem Vínculos;
        /// 50 - Nenhum;
        /// 51 - Nota Avulsa.
        /// Algumas descrições e/ou tipos de são específicos para o município de Nova Friburgo - RJ.
        /// </summary>
        [XmlElement(ElementName = "regimeesptrib")]
        public string TipoRegimeEspecialTributacao
        {
            get
            {
                return TipoTributacao == TipoTributacaoNFS.DentroMunicipio ? "1" : "2";
            }
            set { }
        }

        /// <summary>
        /// Forma de pagamento do documento. Tamanho 100
        /// </summary>
        //[XmlElement(ElementName = "formpagto")] Disponível xml único, não utilizado ainda
        [XmlIgnore]
        public string FormaPagamentoDocumento { get; set; }

        /// <summary>
        /// Informar se o ISS é devido no município do Prestador.
        /// 1 - Sim, devido no Prestador. 2 - Não,devido no Tomador.
        /// O tipo é fixo pois sempre deve ISS ao municipio
        /// </summary>
        //TODO: Rever com SP, colocar em parametros?
        [XmlElement(ElementName = "deveissmunprestador")]
        public TipoSimNao DeveISSMunicipioPrestador
        {
            get
            {
                return TipoSimNao.Sim;
            }
            set { }
        }

        /// <summary>
        /// Tag especifica para cidade Vitória - Espirito Santo
        /// </summary>
        [XmlIgnore]
        public DateTime CompetenciaRPS { get; set; }

        [XmlElement(ElementName = "competenciarps")]
        public string CompetenciaRPSString
        {
            get { return CompetenciaRPS.ToString("yyyy-MM-dd"); }
            set { CompetenciaRPS = DateTime.Parse(value); }
        }

        public bool ShouldSerializeCompetenciaRPSString()
        {
            return (!string.IsNullOrEmpty(CodigoIBGEPrestador) && CodigoIBGEPrestador.ToUpper() == "3205309");
        }

        /// <summary>
        /// Código de validação da prénota. UsaAidfRps
        /// </summary>
        //[XmlElement(ElementName = "codverificacao")]
        [XmlIgnore]//TODO: Ver com SP AIDF e ver sobre tag Substituicao
        public DateTime CodigoVerificacao { get; set; }
    }
}