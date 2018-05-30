﻿using System;
using Fly01.Core;
using System.Web.Mvc;
using Fly01.Financeiro.ViewModel;
using System.Collections.Generic;
using Fly01.Financeiro.Controllers.Base;
using Fly01.Core.Helpers;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Estoque.Controllers
{
    public class CnabItemController : ContaFinanceiraController<ContaReceberVM, ContaFinanceiraBaixaVM, ContaFinanceiraRenegociacaoVM>
    {
        public CnabItemController() { }

        public override Func<ContaReceberVM, object> GetDisplayData()
        {
            return x => new
            {
                contaReceberId = x.Id.ToString(),
                numero = x.Numero,
                descricao = x.Descricao,
                dataVencimento = x.DataVencimento.ToString("dd/MM/yyyy"),
                statusContaBancaria = x.StatusContaBancaria,
                statusContaBancariaCssClass = EnumHelper.GetCSS(typeof(StatusContaBancaria), x.StatusContaBancaria),
                statusContaBancariaNomeCompleto = EnumHelper.GetValue(typeof(StatusContaBancaria), x.StatusContaBancaria),
                valorPrevisto = x.ValorPrevisto.ToString("C", AppDefaults.CultureInfoDefault),
                descricaoParcela = string.IsNullOrEmpty(x.DescricaoParcela) ? "" : x.DescricaoParcela,
                nossoNumero = x.Numero,
                valorPago = x.ValorPago
            };
        }

        public JsonResult GridLoadContaCnabItem(string pessoaId)
        {
            try
            {
                var filters = new Dictionary<string, string>()
                {
                    {
                        "pessoaId", $" eq {pessoaId} and ativo and (statusContaBancaria ne {AppDefaults.APIEnumResourceName}StatusContaBancaria'Pago')"
                    }
                };

                return base.GridLoad(filters);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = string.Format("Ocorreu um erro ao carregar dados: {0}", ex.Message) }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GridLoadContaCnabItemByArquivo(string arquivoId)
        {
            try
            {
                return base.GridLoad(new Dictionary<string, string> { { "pessoaId eq", arquivoId } });
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