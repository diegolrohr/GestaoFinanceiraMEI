using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core.Domain;
using Fly01.Core.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using TipoAmbienteNFe = Fly01.EmissaoNFE.Domain.Enums.TipoAmbiente;
using Fly01.Core;
using Fly01.Core.Rest;
using Fly01.Core.Notifications;

namespace Fly01.Faturamento.BL
{
    public class ParametroTributarioBL : PlataformaBaseBL<ParametroTributario>
    {
        private readonly Dictionary<string, string> _queryString;
        private readonly Dictionary<string, string> _header;
        protected EntidadeBL EntidadeBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }

        public ParametroTributarioBL(AppDataContextBase context, EntidadeBL entidadeBL, CertificadoDigitalBL certificadoDigitalBL) : base(context)
        {
            EntidadeBL = entidadeBL;
            CertificadoDigitalBL = certificadoDigitalBL;
            _queryString = AppDefaults.GetQueryStringDefault();
            _header = new Dictionary<string, string>
            {
                {"PlataformaUrl", PlataformaUrl},
                {"AppUser", AppUser},
                {"PlataformaId", PlataformaUrl},
                {"UsuarioInclusao", AppUser}
            };
        }

        public void EnviaParametroTributario(ParametroTributario parametroTributario)
        {
            var consultaCertificado = CertificadoDigitalBL.All.FirstOrDefault();

            var entidade = consultaCertificado == null ? EntidadeBL.RetornaEntidade() : new EmpresaVM(){
                Homologacao = consultaCertificado.EntidadeHomologacao,
                Producao = consultaCertificado.EntidadeProducao
            };

            if (entidade != null)
            {
                try
                {
                    var parametro = new ParametroVM
                    {
                        Homologacao = entidade.Homologacao,
                        Producao = entidade.Producao,
                        EntidadeAmbiente = (TipoAmbienteNFe)Enum.Parse(typeof(TipoAmbienteNFe), parametroTributario.TipoAmbiente.ToString()),
                        TipoAmbiente = parametroTributario.TipoAmbienteRest,
                        VersaoNFe = parametroTributario.TipoVersaoNFeRest == "3" ? "3.10" : "4.0",
                        VersaoNFSe = "0.00",
                        VersaoDPEC = "1.01",
                        TipoModalidade = parametroTributario.TipoModalidadeRest,
                        NumeroRetornoNF = string.IsNullOrEmpty(parametroTributario.NumeroRetornoNF) ? string.Empty : parametroTributario.NumeroRetornoNF,
                        EnviaDanfe = false,
                        UsaEPEC = false,
                    };

                    var response = RestHelper.ExecutePostRequest<ParametroVM>(AppDefaults.UrlEmissaoNfeApi,
                                                                       "parametronf",
                                                                       parametro,
                                                                       _queryString,
                                                                       _header);
                }
                catch (Exception e)
                {
                    throw new BusinessException(e.Message);
                }

            }
        }

    }
}