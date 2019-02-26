﻿using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using Fly01.Core.Helpers;
using System.Collections.Generic;
using System.Linq;
using Fly01.Core.Rest;
using Fly01.Core;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels;
using System;
using Fly01.Compras.DAL;

namespace Fly01.Compras.BL
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

        protected EstadoBL EstadoBL;
        protected ParametroTributarioBL ParametroTributarioBL;
        private ManagerEmpresaVM empresa;
        private string empresaUF;

        public CertificadoDigitalBL(AppDataContext context, EstadoBL estadoBL, ParametroTributarioBL parametroTributarioBL) : base(context)
        {
            EstadoBL = estadoBL;
            ParametroTributarioBL = parametroTributarioBL;
            empresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            empresaUF = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla : string.Empty) : string.Empty;
            MustConsumeMessageServiceBus = true;
        }

        public IQueryable<CertificadoDigital> Everything => repository.All.Where(x => x.Ativo);

        private CertificadoRetornoVM EnviaCertificadoEmissaoNFE(CertificadoDigital entity)
        {
            var certificado = new CertificadoVM
            {
                Homologacao = entity.EntidadeHomologacao,
                Producao = entity.EntidadeProducao,
                Certificado = entity.Certificado,
                Senha = entity.Senha,
                MD5 = entity.Md5,
                PlataformaId = PlataformaUrl
            };

            return RestHelper.ExecutePostRequest<CertificadoRetornoVM>(AppDefaults.UrlEmissaoNfeApi, "certificado", certificado, null, GetHeaderDefault());
        }

        public CertificadoDigital ProcessEntity(CertificadoDigital entity)
        {
            var ambiente = GetEntidade(true);

            #region ResgataDadosEmpresa

            entity.Cnpj = empresa.CNPJ;
            entity.UF = empresaUF;
            entity.InscricaoEstadual = empresa.InscricaoEstadual;

            #endregion

            entity.Md5 = Base64Helper.CalculaMD5Hash(entity.Certificado);

            var entidadeNFEId = ambiente.Homologacao != null && ambiente.Producao != null ? ambiente : RetornaEntidade();

            if (string.IsNullOrEmpty(entidadeNFEId.ToString()))
                throw new BusinessException("Entidade não encontrada.");

            entity.EntidadeHomologacao = entidadeNFEId.Homologacao;
            entity.EntidadeProducao = entidadeNFEId.Producao;

            var responseNFE = EnviaCertificadoEmissaoNFE(entity);
            entity.Tipo = int.Parse(responseNFE.Tipo);
            entity.DataEmissao = responseNFE.DataEmissao;
            entity.DataExpiracao = responseNFE.DataExpiracao;
            entity.Emissor = responseNFE.Emissor.Substring(0, responseNFE.Emissor.Length > 200 ? 200 : responseNFE.Emissor.Length);
            entity.Pessoa = responseNFE.Pessoa.Substring(0, responseNFE.Pessoa.Length > 200 ? 200 : responseNFE.Pessoa.Length);
            entity.Versao = responseNFE.Versao.Substring(0, responseNFE.Versao.Length > 200 ? 200 : responseNFE.Versao.Length);

            return entity;
        }

        public EntidadeVM RetornaEntidade()
        {
            string estadoSigla = empresa?.Cidade?.Estado?.Sigla;

            var entidade = new EmpresaVM
            {
                Nome = empresa.RazaoSocial,
                NIRE = empresa.Nire,
                Municipio = empresa.Cidade?.Nome,
                CodigoIBGECidade = empresa.Cidade?.CodigoIbge,
                InscricaoMunicipal = empresa.InscricaoMunicipal,
                InscricaoEstadual = empresa.InscricaoEstadual,
                Fone = empresa.Telefone,
                Fantasia = empresa.NomeFantasia,
                Email = empresa.Email,
                Cnpj = empresa.CNPJ,
                Cep = empresa.CEP,
                Bairro = empresa.Bairro,
                Endereco = empresa.Endereco,
                UF = estadoSigla
            };

            var empresaNfe = RestHelper.ExecutePostRequest<EmpresaVM>
                                (AppDefaults.UrlEmissaoNfeApi,
                                    "Empresa",
                                    entidade,
                                    null,
                                    GetHeaderDefault());

            return empresaNfe;
        }

        public EntidadeVM GetEntidade(bool postCertificado = false)
        {
            var certificado = All.Where(x => x.Cnpj == empresa.CNPJ && x.InscricaoEstadual == empresa.InscricaoEstadual && x.UF == empresaUF).FirstOrDefault();

            if (certificado == null && !postCertificado)
            {
                throw new BusinessException("Cadastre o seu Certificado Digital em Configurações");
            }

            var parametros = ParametroTributarioBL.All.Where(x => x.Cnpj == empresa.CNPJ && x.InscricaoEstadual == empresa.InscricaoEstadual && x.UF == empresaUF).FirstOrDefault();

            var ambiente = parametros != null ? parametros.TipoAmbiente : TipoAmbiente.Homologacao;
            var ambienteNFS = parametros != null ? parametros.TipoAmbienteNFS : TipoAmbiente.Homologacao;
            var retorno = new EntidadeVM();

            if (certificado != null && !string.IsNullOrEmpty(certificado.EntidadeHomologacao) && !string.IsNullOrEmpty(certificado.EntidadeProducao))
            {
                retorno.Homologacao = certificado.EntidadeHomologacao;
                retorno.Producao = certificado.EntidadeProducao;
            }
            else
            {
                retorno = RetornaEntidade();
            }
            retorno.EntidadeAmbiente = ambiente;
            retorno.EntidadeAmbienteNFS = ambienteNFS;
            return retorno;
        }

        public EntidadeVM GetEntidade(string plataformaId)
        {
            var empresa = String.IsNullOrEmpty(plataformaId) ? this.empresa : ApiEmpresaManager.GetEmpresa(plataformaId);
            var certificado = Everything.Where(x => x.PlataformaId == plataformaId && x.Cnpj == empresa.CNPJ && x.InscricaoEstadual == empresa.InscricaoEstadual && x.UF == empresaUF).FirstOrDefault();
            var ambiente = ParametroTributarioBL.Everything.Where(x => x.PlataformaId == plataformaId && x.Cnpj == empresa.CNPJ && x.InscricaoEstadual == empresa.InscricaoEstadual && x.UF == empresaUF).FirstOrDefault();

            if (certificado == null || ambiente == null || plataformaId == null)
                return null;

            var retorno = new EntidadeVM
            {
                EntidadeAmbiente = ambiente.TipoAmbiente,
                EntidadeAmbienteNFS = ambiente.TipoAmbienteNFS
            };

            if (!string.IsNullOrEmpty(certificado.EntidadeHomologacao) && !string.IsNullOrEmpty(certificado.EntidadeProducao))
            {
                retorno.Homologacao = certificado.EntidadeHomologacao;
                retorno.Producao = certificado.EntidadeProducao;
            }
            else
            {
                var entidades = RetornaEntidade();
                retorno.Homologacao = entidades.Homologacao;
                retorno.Producao = entidades.Producao;
            }
            return retorno;
        }

        public EntidadeVM GetEntidadeFromCertificado(string plataformaId, TipoAmbiente tipoAmbiente, Guid? certificadoId)
        {
            var certificadoDigital = Everything.Where(x => x.Id == certificadoId).FirstOrDefault();

            var entidade = new EntidadeVM();
            if (certificadoDigital != null)
            {
                entidade.Homologacao = certificadoDigital.EntidadeHomologacao;
                entidade.Producao = certificadoDigital.EntidadeProducao;
                entidade.EntidadeAmbiente = tipoAmbiente;
            }
            else
            {
                entidade = GetEntidade(plataformaId);
            }
            return entidade;
        }

        public CertificadoDigital CertificadoAtualValido()
        {
            //retorna conforme os dados atuais da empresa
            return All.FirstOrDefault(x => x.Cnpj == empresa.CNPJ && x.InscricaoEstadual == empresa.InscricaoEstadual && x.UF == empresaUF);
        }
    }
}