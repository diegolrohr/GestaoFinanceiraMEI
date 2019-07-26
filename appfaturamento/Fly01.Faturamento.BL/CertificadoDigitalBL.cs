using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Faturamento.DAL;
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
using Fly01.Core.API;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Config;

namespace Fly01.Faturamento.BL
{
    public class CertificadoDigitalBL : PlataformaBaseBL<CertificadoDigital>
    {
        protected EstadoBL EstadoBL;
        protected ParametroTributarioBL ParametroTributarioBL;
        private ManagerEmpresaVM empresa;
        private string empresaUF;
        private List<int> PeriodosVerificacao = new List<int>() { 30, 20, 10, 7, 3, 1 };

        protected void GetOrUpdateEmpresa()
        {
            if (empresa == null || (empresa != null && empresa?.PlatformUrl?.Fly01Url != PlataformaUrl))
            {
                empresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
                empresaUF = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla : string.Empty) : string.Empty;
            }
        }

        public ManagerEmpresaVM GetEmpresa()
        {
            GetOrUpdateEmpresa();
            return empresa;
        }

        private Dictionary<string, string> GetHeaderDefault()
        {
            return new Dictionary<string, string>()
            {
                { "PlataformaUrl", PlataformaUrl },
                { "AppUser", AppUser }
            };
        }

        private bool PeriodoNotificacao(int dateDiff)
        {
            return PeriodosVerificacao.Contains(dateDiff);
        }

        private bool CertificadoJaVencido(int dateDiff)
        {
            return dateDiff <= 0;
        }

        public void VerificaValidade()
        {
            var dataExpiracaoFinal = DateTime.Now.AddDays(PeriodosVerificacao.Max()).Date;
            var certificadosVencidos = Everything.Where(x => x.DataExpiracao.HasValue && x.DataExpiracao.Value <= dataExpiracaoFinal && x.Ativo == true);

            foreach (var item in certificadosVencidos)
            {
                try
                {
                    var dadosEmpresa = ApiEmpresaManager.GetEmpresa(item.PlataformaId);

                    if (item.Cnpj == dadosEmpresa.CNPJ)
                    {
                        var dateDiff = (item.DataExpiracao.Value.Date - DateTime.Now.Date).Days;
                        if (PeriodoNotificacao(dateDiff) || CertificadoJaVencido(dateDiff))
                        {
                            var vencimento = " irá vencer em ";
                            var dias = dateDiff + " dias ";
                            var messageType = EnumHelper.GetKey(typeof(SocketMessageType), "WARNING");
                            if (dateDiff <= 0)
                            {
                                vencimento = " já venceu";
                                dias = "";
                                messageType = EnumHelper.GetKey(typeof(SocketMessageType), "ERROR");
                            }
                            else if (dateDiff == 1)
                            {
                                dias = "1 dia";
                            }

                            var message = new SocketMessageVM()
                            {
                                Message = $"O Certificado Digital do CNPJ:{item.Cnpj}{vencimento}{dias}({item.DataExpiracao?.ToString("dd/MM/yyyy")}). Atualize para continuar a emitir suas Notas Fiscais.",
                                PlatformId = item.PlataformaId,
                                NotificationDate = DateTime.Now,
                                MessageType = messageType.ToString(),
                                PlatformApps = new List<SocketPlatformAppVM>()
                        {
                            new SocketPlatformAppVM()
                            {
                                ActionUrl = $"{AppDefaults.UrlFaturamentoWeb}CertificadoDigital",
                                ClientId = AppDefaults.FaturamentoClientId
                            },
                            new SocketPlatformAppVM()
                            {
                                ActionUrl = $"{AppDefaults.UrlComprasWeb}CertificadoDigital",
                                ClientId = AppDefaults.ComprasClientId
                            }
                        },
                                ReadDate = null
                            };

                            SocketIOHelper.NewMessage(message);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            };
        }

        public CertificadoDigitalBL(AppDataContext context, EstadoBL estadoBL, ParametroTributarioBL parametroTributarioBL) : base(context)
        {
            EstadoBL = estadoBL;
            ParametroTributarioBL = parametroTributarioBL;
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

            GetOrUpdateEmpresa();

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
            GetOrUpdateEmpresa();
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
                Cnpj = empresa?.CNPJ?.Length == 14 ? empresa.CNPJ : null,
                Cpf = empresa?.CNPJ?.Length == 11 ? empresa.CNPJ : null,
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
            GetOrUpdateEmpresa();
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
            if (string.IsNullOrEmpty(plataformaId))
            {
                GetOrUpdateEmpresa();
                var certificado = Everything.Where(x => x.PlataformaId == PlataformaUrl && x.Cnpj == empresa.CNPJ && x.InscricaoEstadual == empresa.InscricaoEstadual && x.UF == empresaUF).FirstOrDefault();
                var ambiente = ParametroTributarioBL.Everything.Where(x => x.PlataformaId == PlataformaUrl && x.Cnpj == empresa.CNPJ && x.InscricaoEstadual == empresa.InscricaoEstadual && x.UF == empresaUF).FirstOrDefault();
                var retorno = RetornaEntidade(certificado, ambiente, empresa);

                return retorno;
            }
            else
            {
                var empresa = ApiEmpresaManager.GetEmpresa(plataformaId);
                var empresaUF = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla : string.Empty) : string.Empty;
                var certificado = Everything.Where(x => x.PlataformaId == plataformaId && x.Cnpj == empresa.CNPJ && x.InscricaoEstadual == empresa.InscricaoEstadual && x.UF == empresaUF).FirstOrDefault();
                var ambiente = ParametroTributarioBL.Everything.Where(x => x.PlataformaId == plataformaId && x.Cnpj == empresa.CNPJ && x.InscricaoEstadual == empresa.InscricaoEstadual && x.UF == empresaUF).FirstOrDefault();
                var retorno = RetornaEntidade(certificado, ambiente, empresa);

                return retorno;
            }
        }

        protected EntidadeVM RetornaEntidade(CertificadoDigital certificado, ParametroTributario ambiente, ManagerEmpresaVM empresa)
        {
            if (certificado == null || ambiente == null || empresa?.PlatformUrlId == null)
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
            //Recupera pela informação salva no momento da transmissão
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
            GetOrUpdateEmpresa();
            //retorna conforme os dados atuais da empresa
            return All.FirstOrDefault(x => x.Cnpj == empresa.CNPJ && x.InscricaoEstadual == empresa.InscricaoEstadual && x.UF == empresaUF);
        }

        public List<CertificadoDigital> TodosCertificados()
        {
            List<CertificadoDigital> certificados = All.ToList();
            return certificados;
        }

        public override void ValidaModel(CertificadoDigital entity)
        {
            GetOrUpdateEmpresa();
            entity.Cnpj = empresa.CNPJ;
            entity.UF = empresaUF;
            entity.InscricaoEstadual = empresa.InscricaoEstadual;

            base.ValidaModel(entity);
        }
    }
}