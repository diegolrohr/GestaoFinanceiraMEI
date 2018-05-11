using Fly01.EmissaoNFE.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.IPI
{
    [XmlRoot(ElementName = "IPI", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class IPIPai
    {
        private IList<KeyValuePair<string, string>> ToValidate = new List<KeyValuePair<string, string>>();
        private static string ErrNotInformed = @"Os seguintes campos obrigatórios não foram informados: ";

        public void DoTheIpi(TipoAliquota tipoAliquota)
        {
            ToValidate.Add(NewKeyValuePair("CodigoST", CodigoST.ToString()));

            if (!Tributa)
            {
                IPI = new IPINT() { CodigoST = CodigoST };
            }
            else
            {
                ToValidate.Add(NewKeyValuePair("ValorIPI", ValorIPI.ToString()));

                if (tipoAliquota == TipoAliquota.AliquotaAdValorem)
                {
                    ToValidate.Add(NewKeyValuePair("CodigoEnquadramento", CodigoEnquadramento));
                    ToValidate.Add(NewKeyValuePair("ValorBaseCalculo", ValorBaseCalculo.ToString()));
                    ToValidate.Add(NewKeyValuePair("PercentualIPI", PercentualIPI.ToString()));
                    
                    IPI = new IPITrib()
                    {
                        CodigoST = CodigoST,
                        ValorBaseCalculo = ValorBaseCalculo,
                        ValorIPI = ValorIPI,
                        PercentualIPI = PercentualIPI
                    };
                }
                else
                {
                    ToValidate.Add(NewKeyValuePair("ClasseEnquadramento", ClasseEnquadramento));
                    ToValidate.Add(NewKeyValuePair("CodigoSelo", CodigoSelo));
                    ToValidate.Add(NewKeyValuePair("QtdSeloEnquadramentoUtilizado", QtdSeloEnquadramentoUtilizado));
                    ToValidate.Add(NewKeyValuePair("CodigoEnquadramento", CodigoEnquadramento));
                    ToValidate.Add(NewKeyValuePair("QtdTotalUnidadeTributavel", QtdTotalUnidadeTributavel.ToString()));
                    ToValidate.Add(NewKeyValuePair("ValorUnidadeTributavel", ValorUnidadeTributavel.ToString()));

                    IPI = new IPITrib()
                    {
                        CodigoST = CodigoST,
                        ValorIPI = ValorIPI,
                        QtdTotalUnidadeTributavel = QtdTotalUnidadeTributavel,
                        ValorUnidadeTributavel = ValorUnidadeTributavel
                    };
                }
            }

        }

        /// <summary>
        /// Informar a classe de enquadramento do IPI para Cigarros e Bebidas conforme Atos Normativos editados pela Receita Federal do Brasil.
        /// </summary>
        [XmlElement(ElementName = "clEnq")]
        public string ClasseEnquadramento { get; set; }

        /// <summary>
        /// Informar o código do Selo de Controle do IPI conforme Atos Normativos editados pela Receita Federal do Brasil.
        /// </summary>
        [XmlElement(ElementName = "cSelo")]
        public string CodigoSelo { get; set; }

        /// <summary>
        /// Informar a quantidade de Selo de Controle do IPI utilizados.
        /// </summary>
        [XmlElement(ElementName = "qSelo")]
        public string QtdSeloEnquadramentoUtilizado { get; set; }

        /// <summary>
        /// Informar o Código de Enquadramento Legal do IPI, informar 999 enquanto a tabela não tiver sido criada pela Receita Federal do Brasil.
        /// </summary>
        [XmlElement(ElementName = "cEnq")]
        public string CodigoEnquadramento { get; set; }

        /// <summary>
        /// Grupo de Tributos do Ipi
        /// </summary>
        public IPI IPI { get; set; }

        /// <summary>
        /// Informar o Código de Situação Tributária do IPI.
        /// </summary>
        [XmlIgnore]
        public CSTIPI CodigoST { get; set; }

        /// <summary>
        /// Informar a quantidade total na unidade padrão de tributação, este campo deve ser informado em caso de alíquota específica.
        /// </summary>
        [XmlIgnore]
        public double QtdTotalUnidadeTributavel { get; set; }

        /// <summary>
        /// Informar o Valor por Unidade Tributável, este campo deve ser informado em caso de alíquota específica.
        /// </summary>
        [XmlIgnore]
        public double ValorUnidadeTributavel { get; set; }

        /// <summary>
        /// Informar o Valor da BC do IPI, este campo deve ser informado em caso de alíquota ad valorem.
        /// </summary>
        [XmlIgnore]
        public double ValorBaseCalculo { get; set; }

        /// <summary>
        /// Informar a alíquota percentual do IPI, este campo deve ser informado em caso de alíquota ad valorem. 
        /// </summary>
        [XmlIgnore]
        public double PercentualIPI { get; set; }

        /// <summary>
        /// Informar o Valor do IPI
        /// </summary>
        [XmlIgnore]
        public double ValorIPI { get; set; }

        /// <summary>
        /// Deve ser informado quando preenchido o Grupo Tributos Devolvidos na emissão de nota finNFe=4 (devolução) nas operações com não contribuintes do IPI.
        /// </summary>
        [XmlIgnore]
        public double ValorIPIDevolucao { get; set; }

        /// <summary>
        /// Informar false caso não haja tributação de IPI
        /// </summary>
        [XmlIgnore]
        public bool Tributa { get; set; } = true;

        private void DoTheValidation()
        {
            if (ToValidate.Any(e =>
            {
                return string.IsNullOrEmpty(e.Value);
            }))
            {
                ThrowErrorNotInformed();
            }
        }

        public KeyValuePair<string, string> NewKeyValuePair(string key, string value)
        {
            return new KeyValuePair<string, string>(key, value);
        }

        private void ThrowErrorNotInformed()
        {
            StringBuilder sb = new StringBuilder(ErrNotInformed);

            foreach (var field in ToValidate)
            {
                sb.Append(string.Format("{0},", field.Key));
            }

            sb.Length--;

            throw new Exception(sb.ToString());
        }

    }
}