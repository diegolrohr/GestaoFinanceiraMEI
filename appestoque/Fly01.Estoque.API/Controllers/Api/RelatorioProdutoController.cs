using Fly01.Core.API;
using System.Web.Http;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using System;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Estoque.BL;
using System.Collections.Generic;
using Fly01.Estoque.Models.ViewModel;
using System.Linq;

namespace Fly01.Estoque.API.Controllers.Api
{
    [RoutePrefix("api/relatorioProduto")]
    public class RelatorioProdutoController : ApiBaseController
    {
        public IHttpActionResult Get(string descricao,
                                string codigo,
                                string tipoProduto,
                                Guid? grupoProdutoId,
                                Guid? unidadeMedidaId,
                                Guid? ncmId,
                                Guid? enquadramentoLegalIPIId,
                                string origemMercadoria)
        {

            Func<Produto, bool> filterPredicate = (x => (
                ((string.IsNullOrWhiteSpace(descricao)) || (x.Descricao == descricao)) &&
                ((string.IsNullOrWhiteSpace(codigo)) || (x.CodigoProduto == codigo)) &&
                ((string.IsNullOrWhiteSpace(tipoProduto)) || (x.TipoProduto == (TipoProduto)Enum.Parse(typeof(TipoProduto), tipoProduto, true))) &&
                ((!grupoProdutoId.HasValue) || (x.GrupoProdutoId == grupoProdutoId)) &&
                ((!unidadeMedidaId.HasValue) || (x.UnidadeMedidaId == unidadeMedidaId)) &&
                ((!ncmId.HasValue) || (x.NcmId == ncmId)) &&
                ((!enquadramentoLegalIPIId.HasValue) || (x.EnquadramentoLegalIPIId == enquadramentoLegalIPIId)) &&
                ((string.IsNullOrWhiteSpace(origemMercadoria)) || (x.OrigemMercadoria == (OrigemMercadoria)Enum.Parse(typeof(OrigemMercadoria), origemMercadoria, true))))
            );

            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                List<RelatorioProdutoVM> result = unitOfWork.ProdutoBL
                .AllIncluding(
                    x => x.GrupoProduto,
                    x => x.UnidadeMedida,
                    x => x.Ncm,
                    x => x.EnquadramentoLegalIPI
                ).Where(filterPredicate)
                .Take(2000)
                .Select(GetDisplayData()).ToList();

                return Ok(new { count = result.Count, value = result });
            }
        }

        private Func<Produto, RelatorioProdutoVM> GetDisplayData()
        {
            return x => new RelatorioProdutoVM()
            {
                Id = x.Id,
                Descricao = x.Descricao,
                Codigo = x.CodigoProduto,
                TipoProduto = EnumHelper.GetEnumDescription(x.TipoProduto),
                GrupoProduto = x.GrupoProduto.Descricao,
                UnidadeMedida = x.UnidadeMedida.Descricao,
                Ncm = x.Ncm.Descricao,
                EnquadramentoLegalIPI = x.EnquadramentoLegalIPI.Descricao,
                OrigemMercadoria = EnumHelper.GetEnumDescription(x.OrigemMercadoria)
            };
        }
    }
}