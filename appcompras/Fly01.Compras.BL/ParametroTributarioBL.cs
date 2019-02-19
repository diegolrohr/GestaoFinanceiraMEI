﻿using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using Fly01.Core;
using Fly01.Core.Rest;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels;

namespace Fly01.Compras.BL
{
    public class ParametroTributarioBL : PlataformaBaseBL<ParametroTributario>
    {
        private readonly Dictionary<string, string> _queryString;
        private readonly Dictionary<string, string> _header;
        private ManagerEmpresaVM empresa;
        private string empresaUF;
        protected EntidadeBL EntidadeBL { get; set; }

        public ParametroTributarioBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            MustConsumeMessageServiceBus = true;
            empresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            empresaUF = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla : string.Empty) : string.Empty;
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

        public IQueryable<ParametroTributario> Everything => repository.All.Where(x => x.Ativo);
        
        public void EnviaParametroTributario(ParametroTributario parametroTributario)
        {
            #region ResgataDadosEmpresa

            parametroTributario.Cnpj = empresa.CNPJ;
            parametroTributario.UF = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla : string.Empty) : string.Empty;
            parametroTributario.InscricaoEstadual = empresa.InscricaoEstadual;

            var ibge = empresa.Cidade != null ? empresa.Cidade.CodigoIbge : string.Empty;
            
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
                        EntidadeAmbienteNFS = parametroTributario.TipoAmbienteNFS,
                        TipoAmbiente = parametroTributario.TipoAmbienteRest,
                        VersaoNFe = parametroTributario.TipoVersaoNFeRest == "3" ? "3.10" : "4.00",
                        VersaoDPEC = "1.01",
                        TipoModalidade = parametroTributario.TipoModalidadeRest,
                        NumeroRetornoNF = string.IsNullOrEmpty(parametroTributario.NumeroRetornoNF) ? string.Empty : parametroTributario.NumeroRetornoNF,
                        EnviaDanfe = false,
                        UsaEPEC = false,
                        HorarioVerao = parametroTributario.HorarioVerao,
                        TipoHorario = parametroTributario.TipoHorario,
                        SimplesNacional = true,
                        VersaoNFSe = parametroTributario.VersaoNFSe,
                        CodigoIBGECidade = ibge,
                        UsuarioWebServer = parametroTributario.UsuarioWebServer,
                        SenhaWebServer = parametroTributario.SenhaWebServer,
                        ChaveAutenticacao = parametroTributario.ChaveAutenticacao,
                        Autorizacao = parametroTributario.Autorizacao,
                        TipoAmbienteNFS = parametroTributario.TipoAmbienteNFSRest,
                        IncentivoCultura = parametroTributario.IncentivoCultura,
                        TipoTributacaoNFS = parametroTributario.TipoTributacaoNFS
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
        
        public ParametroTributario ParametroAtualValido()
        {
            return All.FirstOrDefault(x => x.Cnpj == empresa.CNPJ && x.InscricaoEstadual == empresa.InscricaoEstadual && x.UF == empresaUF);
        }

        public override void ValidaModel(ParametroTributario entity)
        {
            entity.Fail(entity.TipoVersaoNFe != TipoVersaoNFe.v4, new Error("Permitido somente a versão 4.00."));
            if (string.IsNullOrEmpty(entity.VersaoNFSe))
                entity.VersaoNFSe = "0.00";

            base.ValidaModel(entity);
        }
    }
}