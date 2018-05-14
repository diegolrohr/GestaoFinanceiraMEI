using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using Fly01.Core;
using Fly01.Core.Rest;
using Fly01.Core.Notifications;
using Fly01.Core.ViewModels;

namespace Fly01.Faturamento.BL
{
    public class ParametroTributarioBL : PlataformaBaseBL<ParametroTributario>
    {
        private readonly Dictionary<string, string> _queryString;
        private readonly Dictionary<string, string> _header;
        private ManagerEmpresaVM empresa;
        protected EntidadeBL EntidadeBL { get; set; }

        public ParametroTributarioBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            empresa = RestHelper.ExecuteGetRequest<ManagerEmpresaVM>($"{AppDefaults.UrlGateway}v2/", $"Empresa/{PlataformaUrl}");
            EntidadeBL = entidadeBL;
            _queryString = AppDefaults.GetQueryStringDefault();
            _header = new Dictionary<string, string>
            {
                {"PlataformaUrl", PlataformaUrl},
                {"AppUser", AppUser},
                {"PlataformaId", PlataformaUrl},
                {"UsuarioInclusao", AppUser}
            };
        }

        public IQueryable<ParametroTributario> AllWithoutPlataformaId => repository.All.Where(x => x.Ativo);
        
        public void EnviaParametroTributario(ParametroTributario parametroTributario)
        {

            #region ResgataDadosEmpresa

            parametroTributario.Cnpj = empresa.CNPJ;
            parametroTributario.UF = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla : string.Empty) : string.Empty;
            parametroTributario.InscricaoEstadual = empresa.InscricaoEstadual;

            #endregion

            var consultaEntidade = EntidadeBL.GetEntidade();
            
            var entidade = consultaEntidade.Homologacao == null || consultaEntidade.Producao == null ? EntidadeBL.RetornaEntidade() : consultaEntidade;
            if (entidade != null)
            {
                try
                {
                    var parametro = new ParametroVM
                    {
                        Homologacao = entidade.Homologacao,
                        Producao = entidade.Producao,
                        EntidadeAmbiente = parametroTributario.TipoAmbiente,
                        TipoAmbiente = parametroTributario.TipoAmbienteRest,
                        VersaoNFe = parametroTributario.TipoVersaoNFeRest == "3" ? "3.10" : "4.0",
                        VersaoNFSe = "0.00",
                        VersaoDPEC = "1.01",
                        TipoModalidade = parametroTributario.TipoModalidadeRest,
                        NumeroRetornoNF = string.IsNullOrEmpty(parametroTributario.NumeroRetornoNF) ? string.Empty : parametroTributario.NumeroRetornoNF,
                        EnviaDanfe = false,
                        UsaEPEC = false                        
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
        public bool IsValid(ParametroTributario parametro = null)
        {

            var dadosEmpresa = RestHelper.ExecuteGetRequest<ManagerEmpresaVM>($"{AppDefaults.UrlGateway}v2/", $"Empresa/{PlataformaUrl}");
            var empresaCNPJ = dadosEmpresa.CNPJ;
            var empresaIE = dadosEmpresa.InscricaoEstadual;
            var empresaUF = dadosEmpresa.Cidade != null ? (dadosEmpresa.Cidade.Estado != null ? dadosEmpresa.Cidade.Estado.Sigla : string.Empty) : string.Empty;

            if (parametro == null)
                parametro = All.FirstOrDefault();

            return (parametro != null) && ((empresaCNPJ == parametro.Cnpj && empresaIE == parametro.InscricaoEstadual && empresaUF == parametro.UF));
        }

    }
}