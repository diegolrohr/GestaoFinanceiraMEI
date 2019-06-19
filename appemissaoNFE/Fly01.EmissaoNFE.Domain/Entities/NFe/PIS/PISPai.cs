using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.PIS
{

    public class PISPai
    {
        private IList<KeyValuePair<string, double?>> ToValidate = new List<KeyValuePair<string, double?>>();
        private static string ErrNotInformed = @"Os seguintes campos obrigatórios não foram informados: ";

        public PISPai() { }

        public void DoThePIS(TipoCRT crt)
        {
            if (crt.Equals(TipoCRT.SimplesNacional| TipoCRT.RegimeNormal|TipoCRT.ExcessoSublimiteDeReceitaBruta)) 
            {
                var adValorem = "01|02";
                var AliqEspecifica = "03";
                var NT = "04|05|06|07|08|09";

                if (adValorem.Contains(((int)CodigoSituacaoTributaria).ToString()))
                {
                    PIS = new PISAliq("0" + ((int)CodigoSituacaoTributaria).ToString(), TipoCRT)
                    {
                        ValorBCDoPIS = ValorBCDoPIS,
                        PercentualPIS = PercentualPIS,
                        ValorPIS = ValorPIS
                    };
                }
                else if (AliqEspecifica.Contains(((int)CodigoSituacaoTributaria).ToString()))
                {
                    PIS = new PISQtde("0" + ((int)CodigoSituacaoTributaria).ToString(), TipoCRT)
                    {
                        AliquotaPISST = AliquotaPISST,
                        QuantidadeVendida = QuantidadeVendida,
                        ValorPIS = ValorPIS
                    };
                }
                else if(NT.Contains(((int)CodigoSituacaoTributaria).ToString()))
                {
                    PIS = new PISNT("0" + ((int)CodigoSituacaoTributaria).ToString(), TipoCRT)
                    {
                    };
                }
                else
                {
                    if(AliquotaPISST != null)
                    {
                        PIS = new PISOutr(((int)CodigoSituacaoTributaria).ToString(), TipoCRT)
                        {
                            AliquotaPISST = AliquotaPISST,
                            QuantidadeVendida = QuantidadeVendida,
                            ValorPIS = ValorPIS
                        };
                    }
                    else
                    {                        
                        PIS = new PISOutr(((int)CodigoSituacaoTributaria).ToString(), TipoCRT)
                        {
                            ValorBCDoPIS = ValorBCDoPIS,
                            PercentualPIS = PercentualPIS,
                            ValorPIS = ValorPIS
                        };
                    }
                }
            }
            else
                throw new NotImplementedException();
        }
        
        public PIS PIS { get; set; }

        [XmlIgnore]
        public CSTPISCOFINS CodigoSituacaoTributaria { get; set; }

        [XmlIgnore]
        public double ValorPIS { get; set; }

        [XmlIgnore]
        public double? PercentualPIS { get; set; }

        [XmlIgnore]
        public double? ValorBCDoPIS { get; set; }

        [XmlIgnore]
        public double? QuantidadeVendida { get; set; }

        [XmlIgnore]
        public double? AliquotaPISST { get; set; }

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
