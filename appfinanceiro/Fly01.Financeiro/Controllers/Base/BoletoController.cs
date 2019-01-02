using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Mensageria;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Financeiro.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers.Base
{
    public abstract class BoletoController<TEntity> : BaseController<TEntity> where TEntity : DomainBaseVM
    {
        private string GetBoletoAsString(Guid? contaReceberId, Guid? contaBancariaId, bool reimprimeBoleto = false)
        {
            var boletoBancario = RestHelper.ExecuteGetRequest<string>("boleto/imprimeBoleto", new Dictionary<string, string>
            {
                { "contaReceberId", contaReceberId.ToString() }, { "contaBancariaId", contaBancariaId.ToString() }
            });

            if (boletoBancario == null)
            {
                throw new Exception("Não foi possível gerar boleto.");
            }

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
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message.Replace("\r\n", " "));
            }
        }

        [OperationRole(NotApply = true)]
        [HttpGet]
        public JsonResult GerarBoletoEnviaEmail(Guid contaReceberId, Guid contaBancariaId, bool reimprimeBoleto = false, string email = "", string assunto = "", string mensagem = "")
        {
            try
            {
                var boleto = GetBoletoAsString(contaReceberId, contaBancariaId, reimprimeBoleto);
                var stream = new MemoryStream(ConvertHTMLToPDF.GerarArquivoPDF(boleto));
                Mail.Send(GetDadosEmpresa().NomeFantasia, email, assunto, mensagem, stream);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [OperationRole(NotApply = true)]
        [HttpPost]
        public JsonResult GerarBoletoEnviaEmailsSelecionados(List<string> idsCnab, bool reimprimeBoleto = false, string email = "", string assunto = "", string mensagem = "")
        {
            try
            {
                List<string> boletosGerados = new List<string>();
                var boletos = GetCnab(idsCnab.Select(Guid.Parse).ToList());

                foreach (var item in boletos)
                {
                    var boleto = GetBoletoAsString(item.ContaReceberId, item.ContaBancariaCedenteId, reimprimeBoleto);
                    boletosGerados.Add(boleto);
                }

                var allBoletos = string.Join(Environment.NewLine + Environment.NewLine, boletosGerados);
                var stream = new MemoryStream(ConvertHTMLToPDF.GerarArquivoPDF(allBoletos));
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
                {
                    boletoTemVinculo = true;
                }
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
                {
                    listaCnab.Add(restResponse);
                }
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

        //[OperationRole(NotApply = true)]
        //[HttpGet]
        //public JsonResult VerificarBoletosPessoa(string id)
        //{
        //    var guids = new List<Guid>
        //    {
        //        new Guid(id)
        //    };

        //    var cnab = GetCnab(guids);
        //    var idPessoa = cnab.FirstOrDefault().PessoaId;
        //    var queryString = new Dictionary<string, string>
        //        {
        //            { "IdPessoa", idPessoa.ToString()},
        //            { "pageSize", "10"}
        //        };

        //    var restResponse = RestHelper.ExecuteGetRequest<List<CnabVM>>("cnab", queryString);
            
        //    return null;
        //}

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