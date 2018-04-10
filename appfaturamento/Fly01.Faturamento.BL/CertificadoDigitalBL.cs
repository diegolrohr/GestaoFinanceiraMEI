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
using EmpresaNfeVM = Fly01.EmissaoNFE.Domain.ViewModel.EmpresaVM;
using EmpresaVM = Fly01.Core.VM.EmpresaVM;
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

        protected EstadoBL EstadoBL;
        protected ParametroTributarioBL ParametroTributarioBL;

        public CertificadoDigitalBL(AppDataContext context, EstadoBL estadoBL, ParametroTributarioBL parametroTributarioBL) : base(context)
        {
            EstadoBL = estadoBL;
            ParametroTributarioBL = parametroTributarioBL;
        }

        public IQueryable<CertificadoDigital> AllWithoutPlataformaId => repository.All.Where(x => x.Ativo);

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
            entity.Emissor = responseNFE.Emissor;
            entity.Pessoa = responseNFE.Pessoa;
            entity.Versao = responseNFE.Versao;

            return entity;
        }

        public EntidadeVM RetornaEntidade()
        {
            var empresa = GetDadosEmpresa();

            var estado = EstadoBL.All.FirstOrDefault(x => x.Nome == empresa.EstadoNome);

            var entidade = new EmpresaNfeVM
            {
                Nome = empresa.RazaoSocial,
                NIRE = empresa.Nire,
                Municipio = empresa.Cidade?.Nome,
                CodigoIBGE = empresa.Cidade?.CodigoIbge,
                InscricaoMunicipal = empresa.InscricaoMunicipal,
                InscricaoEstadual = empresa.InscricaoEstadual,
                Fone = empresa.Telefone,
                Fantasia = empresa.NomeFantasia,
                Email = empresa.Email,
                Cnpj = empresa.CNPJ,
                Cep = empresa.CEP,
                Bairro = empresa.Bairro,
                Endereco = empresa.Endereco,
                UF = estado?.Sigla
            };

            var empresaNfe = RestHelper.ExecutePostRequest<EmpresaNfeVM>
                                (AppDefaults.UrlEmissaoNfeApi,
                                    "Empresa",
                                    entidade,
                                    null,
                                    GetHeaderDefault());

            return empresaNfe;
        }

        private EmpresaVM GetDadosEmpresa()
        {
            var urlGateway = AppDefaults.UrlGateway
                                .Replace("financeiro/", string.Empty)
                                .Replace("faturamento/", string.Empty)
                                .Replace("estoque/", string.Empty)
                                .Replace("compras/", string.Empty);

            return RestHelper.ExecuteGetRequest<EmpresaVM>(urlGateway, $"Empresa/{PlataformaUrl}");
        }

        public string GetEntidade(TipoAmbienteNFe tipoAmbiente)
        {
            string entidade;
            var certificado = All.FirstOrDefault();

            if (certificado == null)
            {
                throw new BusinessException("Cadastre o seu Certificado Digital em Configurações");
            }

            if (!string.IsNullOrEmpty(certificado.EntidadeHomologacao) && !string.IsNullOrEmpty(certificado.EntidadeProducao))
            {
                if ((int)tipoAmbiente == 2)
                {
                    entidade = certificado.EntidadeHomologacao;
                }
                else
                {
                    entidade = certificado.EntidadeProducao;
                }
            }
            else
            {
                var retorno = RetornaEntidade();
                if ((int)tipoAmbiente == 2)
                {
                    entidade = retorno.Homologacao;
                }
                else
                {
                    entidade = retorno.Producao;
                }
            }

            return entidade;
        }

        public EntidadeVM GetEntidade(bool postCertificado = false)
        {
            var certificado = All.FirstOrDefault();

            if(certificado == null && !postCertificado)
            {
                throw new BusinessException("Cadastre o seu Certificado Digital em Configurações");
            }

            var parametros = ParametroTributarioBL.All.AsNoTracking().FirstOrDefault();

            var ambiente = parametros != null ? parametros.TipoAmbiente : TipoAmbiente.Homologacao;

            if (certificado != null && !string.IsNullOrEmpty(certificado.EntidadeHomologacao) && !string.IsNullOrEmpty(certificado.EntidadeProducao))
            {
                var retorno = new EntidadeVM
                {
                    Homologacao = certificado.EntidadeHomologacao,
                    Producao = certificado.EntidadeProducao,
                    EntidadeAmbiente = (TipoAmbienteNFe)Enum.Parse(typeof(TipoAmbienteNFe), ambiente.ToString())
                };

                return retorno;
            }
            else
            {
                var entidades = RetornaEntidade();

                entidades.EntidadeAmbiente = (TipoAmbienteNFe)Enum.Parse(typeof(TipoAmbienteNFe), ambiente.ToString());

                return entidades;
            }
        }

        public EntidadeVM GetEntidade(string plataformaId)
        {
            var certificado = AllWithoutPlataformaId.Where(x => x.PlataformaId == plataformaId).FirstOrDefault();
            
            var ambiente = ParametroTributarioBL.AllWithoutPlataformaId.Where(x => x.PlataformaId == plataformaId).FirstOrDefault();

            if (certificado == null || ambiente == null || plataformaId == null)
                return null;

            if (!string.IsNullOrEmpty(certificado.EntidadeHomologacao) && !string.IsNullOrEmpty(certificado.EntidadeProducao))
            {
                var retorno = new EntidadeVM
                {
                    Homologacao = certificado.EntidadeHomologacao,
                    Producao = certificado.EntidadeProducao,
                    EntidadeAmbiente = (TipoAmbienteNFe)Enum.Parse(typeof(TipoAmbienteNFe), ambiente.TipoAmbiente.ToString())
                };

                return retorno;
            }
            else
            {
                var entidades =  RetornaEntidade();

                var retorno = new EntidadeVM
                {
                    Homologacao = entidades.Homologacao,
                    Producao = entidades.Producao,
                    EntidadeAmbiente = (TipoAmbienteNFe)Enum.Parse(typeof(TipoAmbienteNFe), ambiente.TipoAmbiente.ToString())
                };

                return retorno;
            }
        }
    }
}