using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using System.Collections.Generic;
using System.Linq;
using Fly01.Core;
using Fly01.Core.Rest;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.ViewModels;
using Fly01.Compras.DAL;

namespace Fly01.Compras.BL
{
    public class EntidadeBL : PlataformaBaseBL<CertificadoDigital>
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
        private ManagerEmpresaVM empresa;
        private string empresaUF;

        protected void GetOrUpdateEmpresa()
        {
            if (empresa == null || (empresa != null && empresa?.PlatformUrl?.Fly01Url != PlataformaUrl))
            {
                empresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
                empresaUF = empresa.Cidade != null ? (empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Sigla : string.Empty) : string.Empty;
            }
        }

        public EntidadeBL(AppDataContext context, EstadoBL estadoBL) : base(context)
        {
            EstadoBL = estadoBL;
        }

        public EntidadeVM RetornaEntidade()
        {
            var empresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            string estadoNome =  empresa.Cidade != null && empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Nome : string.Empty;

            var estado = EstadoBL.All.FirstOrDefault(x => x.Nome == estadoNome);

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
                UF = estado?.Sigla
            };

            var empresaNfe = RestHelper.ExecutePostRequest<EmpresaVM>
                                (AppDefaults.UrlEmissaoNfeApi,
                                    "Empresa",
                                    entidade,
                                    null,
                                    GetHeaderDefault());

            return empresaNfe;
        }

        public EntidadeVM GetEntidade()
        {
            GetOrUpdateEmpresa();
            var certificado = All.Where(x => x.Cnpj == empresa.CNPJ && x.InscricaoEstadual == empresa.InscricaoEstadual && x.UF == empresaUF).FirstOrDefault();
            
            if (certificado != null && certificado.EntidadeHomologacao != null && certificado.EntidadeProducao != null)
            {
                var retorno = new EntidadeVM
                {
                    Homologacao = certificado.EntidadeHomologacao,
                    Producao = certificado.EntidadeProducao
                };

                return retorno;
            }
            else
            {
                var entidades = RetornaEntidade();
                
                return entidades;
            }
        }
    }
}