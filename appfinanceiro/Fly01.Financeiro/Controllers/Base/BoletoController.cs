﻿using System;
using Fly01.Core;
using System.Linq;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Financeiro.ViewModel;
using System.Collections.Generic;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.ViewModels.Presentation;
using Fly01.Core.Mensageria;
using System.IO;

namespace Fly01.Financeiro.Controllers.Base
{
    public abstract class BoletoController<TEntity> : BaseController<TEntity> where TEntity : DomainBaseVM
    {
        private string GetBoletoAsString(Guid contaReceberId, Guid contaBancariaId, bool reimprimeBoleto = false)
        {
            var boletoBancario = RestHelper.ExecuteGetRequest<string>("boleto/imprimeBoleto", new Dictionary<string, string>
            {
                { "contaReceberId", contaReceberId.ToString() }, { "contaBancariaId", contaBancariaId.ToString() }
            });

            if (boletoBancario == null) throw new Exception("Não foi possível gerar boleto.");

            return boletoBancario;
        }

        [OperationRole(NotApply = true)]
        [HttpGet]
        public JsonResult ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId, bool reimprimeBoleto = false)
        {
            try
            {
                var boleto = GetBoletoAsString(contaReceberId, contaBancariaId, reimprimeBoleto);

                return Json(new { success = true, message = boleto }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Ocorreu um erro ao gerar boleto: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        [OperationRole(NotApply = true)]
        [HttpGet]
        public JsonResult GerarBoletoEnviaEmail(Guid contaReceberId, Guid contaBancariaId, bool reimprimeBoleto = false, string email = "", string assunto = "", string mensagem = "")
        {
            try
            {
                var boleto = GetBoletoAsString(contaReceberId, contaBancariaId, reimprimeBoleto);
                var stream = new MemoryStream(ConvertHTMLToPDF.Convert(boleto));
                Mail.Send(GetDadosEmpresa().NomeFantasia, email, assunto, mensagem, stream);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [OperationRole(NotApply = true)]
        [HttpGet]
        public JsonResult ValidaBoletoJaGeradoParaOutroBanco(Guid contaReceberId, Guid contaBancariaId)
        {
            bool boletoTemVinculo = false;

            var cnab = GetCnab($"contaReceberId eq {contaReceberId}");
            var bancoId = GetIdBanco($"id eq {contaBancariaId}");
            var arquivoRemessaId = cnab.Count > 0 ? cnab.FirstOrDefault().ArquivoRemessaId : null;

            if (cnab.Count > 0)
            {
                if (cnab.Any(x => x.ContaBancariaCedente.BancoId != bancoId))
                    boletoTemVinculo = true;
            }

            return Json(new { success = boletoTemVinculo, data = arquivoRemessaId }, JsonRequestBehavior.AllowGet);
        }

        protected List<CnabVM> GetCnab(List<Guid> idsBoletos)
        {
            var listaCnab = new List<CnabVM>();

            foreach (var item in idsBoletos)
            {
                var queryString = new Dictionary<string, string>
                {
                    { "Id", item.ToString()},
                    { "pageSize", "10"}
                };

                var restResponse = RestHelper.ExecuteGetRequest<CnabVM>("cnab", queryString);
                if (restResponse != null)
                    listaCnab.Add(restResponse);
            }

            return listaCnab;
        }

        protected List<CnabVM> GetCnab(string filter)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", filter);
            queryString.AddParam("$expand", "contaReceber($expand=pessoa),contaBancariaCedente($expand=banco)");

            var boletos = RestHelper.ExecuteGetRequest<ResultBase<CnabVM>>("cnab", queryString);

            return boletos.Data;
        }

        protected List<DadosArquivoRemessaVM> GetListaBoletos(List<Guid> idsCnabToSave)
        {
            var queryString = new Dictionary<string, string>()
            {
                { "listIdCnab", idsCnabToSave
                    .Select(g => g.ToString())
                    .Aggregate((working, next) => working + "," + next)
                }
            };

            return RestHelper.ExecuteGetRequest<List<DadosArquivoRemessaVM>>("boleto/getListaBoletos", queryString);
        }

        private Guid? GetIdBanco(string filter)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", filter);

            return RestHelper.ExecuteGetRequest<ResultBase<ContaBancariaVM>>("contaBancaria", queryString).Data.FirstOrDefault().BancoId;
        }

        [OperationRole(NotApply = true)]
        public static List<BancoVM> GetBancosEmiteBoletos()
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"emiteBoleto eq true");

            return RestHelper.ExecuteGetRequest<ResultBase<BancoVM>>(AppDefaults.GetResourceName(typeof(BancoVM)), queryString).Data;
        }
    }
}