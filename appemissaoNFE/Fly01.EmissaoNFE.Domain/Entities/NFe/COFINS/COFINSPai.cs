using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS
{

    public class COFINSPai
    {
        private IList<KeyValuePair<string, double?>> ToValidate = new List<KeyValuePair<string, double?>>();
        private static string ErrNotInformed = @"Os seguintes campos obrigatórios não foram informados: ";

        public COFINSPai() { }

        public void DoTheCofins(TipoCRT crt)
        {
            if (crt.Equals(TipoCRT.SimplesNacional | TipoCRT.RegimeNormal | TipoCRT.ExcessoSublimiteDeReceitaBruta))
            {
                var adValorem = "01|02";
                var AliqEspecifica = "03";
                var NT = "04|05|06|07|08|09";

                if (adValorem.Contains(((int)CodigoSituacaoTributaria).ToString()))
                {
                    COFINS = new COFINSAliq("0" + ((int)CodigoSituacaoTributaria).ToString(), TipoCRT)
                    {
                        ValorBC = ValorBC,
                        AliquotaPercentual = AliquotaPercentual,
                        ValorCOFINS = ValorCOFINS
                    };
                }
                else if (AliqEspecifica.Contains(((int)CodigoSituacaoTributaria).ToString()))
                {
                    COFINS = new COFINSQtde("0" + ((int)CodigoSituacaoTributaria).ToString(), TipoCRT)
                    {
                        QuantidadeVendida = QuantidadeVendida,
                        ValorAliquota = ValorAliquota,
                        ValorCOFINS = ValorCOFINS
                    };
                }
                else if (NT.Contains(((int)CodigoSituacaoTributaria).ToString()))
                {
                    COFINS = new COFINSNT("0" + ((int)CodigoSituacaoTributaria).ToString(), TipoCRT) { };
                }
                else
                {
                    ToValidate.Add(NewKeyValuePair("ValorAliquota", ValorAliquota));
                    ToValidate.Add(NewKeyValuePair("ValorCOFINS", ValorCOFINS));

                    DoTheValidation();

                    COFINS = new COFINSOutr(((int)CodigoSituacaoTributaria).ToString(), TipoCRT)
                    {
                        QuantidadeVendida = QuantidadeVendida,
                        ValorAliquota = ValorAliquota,
                        ValorCOFINS = ValorCOFINS
                    };
                }
            }
            else
                throw new NotImplementedException();
        }
        
        public COFINS COFINS { get; set; }

        [XmlIgnore]
        public CSTPISCOFINS CodigoSituacaoTributaria { get; set; }

        [XmlIgnore]
        public double ValorCOFINS { get; set; }

        [XmlIgnore]
        public double ValorBC { get; set; }

        [XmlIgnore]
        public double AliquotaPercentual { get; set; }

        [XmlIgnore]
        public double QuantidadeVendida { get; set; }

        [XmlIgnore]
        public double ValorAliquota { get; set; }

        [XmlIgnore]
        public TipoCRT TipoCRT { get; set; }


        private void DoTheValidation()
        {
            if (ToValidate.Any(e =>
            {
                return !e.Value.HasValue;
            }))
            {
                ThrowErrorNotInformed();
            }
        }

        public KeyValuePair<string, double?> NewKeyValuePair(string key, double? value)
        {
            return new KeyValuePair<string, double?>(key, value);
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
