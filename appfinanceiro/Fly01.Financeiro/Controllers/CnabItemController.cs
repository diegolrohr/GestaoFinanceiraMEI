using System;
using Fly01.Core;
using System.Web.Mvc;
using Fly01.Financeiro.ViewModel;
using System.Collections.Generic;
using Fly01.Financeiro.Controllers.Base;

namespace Fly01.Estoque.Controllers
{
    public class CnabItemController : ContaFinanceiraController<ContaReceberVM, ContaFinanceiraBaixaVM, ContaFinanceiraRenegociacaoVM>
    {
        public CnabItemController() { }

        public override Func<ContaReceberVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                numero = x.Numero,
                dataVencimento = x.DataVencimento.ToString("dd/MM/yyyy"),
                descricao = x.Descricao,
                valorPrevisto = x.ValorPrevisto.ToString("C", AppDefaults.CultureInfoDefault),
                descricaoParcela = string.IsNullOrEmpty(x.DescricaoParcela) ? "" : x.DescricaoParcela,
                diasVencidos = x.DiasVencidos,
                valorPago = x.ValorPago
            };
        }

        public JsonResult GridLoadContaCnabItem(string pessoaId)
        {
            try
            {
                return base.GridLoad(new Dictionary<string, string> { { "pessoaId eq", pessoaId } });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = string.Format("Ocorreu um erro ao carregar dados: {0}", ex.Message) }, JsonRequestBehavior.AllowGet);
            }
        }

        #region NotImplementedMethods

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

        public override JsonResult ListRenegociacaoRelacionamento(string contaFinanceiraId)
        {
            throw new NotImplementedException();
        }

        public override JsonResult GridLoadTitulosARenegociar(string renegociaoPessoaId)
        {
            throw new NotImplementedException();
        }

        public override string GetResourceDeleteTituloBordero(string id)
        {
            throw new NotImplementedException();
        }

        public override ActionResult ImprimirRecibo(Guid id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}