using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Faturamento.DAL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Faturamento.Domain.Enums;
using Fly01.Core.BL;
using Fly01.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TipoAmbienteNFe = Fly01.EmissaoNFE.Domain.Enums.TipoAmbiente;
using Fly01.Core.Rest;
using Fly01.Core;
using Fly01.Core.Notifications;

namespace Fly01.Faturamento.BL
{
    public class CertificadoDigitalBL : PlataformaBaseBL<CertificadoDigital>
    {
        private Dictionary<string, string> GetHeaderDefault()
        {
            return new Dictionary<string, string>()
            {
                { "PlataformaUrl", PlataformaUrl },
                { "AppUser", AppUser }
            };
        }

        protected EntidadeBL EntidadeBL;
        protected ParametroTributarioBL ParametroTributarioBL;

        public CertificadoDigitalBL(AppDataContext context, EntidadeBL entidadeBL, ParametroTributarioBL parametroTributarioBL) : base(context)
        {
            EntidadeBL = entidadeBL;
            ParametroTributarioBL = parametroTributarioBL;
        }

        public IQueryable<CertificadoDigital> AllWithoutPlataformaId => repository.All.Where(x => x.Ativo);

        private CertificadoRetornoVM EnviaCertificadoEmissaoNFE(CertificadoDigital entity)
        {
            var ambiente = GetAmbiente();

            var certificado = new CertificadoVM
            {
                Homologacao = entity.EntidadeHomologacao,
                Producao = entity.EntidadeProducao,
                EntidadeAmbiente = (TipoAmbienteNFe)Enum.Parse(typeof(TipoAmbienteNFe), ambiente.ToString()),
                Certificado = entity.Certificado,
                Senha = entity.Senha,
                MD5 = entity.Md5,
                PlataformaId = PlataformaUrl
            };

            return RestHelper.ExecutePostRequest<CertificadoRetornoVM>(AppDefaults.UrlEmissaoNfeApi, "certificado", certificado, null, GetHeaderDefault());
        }

        public CertificadoDigital ProcessEntity(CertificadoDigital entity)
        {
            var ambiente = GetAmbiente();
            var entidade = GetEntidade(ambiente);

            entity.Md5 = Base64Helper.CalculaMD5Hash(entity.Certificado);

            if (string.IsNullOrEmpty(entidade))
            {
                var entidadeNFEId = EntidadeBL.RetornaEntidade();

                if (string.IsNullOrEmpty(entidadeNFEId.ToString()))
                    throw new BusinessException("Entidade não encontrada.");
                    
                entity.EntidadeHomologacao = entidadeNFEId.Homologacao;
                entity.EntidadeProducao = entidadeNFEId.Producao;
            }

            var responseNFE = EnviaCertificadoEmissaoNFE(entity);
            entity.Tipo = int.Parse(responseNFE.Tipo);
            entity.DataEmissao = responseNFE.DataEmissao;
            entity.DataExpiracao = responseNFE.DataExpiracao;
            entity.Emissor = responseNFE.Emissor;
            entity.Pessoa = responseNFE.Pessoa;
            entity.Versao = responseNFE.Versao;

            return entity;
        }

        public string GetEntidade(TipoAmbiente tipoAmbiente)
        {
            string entidade;

            if ((int)tipoAmbiente == 2)
            {
                entidade = All.FirstOrDefault().EntidadeHomologacao;
            }
            else
            {
                entidade = All.FirstOrDefault().EntidadeProducao;
            }

            return entidade;
        }

        public TipoAmbiente GetAmbiente()
        {
            var parametros = ParametroTributarioBL.All.AsNoTracking().FirstOrDefault();
                
            var retorno = parametros != null ? parametros.TipoAmbiente : TipoAmbiente.Homologacao;

            return retorno;
        }
    }
}