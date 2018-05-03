using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Financeiro.Controllers.Base;
using Fly01.Financeiro.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Estoque.Controllers
{
    public class CnabItemController : ContaFinanceiraController<ContaReceberVM, ContaFinanceiraBaixaVM, ContaFinanceiraRenegociacaoVM>
    {

        public CnabItemController()
        {
            //ExpandProperties = "produto($select=descricao,codigoProduto,valorCusto,saldoProduto,unidadeMedidaId),produto($expand=unidadeMedida)";
        }

        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

        public override Func<ContaReceberVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                statusEnum = x.StatusContaBancaria,
                statusContaBancaria = EnumHelper.SubtitleDataAnotation(typeof(StatusContaBancaria), x.StatusContaBancaria).Description,
                statusContaBancariaCssClass = EnumHelper.SubtitleDataAnotation(typeof(StatusContaBancaria), x.StatusContaBancaria).CssClass,
                statusContaBancariaNomeCompleto = EnumHelper.SubtitleDataAnotation(typeof(StatusContaBancaria), x.StatusContaBancaria).Value,
                contaFinanceiraRepeticaoPaiId = x.ContaFinanceiraRepeticaoPaiId,
                tipoPeriodicidade = x.TipoPeriodicidade,
                numero = x.Numero,
                pessoaId = x.PessoaId,
                dataEmissao = x.DataEmissao.ToString("dd/MM/yyyy"),
                dataVencimento = x.DataVencimento.ToString("dd/MM/yyyy"),
                descricao = x.Descricao,
                valorPrevisto = x.ValorPrevisto.ToString("C", AppDefaults.CultureInfoDefault),
                formaPagamento_descricao = "",
                descricaoParcela = string.IsNullOrEmpty(x.DescricaoParcela) ? "" : x.DescricaoParcela,
                categoria_descricao = "",
                pessoa_nome = "",
                saldo = x.Saldo.ToString("C", AppDefaults.CultureInfoDefault),
                saldoSemFormatacao = x.Saldo,
                diasVencidos = x.DiasVencidos,
                condicaoParcelamento_descricao = "",
                observacao = x.Observacao,
                repetir = x.Repetir,
                valorConciliado = x.Saldo.ToString("C", AppDefaults.CultureInfoDefault),
                NumeroRepeticoes = x.NumeroRepeticoes,
                valorPago = x.ValorPago,
                FormaPagamentoObject = x.FormaPagamento,
                Pessoa = x.Pessoa,
                dataVencimentoObject = x.DataVencimento
            };
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        public JsonResult GridLoadContaCnabItem(string pessoaId)
        {
            try
            {
                var filters = new Dictionary<string, string>
                {
                    { "pessoaId eq", pessoaId }
                };

                return base.GridLoad(filters);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = string.Format("Ocorreu um erro ao carregar dados: {0}", ex.Message) }, JsonRequestBehavior.AllowGet);
            }
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
    }
}