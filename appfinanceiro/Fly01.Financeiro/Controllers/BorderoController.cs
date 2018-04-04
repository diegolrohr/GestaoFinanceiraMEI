using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web.Mvc;
using Fly01.Financeiro.Controllers.Base;
using Fly01.Financeiro.Entities.ViewModel;
using Fly01.Core;
using Fly01.Core.Api;
using Fly01.Core.Helpers;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Controllers
{
    public class BorderoController : BaseController<BorderoVM>
    {
        protected override void LoadDependence()
        {
            ViewBag.PrintingLocation = SystemValueHelper.Get("Bank_PrintingLocation", true, false);
        }

        public override Func<BorderoVM, object> GetDisplayData()
        {
            return x => new
            {
                x.Id,
                DataInicial = "",
                DataFinal = "",
                x.NomeBanco,
                x.Convenio,
                Pessoa = "",
                Valor = "",
                Situacao = x.SituacaoCNAB,
                x.InstrucaoCNAB1,
                x.InstrucaoCNAB2,
                x.TipoJuros,
                TaxaJuros = x.PercentTaxaJuros,
                x.DiasJuros,
                TaxaMulta = x.PercentTaxaMulta,
                x.DiasMulta,
                x.TipoDesconto,
                x.ValorDesconto,
                x.Especie,
                x.SacadorAvalista,
                CPFCNPJ = x.CnpjCpf,
                x.LocalImpressao,
                CarteiraCNAB = x.Carteira,
                x.TipoProtesto,
                x.DiasProtesto,
                x.MensagemReciboSacado,
                x.MensagemFichaCompensacao,
                //StatusAPI = x.Status,
                //SubtitleCode = x.SubtitleCode,
                Status = x.SubtitleCode.ToEnum<BorderoStatusVM>().GetTitle(),
                x.Agencia,
                x.Conta
                //Numero = x.Id,
                //Descricao = String.Format("{0} - {1}", x.Id, x.NomeBanco),
                //Agencia = x.Agencia,
            };
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            Dictionary<string, string> customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("justfields", "status,subtitleCode,bankName,bankAgency,bankAccount");

            return customFilters;
        }

        [HttpPost]
        public override JsonResult Create(BorderoVM entityVM)
        {
            if (entityVM.AccountsIds != null)
            {
                entityVM.AccountReceivableIds = new List<string>();
                entityVM.AccountsIds.Split(',').ToList().ForEach(item => entityVM.AccountReceivableIds.Add(item));
            }

            return base.Create(entityVM);
        }

        public override ActionResult Edit(Guid id)
        {
            BorderoVM entityVM = Get(id);

            LoadDependence();

            foreach (var item in entityVM.AccountReceivableIds)
            {
                entityVM.AccountsIds += String.IsNullOrWhiteSpace(entityVM.AccountsIds) ? item : "," + item;
            }

            return PartialView("_Edit", entityVM);
        }

        /// <summary>
        /// Método responsável por apresentar a View de seleção de filtros
        /// para a geração do CNAB
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarCNAB()
        {
            CNABEmissionVM model = new CNABEmissionVM
            {
                InitialBorderoId = "000001",
                FinalBorderoId = "000001",
                ParametersBankId = string.Empty
            };

            return PartialView("_GerarCNAB", model);
        }

        [HttpPost]
        public JsonResult GerarCNAB(CNABEmissionVM entityVM)
        {
            try
            {
                JsonResult jsonResult = new JsonResult();

                #region Para facilitar - Usuário seleciona o banco e no POST é enviado o ParametersBankId
                string bankId = entityVM.ParametersBankId;
                Dictionary<string, string> queryStringFilterParametersBank = new Dictionary<string, string>();
                queryStringFilterParametersBank.Add("bankId", bankId);

                ParametrosBancariosVM parametersBankVM = RestHelper.ExecuteGetRequest<ResultBase<ParametrosBancariosVM>>(AppDefaults.GetResourceName(typeof(ParametrosBancariosVM)), queryStringFilterParametersBank).Data.FirstOrDefault();
                if (parametersBankVM == null)
                    throw new ApplicationException("A conta bancária selecionada não possui informado os parametros bancários.");
                entityVM.ParametersBankId = parametersBankVM.Id.ToString();
                #endregion

                string resource = AppDefaults.GetResourceName(typeof(CNABEmissionVM));

                FileVM response = RestHelper.ExecutePostRequest<FileVM>(resource, JsonConvert.SerializeObject(entityVM));
                if (response.FileMD5 == Base64Helper.CalculaMD5Hash(response.FileContent))
                {
                    string guidArquivo = Guid.NewGuid().ToString();
                    Session[guidArquivo] = Convert.FromBase64String(response.FileContent);
                    jsonResult.Data = new { success = true, fileGuid = guidArquivo, fileName = response.FileName };
                    return jsonResult;
                }
                throw new ApplicationException("Falha no download do arquivo. Hash do Arquivo Inválido");
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public ActionResult DownloadFile(string fileGuid, string fileName)
        {
            if (Session[fileGuid] != null)
            {
                byte[] data = Session[fileGuid] as byte[];
                return File(data, MediaTypeNames.Application.Octet, fileName);
            }
            return new EmptyResult();
        }

        public ActionResult RetornoCNAB()
        {
            return PartialView("_RetornoCNAB", new CNABReturnVM());
        }

        [HttpPost]
        public JsonResult RetornoCNAB(string bankId)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var fileContent = Request.Files[0];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        byte[] buffer = new byte[fileContent.InputStream.Length];
                        fileContent.InputStream.Seek(0, SeekOrigin.Begin);
                        fileContent.InputStream.Read(buffer, 0, Convert.ToInt32(fileContent.InputStream.Length));
                        MemoryStream memoryStream = new MemoryStream(buffer);

                        string fileBase64 = Convert.ToBase64String(memoryStream.ToArray());

                        #region Para facilitar - Usuário seleciona o banco e no POST é enviado o ParametersBankId
                        Dictionary<string, string> queryStringFilterParametersBank = new Dictionary<string, string>();
                        queryStringFilterParametersBank.Add("bankId", bankId);

                        ParametrosBancariosVM parametersBankVM = RestHelper.ExecuteGetRequest<ResultBase<ParametrosBancariosVM>>(AppDefaults.GetResourceName(typeof(ParametrosBancariosVM)), queryStringFilterParametersBank).Data.FirstOrDefault();
                        if (parametersBankVM == null)
                            throw new ApplicationException("A conta bancária selecionada não possui informado os parametros bancários.");
                        string parametersBankId = parametersBankVM.Id.ToString();
                        #endregion

                        CNABReturnVM entityVM = new CNABReturnVM
                        {
                            FileName = fileContent.FileName,
                            FileMD5 = Base64Helper.CalculaMD5Hash(fileBase64),
                            ParametersBankId = parametersBankId,
                            FileContent = fileBase64
                        };

                        string resource = AppDefaults.GetResourceName(typeof(CNABReturnVM));
                        RestHelper.ExecutePostRequest(resource, JsonConvert.SerializeObject(entityVM));
                        return JsonResponseStatus.GetSuccess("Arquivo importado com sucesso.");
                    }
                    return JsonResponseStatus.GetFailure("Não foi possível ler o arquivo.");
                }
                return JsonResponseStatus.GetFailure("Favor, selecionar o arquivo.");
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpPost]
        public JsonResult GetBoleto(string idTitulo)
        {
            try
            {
                JsonResult jsonResult = new JsonResult();

                string resource = String.Format("Boleto/{0}", idTitulo);

                FileVM response = RestHelper.ExecuteGetRequest<FileVM>(resource);
                if (response.FileMD5 == Base64Helper.CalculaMD5Hash(response.FileContent))
                {
                    string guidArquivo = Guid.NewGuid().ToString();
                    Session[guidArquivo] = Convert.FromBase64String(response.FileContent);
                    jsonResult.Data = new { success = true, fileGuid = guidArquivo, fileName = response.FileName };
                    return jsonResult;
                }
                throw new ApplicationException("Falha no download do arquivo. Hash do Arquivo Inválido");
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }
    }
}