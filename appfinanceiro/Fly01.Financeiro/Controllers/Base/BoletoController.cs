using System;
using Fly01.Core;
using System.Linq;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Newtonsoft.Json;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Financeiro.ViewModel;
using System.Collections.Generic;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.ViewModels.Presentation.Commons;
using Boleto2Net;
using Fly01.Core.ViewModels.Presentation;

namespace Fly01.Financeiro.Controllers.Base
{
    public abstract class BoletoController<TEntity> : BaseController<TEntity> where TEntity : DomainBaseVM
    {
        [HttpGet]
        public JsonResult ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId, bool reimprimeBoleto = false)
        {
            try
            {
                var boletoBancario = RestHelper.ExecuteGetRequest<string>("boleto/imprimeBoleto", new Dictionary<string, string>
                {
                    { "contaReceberId", contaReceberId.ToString() }, { "contaBancariaId", contaBancariaId.ToString() }
                });

                if (boletoBancario == null) throw new Exception("Não foi possível gerar boleto.");

                return Json(new { success = true, message = boletoBancario }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Ocorreu um erro ao gerar boleto: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ValidaBoletoJaGeradoParaOutroBanco(Guid contaReceberId, Guid contaBancariaId)
        {
            bool boletoTemVinculo = false;

            var cnab = GetCnab($"contaReceberId eq {contaReceberId}");
            var bancoId = GetIdBanco($"id eq {contaBancariaId}");
            var arquivoRemessaId = cnab.Count > 0 ? cnab.FirstOrDefault().ArquivoRemessaId : null;

            if (cnab.Count > 0)
            {
                //if (cnab.Any(x => x.ContaBancariaCedente.BancoId != bancoId))
                //    boletoTemVinculo = true;
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

        protected List<KeyValuePair<Guid?, BoletoBancario>> GetListaBoletos(List<Guid> idsCnabToSave)
        {
            var queryString = new Dictionary<string, string>()
            {
                { "listIdCnab", idsCnabToSave
                    .Select(g => g.ToString())
                    .Aggregate((working, next) => working + "," + next)
                }
            };

            return RestHelper.ExecuteGetRequest<List<KeyValuePair<Guid?, BoletoBancario>>>("boleto/getListaBoletos", queryString);
        }

        protected static Dictionary<string, string> GetQueryStringEmiteBoleto()
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"emiteBoleto eq true");
            return queryString;
        }

        protected static List<BancoVM> GetListBancos()
        {
            return RestHelper.ExecuteGetRequest<ResultBase<BancoVM>>(AppDefaults.GetResourceName(typeof(BancoVM)), GetQueryStringEmiteBoleto()).Data;
        }

        protected static void UpdateCnab(List<Guid> ids, ArquivoRemessaVM result)
        {
            var status = ((int)StatusCnab.AguardandoRetorno).ToString();

            ids.ForEach(x =>
            {
                var resource = $"cnab/{x}";
                RestHelper.ExecutePutRequest(resource, JsonConvert.SerializeObject(new
                {
                    arquivoRemessaId = result.Id,
                    status = status
                }));
            });
        }

        private List<CnabVM> GetContasReceber(Guid? idArquivo, int pageNo)
        {
            var queryString = new Dictionary<string, string>
            {
                { "arquivoRemessaId", idArquivo.ToString()},
                { "pageNo", pageNo.ToString() },
                { "pageSize", "10"}
            };

            var boletos = RestHelper.ExecuteGetRequest<PagedResult<CnabVM>>("cnab", queryString);

            return boletos.Data.Where(x => x.ArquivoRemessaId == idArquivo).ToList();
        }

        private Guid? GetIdBanco(string filter)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", filter);

            return RestHelper.ExecuteGetRequest<ResultBase<ContaBancariaVM>>("contaBancaria", queryString).Data.FirstOrDefault().BancoId;
        }
        
        public static List<BancoVM> GetBancosEmiteBoletos()
        {
            return RestHelper.ExecuteGetRequest<ResultBase<BancoVM>>(AppDefaults.GetResourceName(typeof(BancoVM)), GetQueryStringEmiteBoleto()).Data;
        }

        public void GerarArquivoPorBanco(List<Boleto2Net.Boleto> lstBoletos, out string nomeArquivo)
        {
            var boletos = new Boletos()
            {
                Banco = lstBoletos.FirstOrDefault().Banco
            };
            boletos.AddRange(lstBoletos);

            var arquivoRemessa = new ArquivoRemessa(lstBoletos.FirstOrDefault().Banco, ValidaDadosBancoVM.GetTipoCnab(lstBoletos.FirstOrDefault().Banco.Codigo), 1); // tem que avaliar os dados passados(tipoArquivo, NumeroArquivo)
            nomeArquivo = $"{lstBoletos.FirstOrDefault().Banco.Codigo}-{DateTime.Now.ToString("ddMMyyyyHHmmss")}";
            Session[nomeArquivo] = arquivoRemessa.GerarArquivoRemessa(boletos);
        }
    }
}