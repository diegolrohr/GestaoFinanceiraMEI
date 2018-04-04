using Fly01.Faturamento.DAL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core;
using Fly01.Core.Api.BL;
using Fly01.Core.Helpers;
using System.Collections.Generic;
using System.Linq;
using EmpresaNfeVM = Fly01.EmissaoNFE.Domain.ViewModel.EmpresaVM;
using EmpresaVM = Fly01.Utils.VM.EmpresaVM;

namespace Fly01.Faturamento.BL
{
    public class EntidadeBL : PlataformaBaseBL<Entidade>
    {
        protected EstadoBL EstadoBL;
        private readonly Dictionary<string, string> _queryString;
        private readonly Dictionary<string, string> _header;

        public EntidadeBL(AppDataContext context, EstadoBL estadoBL) : base(context)
        {
            EstadoBL = estadoBL;
            _queryString = AppDefaults.GetQueryStringDefault();
            _header = new Dictionary<string, string>
            {
                {"PlataformaUrl", PlataformaUrl},
                {"AppUser", AppUser},
                {"PlataformaId", PlataformaUrl},
                {"UsuarioInclusao", AppUser}
            };
        }
        
        public EmpresaNfeVM RetornaEntidade()
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
                                    _queryString,
                                    _header);

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
    }
}