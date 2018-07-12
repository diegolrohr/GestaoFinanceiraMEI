using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Newtonsoft.Json;
using System.Reflection;
using Fly01.Core.Helpers;
using Fly01.uiJS.Defaults;
using Fly01.Core.SOAManager;
using System.Collections.Generic;
using Fly01.Core.Helpers.Attribute;
using Fly01.Core.Presentation.Commons;
using System.Web.Script.Serialization;
using Fly01.Core.ViewModels.Presentation;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.ViewModels;
using Fly01.Core.Config;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using System.Web.Configuration;
using Fly01.Core.Presentation.Controllers;
using Fly01.uiJS.Enums;

namespace Fly01.Core.Presentation
{
    public abstract class BaseController<T> : PrimitiveBaseController where T : DomainBaseVM
    {
        protected string ExpandProperties { get; set; }

        protected string ResourceName 
            => AppDefaults.GetResourceName(typeof(T));

        protected string AppEntitiesResourceName 
            => WebConfigurationManager.AppSettings["AppViewModelResourceName"];

        protected string AppViewModelResourceName 
            => WebConfigurationManager.AppSettings["AppEntitiesResourceName"];

        public virtual List<HtmlUIButton> GetListButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo", Position = HtmlUIButtonPosition.Main });

            return target;
        }

        public virtual List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main });

            return target;
        }

        public List<DataTableUIAction> GetActionsInGrid(List<DataTableUIAction> customWriteActions)
        {
            if (UserCanWrite)
                return customWriteActions;

            return new List<DataTableUIAction>();
        }

        [OperationRole(NotApply = true)]
        public JsonResult BuscaCEP(string cep)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cep))
                    return JsonResponseStatus.GetFailure("Busca de CEP: Os parâmetros informados não são válidos.");

                var buscaCepVM = SOADataManager.BuscaCEP(cep, new SOAConnectionConfig(AppDefaults.MashupClientId, AppDefaults.MashupUser, AppDefaults.MashupPassword));

                if (buscaCepVM != null)
                {
                    var queryString = AppDefaults.GetQueryStringDefault();
                    queryString.AddParam("$expand", "estado");
                    queryString.AddParam("$filter", $"(nome eq '{buscaCepVM.City}' and (estado/sigla eq '{buscaCepVM.State}') or estado/nome eq '{buscaCepVM.State}')");

                    var data = RestHelper.ExecuteGetRequest<ResultBase<CidadeVM>>(AppDefaults.GetResourceName(typeof(CidadeVM)), queryString).Data.FirstOrDefault();

                    buscaCepVM.StateId = data.EstadoId.ToString();
                    buscaCepVM.CityId = data.Id.ToString();

                    return JsonResponseStatus.GetJson(new { success = true, endereco = buscaCepVM });
                }
                else
                    return JsonResponseStatus.GetFailure("Busca de CEP: Os parâmetros informados não retornaram nenhum resultado válido.");
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(string.Format("Busca de CEP: {0}", ex.Message));
            }
        }

        [OperationRole(NotApply = true)]
        public ActionResult GetCodeData(string document)
        {
            dynamic response = new { };
            try
            {
                dynamic dynamicObj = GetDocumentData(document);

                if (!ValidatorUtils.ExistDynamicMember(dynamicObj.Data, "error"))
                {
                    ViewBag.CaptchaImage = dynamicObj.Data.captchaImage;
                    ViewBag.TipoPessoa = document.Length > 0 && document.Length <= 11 ? "F" : "J";
                    return PartialView("_DocumentData");
                }

                throw new Exception(!string.IsNullOrWhiteSpace(dynamicObj.Data.error) ? dynamicObj.Data.error : "Erro desconhecido. Favor tentar novamente.");
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.Message.Contains("does not contain a definition for 'captchaImage'"))
                {
                    errorMessage = "serviço indisponível no momento (DNCDCI).";
                }
                response = JsonResponseStatus.GetFailure(string.Format("Busca de CPF/CNPJ: {0}", errorMessage));
            }

            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        [OperationRole(NotApply = true)]
        public ActionResult GetDocumentData(string document = "", string code = "", string dataNascimento = "")
        {
            dynamic response = new { };

            try
            {
                dynamic dynamicObj = SOADataManager.GetReceitaFederalData(Session, new SOAConnectionConfig(AppDefaults.MashupClientId, AppDefaults.MashupUser, AppDefaults.MashupPassword), document, code, dataNascimento);

                if (ValidatorUtils.ExistDynamicMember(dynamicObj, "error"))
                {
                    throw new Exception(!string.IsNullOrWhiteSpace(dynamicObj.Data.error) ? dynamicObj.Data.error : "Erro desconhecido. Favor tentar novamente.");
                }

                if (!string.IsNullOrWhiteSpace(code))
                {
                    if (ValidatorUtils.ExistDynamicMember(dynamicObj, "error"))
                    {
                        throw new Exception(!string.IsNullOrWhiteSpace(dynamicObj.Data.error) ? dynamicObj.Data.error : "Erro desconhecido. Favor tentar novamente.");
                    }

                    bool retorno = string.IsNullOrWhiteSpace(dynamicObj.RazaoSocial) &&
                        string.IsNullOrWhiteSpace(dynamicObj.NomeFantasia) &&
                        string.IsNullOrWhiteSpace(dynamicObj.CEP) &&
                        string.IsNullOrWhiteSpace(dynamicObj.Endereco) &&
                        string.IsNullOrWhiteSpace(dynamicObj.Bairro) &&
                        string.IsNullOrWhiteSpace(dynamicObj.Numero) &&
                        string.IsNullOrWhiteSpace(dynamicObj.Complemento) &&
                        string.IsNullOrWhiteSpace(dynamicObj.Estado) &&
                        string.IsNullOrWhiteSpace(dynamicObj.Cidade);

                    if (retorno)
                    {
                        response = JsonResponseStatus.GetFailure("Busca de CPF/CNPJ: Código de verificação informado não confere.");
                    }
                }

                response = JsonResponseStatus.GetJson(dynamicObj);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.Message.Contains("does not contain a definition for 'captchaImage'"))
                {
                    errorMessage = "serviço indisponível no momento (DNCDCI).";
                }
                response = JsonResponseStatus.GetFailure(string.Format("Busca de CPF/CNPJ: {0}", errorMessage));
            }

            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        [OperationRole(NotApply = true)]
        public ManagerEmpresaVM GetDadosEmpresa() 
            => ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);

        [OperationRole(NotApply = true)]
        public ContentResult EmConstrucao(string history)
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory() { Default = history },
                Header = new HtmlUIHeader()
                {
                    Title = "Opção indisponível",
                    Buttons = new List<HtmlUIButton>()
                },
                UrlFunctions = ""
            };

            cfg.Content.Add(new FormUI()
            {
                Elements = new List<BaseUI>()
                {
                    new LabelSetUI()
                    {
                        Class = "col s12",
                        Id = "underconstruction",
                        Name = "underconstruction",
                        Label = "O recurso está em desenvolvimento."
                    }
                },
                Class = "col s12"
            });

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(NotApply = true)]
        public virtual ContentResult Functions(string fns)
        {
            string content = fns.Split(',')
                .ToList()
                .Aggregate(string.Empty, (current, function) => current + RenderRazorViewToString("Functions/_" + function));

            content = content.Replace("<script>", "").Replace("</script>", "");
            return Content(content, "text/javascript");
        }

        [OperationRole(NotApply = true)]
        private string RenderRazorViewToString(string viewName)
        {
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);

                if (viewResult.View == null) return string.Empty;
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        [OperationRole(NotApply = true)]
        public List<T> GetAll(string order = "", string filterField = "", string filterValue = "")
        {
            Dictionary<string, string> queryStringRequest = AppDefaults.GetQueryStringDefault(filterField, filterValue, AppDefaults.MaxRecordsPerPageAPI);
            int page = 1;
            queryStringRequest.AddParam("page", page.ToString());

            if (!string.IsNullOrWhiteSpace(order))
                queryStringRequest.AddParam("order", order);

            string resource = AppDefaults.GetResourceName(typeof(T));
            var items = new List<T>();
            bool hasNextRecord = true;
            while (hasNextRecord)
            {
                queryStringRequest.AddParam("page", page.ToString());
                var response = RestHelper.ExecuteGetRequest<ResultBase<T>>(resource, queryStringRequest);
                items.AddRange(response.Data);
                hasNextRecord = response.HasNext && response.Data.Count > 0;
                page++;
            }

            return items;
        }

        [OperationRole(NotApply = true)]
        public List<T> GetAll(string resourceName, int maxRecords, string order = "", string filterField = "", string filterValue = "", Dictionary<string, string> querystring = null)
        {
            var queryStringRequest = new List<KeyValuePair<string, string>>();
            queryStringRequest.AddRange(AppDefaults.GetQueryStringDefault(filterField, filterValue, maxRecords));
            if (querystring != null)
            {
                queryStringRequest.AddRange(querystring);
            }

            var page = 1;
            queryStringRequest.AddParam("page", page.ToString());

            if (!string.IsNullOrWhiteSpace(order))
                queryStringRequest.AddParam("order", order);

            var items = new List<T>();
            var hasNextRecord = true;
            while (hasNextRecord)
            {
                queryStringRequest.AddParam("page", page.ToString());
                var response = RestHelper.ExecuteGetRequest<ResultBase<T>>(resourceName, queryStringRequest.ToDictionary());
                items.AddRange(response.Data);
                hasNextRecord = response.HasNext && response.Data.Count > 0;
                page++;
            }

            return items;
        }

        protected virtual void LoadDependence() { }

        [OperationRole(NotApply = true)]
        public ActionResult NotAllow(string routeDescription)
            => View(viewName: "NotAllow", model: routeDescription);

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public virtual ActionResult Index() 
            => View();

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public virtual ActionResult Create()
        {
            return View("Create");
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public virtual ActionResult Edit(Guid id)
        {
            return View("Edit", id);
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public virtual JsonResult Delete(Guid id)
        {
            try
            {
                RestHelper.ExecuteDeleteRequest(String.Format("{0}/{1}", ResourceName, id));
                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Delete);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public virtual JsonResult Create(T entityVM)
        {
            try
            {
                var postResponse = RestHelper.ExecutePostRequest(ResourceName, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
                T postResult = JsonConvert.DeserializeObject<T>(postResponse);
                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, postResult.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public virtual JsonResult Edit(T entityVM)
        {
            try
            {
                var resourceNamePut = $"{ResourceName}/{entityVM.Id}";
                RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Edit));

                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Edit);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        private static List<BatchVM> batchTasks = new List<BatchVM>();

        private string GetOrderDir(string dir)
        {
            dir = dir.Trim();
            if (dir.Equals("asc", StringComparison.InvariantCultureIgnoreCase))
                return string.Empty;
            return string.Format(" {0}", dir.Trim());
        }

        [OperationRole(NotApply = true)]
        public virtual ContentResult Json(Guid id)
        {
            try
            {
                T entity = Get(id);
                var x = Content(JsonConvert.SerializeObject(entity, JsonSerializerSetting.Front), "application/json");
                return x;
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return Content(JsonConvert.SerializeObject(JsonResponseStatus.GetFailure(error.Message).Data), "application/json");
            }
        }

        protected T Get(Guid id)
        {
            string resourceName = AppDefaults.GetResourceName(typeof(T));
            string resourceById = String.Format("{0}/{1}", ResourceName, id);

            if (string.IsNullOrEmpty(ExpandProperties))
            {
                return RestHelper.ExecuteGetRequest<T>(resourceById);
            }
            else
            {
                var queryString = new Dictionary<string, string> {
                    { "$expand", ExpandProperties }
                };
                return RestHelper.ExecuteGetRequest<T>(resourceById, queryString);
            }
        }

        public abstract Func<T, object> GetDisplayData();

        public virtual Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var queryStringDefault = AppDefaults.GetQueryStringDefault();

            if (!string.IsNullOrEmpty(ExpandProperties))
                queryStringDefault.Add("$expand", ExpandProperties);

            return queryStringDefault;
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public virtual JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);

            try
            {
                var gridParams = new Dictionary<string, string>();
                gridParams.AddParam("$count", "true");

                if (param.Length > 0)
                    gridParams.AddParam("$top", param.Length.ToString());

                if (param.Order != null)
                {
                    var paramOrder = param.Order.FirstOrDefault();

                    if (paramOrder != null)
                        gridParams.AddParam("$orderby", string.Format("{0} {1}", param.Columns[paramOrder.Column].Data.Replace("_", "/"), GetOrderDir(paramOrder.Dir)));
                }

                if (param.Columns != null)
                {
                    foreach (JQueryDataTableParamsColumn item in param.Columns)
                    {
                        if ((item.Searchable) && (!string.IsNullOrWhiteSpace(item.Search.Value)) && (!string.IsNullOrWhiteSpace(item.Data)))
                        {
                            PropertyInfo propertyInfo = null;
                            var valueFilter = string.Empty;
                            var filterValid = false;
                            var filterField = item.Data;
                            bool dateTimeRange = false;

                            propertyInfo = typeof(T).GetProperties().FirstOrDefault(x => x.Name.Equals(
                                (filterField.IndexOf("_", StringComparison.Ordinal) > -1)
                                ? filterField.Substring(0, filterField.IndexOf("_", StringComparison.Ordinal))
                                : filterField, StringComparison.InvariantCultureIgnoreCase));

                            if (propertyInfo != null)
                            {
                                bool mustCompareAsEqual = false;

                                item.Search.Value = ODataStringFormater.GetString(item.Search.Value);

                                if (propertyInfo.PropertyType == typeof(string))
                                {
                                    var specialType = propertyInfo.GetCustomAttributes(typeof(APIEnumAttribute));
                                    mustCompareAsEqual = specialType.Any();

                                    if (mustCompareAsEqual)
                                        valueFilter = string.Format("{0}'{1}'", ((APIEnumAttribute[])(specialType))[0].Get(AppDefaults.APIEnumResourceName), item.Search.Value);
                                    else
                                        valueFilter = item.Search.Value;

                                    filterValid = true;
                                }

                                if ((propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?)) && item.Search.Value.IsDate())
                                {
                                    valueFilter = item.Search.Value;
                                    filterValid = dateTimeRange = true;
                                    mustCompareAsEqual = true;
                                }

                                if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
                                {
                                    int intValue;
                                    var validInt = int.TryParse(item.Search.Value, out intValue);

                                    if (!intValue.Equals(default(int)) && validInt)
                                    {
                                        valueFilter = item.Search.Value;
                                        filterValid = mustCompareAsEqual = true;
                                    }
                                }

                                if ((propertyInfo.PropertyType == typeof(double) || propertyInfo.PropertyType == typeof(double?)))
                                {
                                    double doubleValue;
                                    var validDouble = double.TryParse(item.Search.Value, out doubleValue);

                                    //var doubleValue = Convert.ToDouble(item.Search.Value);

                                    if (!doubleValue.Equals(default(double)) && validDouble)
                                    {
                                        valueFilter = doubleValue.ToString().Replace(",", ".");
                                        filterValid = true;
                                    }

                                    mustCompareAsEqual = true;
                                }

                                if (propertyInfo.PropertyType.BaseType == typeof(DomainBaseVM))
                                {
                                    item.Data = item.Data.Replace("_", "/");
                                    valueFilter = item.Search.Value;

                                    filterValid = true;
                                }

                                if (filterValid)
                                    gridParams.AddParamAndUpdate("$filter", Extensions.ReturnFilteredValue(item.Data, valueFilter, mustCompareAsEqual, dateTimeRange));
                            }
                        }
                    }
                }

                if (param.Start > 0)
                    gridParams.AddParam("$skip", param.Start.ToString());

                if (filters != null && filters.Any())
                {
                    filters.Remove("action");
                    filters.Remove("controller");
                    if (filters.Any())
                    {// se ainda tem algo
                        gridParams.AddParamAndUpdate("$filter", Extensions.ReturnProcessFilter(filters));
                    }
                }

                //GetQueryStringDefaultGridLoad().ForEach(x => gridParams.AddParamAndUpdate(x.Key, x.Value));
                foreach (var item in GetQueryStringDefaultGridLoad())
                {
                    gridParams.AddParamAndUpdate(item.Key, item.Value);
                }

                var responseGrid = RestHelper.ExecuteGetRequest<ResultBase<T>>(ResourceName, gridParams);
                return Json(new
                {
                    recordsTotal = responseGrid.Total,
                    recordsFiltered = responseGrid.Total,
                    data = responseGrid.Data.Select(GetDisplayData())
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    queryStringFilter = string.Empty,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new { },
                    success = false,
                    message = string.Format("Ocorreu um erro ao carregar dados: {0}", ex.Message)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        private Dictionary<string, List<SystemValueVM>> _systemValues = null;

        public Dictionary<string, List<SystemValueVM>> SystemValues
        {
            get
            {
                if (_systemValues == null)
                    _systemValues = new Dictionary<string, List<SystemValueVM>>(StringComparer.InvariantCultureIgnoreCase);
                return _systemValues;
            }

            set
            {
                _systemValues = value;
            }
        }

        public List<SelectListItem> GetComboValues(string systemValue, bool fieldStruct = false, bool defaultValue = false)
        {
            List<SelectListItem> listReturn = new List<SelectListItem>();

            List<KeyValueVM> items = GetSystemKeyValue(systemValue, fieldStruct);
            if (items != null && items.Count > 0)
                listReturn.AddRange(items.Select(x => new SelectListItem { Text = x.Value, Value = x.Key, Selected = false }));

            if (defaultValue)
                listReturn.Insert(0, new SelectListItem { Text = "Selecione...", Value = "", Selected = false });

            return listReturn;
        }

        public List<KeyValueVM> GetSystemKeyValue(string systemValue, bool fieldStruct = false)
        {
            string[] strValue = systemValue.Split('_');
            string systemEntity = strValue[0];
            string entityProperty = strValue[1];

            if (fieldStruct)
                systemEntity = string.Format("{0}.{1}", systemEntity, entityProperty);

            if (!this.SystemValues.ContainsKey(systemEntity))
            {
                List<SystemValueVM> result = null;
                if (fieldStruct)
                    result = new List<SystemValueVM> { new SystemValueVM { Name = entityProperty, Values = GetSystemKeyValue(systemEntity) } };
                else
                    result = GetSystemEntityValues(systemEntity);

                this.SystemValues.Add(systemEntity, result);
            }

            return this.SystemValues[systemEntity].FirstOrDefault(x => x.Name.Equals(entityProperty, StringComparison.InvariantCultureIgnoreCase)).Values;
        }

        private List<KeyValueVM> GetSystemKeyValue(string systemEntity)
        {
            return GetSystemEntityValuesAPI<KeyValueVM>(systemEntity);
        }

        private List<SystemValueVM> GetSystemEntityValues(string systemEntity)
        {
            return GetSystemEntityValuesAPI<SystemValueVM>(systemEntity);
        }

        private List<TReturn> GetSystemEntityValuesAPI<TReturn>(string systemEntity)
        {
            return RestHelper.ExecuteGetRequest<List<TReturn>>(string.Format("systemvalue/{0}", systemEntity));
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult ImportCsv(string entity, string defaultFields = null)
        {
            try
            {
                ImportHelper.appViewModelName = AppViewModelResourceName;
                ImportHelper.appEntitiesName = AppEntitiesResourceName;

                var successRead = 0;
                var totalFiles = Request.Files.Count;

                var batchVm = new BatchVM
                {
                    Items = new List<BatchItemVM>
                    {
                        new BatchItemVM
                        {
                            Entity = entity.ToLower(),
                            Operation = "csv",
                            Entries = new List<BatchEntryVM>()
                        }
                    }
                };

                for (var i = 0; i < totalFiles; i++)
                {
                    var fileContent = Request.Files[i];
                    if (fileContent == null || fileContent.ContentLength <= 0) continue;

                    var newContent = ImportHelper.RenameColumns(fileContent, entity);
                    var newContentBytes = Encoding.ASCII.GetBytes(newContent);
                    var fileBase64 = Convert.ToBase64String(newContentBytes);


                    batchVm.Items[0].Entries.Add(new BatchEntryVM
                    {
                        ItemId = DateTime.Now.Ticks.ToString() + (i + 1),
                        Data = new BatchDataVM
                        {
                            FileContent = fileBase64,
                            Md5 = Base64Helper.CalculaMD5Hash(fileBase64)
                        },
                        DefaultFields = defaultFields != null
                                            ? new JavaScriptSerializer().DeserializeObject(defaultFields)
                                            : null
                    });

                    successRead++;
                }

                if (successRead > 0)
                {
                    var responsePost = RestHelper.ExecutePostRequest<PostResponseAPI>("batch", JsonConvert.SerializeObject(batchVm), timeout: 1200);
                    return Json(BatchStatus(responsePost.Id));
                }

                if (successRead == 0 && totalFiles > 0)
                {
                    return JsonResponseStatus.GetFailure(string.Format("Não foi possível ler {0}",
                                                                       totalFiles > 1
                                                                            ? "todos os " + totalFiles + " arquivos"
                                                                            : "o arquivo"));
                }

                return JsonResponseStatus.GetFailure("Favor selecionar o arquivo.");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public ActionResult BatchStatus(string id)
        {
            try
            {
                var responseBatch = RestHelper.ExecuteGetRequest<BatchVM>(string.Format("batch/{0}", id));
                responseBatch.Id = id;
                var retJson = Json(new JavaScriptSerializer().DeserializeObject(JsonConvert.SerializeObject(responseBatch)));

                return retJson;
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }

        [OperationRole(NotApply = true)]
        [HttpPost]
        public string DecodeBase64(string content)
        {
            var data = Convert.FromBase64String(content);
            var text = Encoding.UTF8.GetString(data);
            return text;
        }
        
        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public abstract ContentResult List();

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public abstract ContentResult Form();

        [OperationRole(NotApply = true)]
        [HttpGet]
        public virtual ActionResult Download(string fileName)
        {
            if (Session[fileName] != null)
            {
                byte[] data = Convert.FromBase64String(Session[fileName].ToString());
                Session.Remove(fileName);
                return File(data, "application/octet-stream", fileName);
            }
            else
            {
                return new HttpNotFoundResult("O arquivo solicitado não está disponível para download.");
            }
        }

        [OperationRole(NotApply = true)]
        [HttpGet]
        public virtual ActionResult DownloadPDF(string fileName)
        {
            if (Session[fileName] != null)
            {
                byte[] data = Convert.FromBase64String(Session[fileName].ToString());
                Session.Remove(fileName);
                return File(data, "application/pdf", fileName);
            }
            else
            {
                return new HttpNotFoundResult("O PDF solicitado não está disponível para download.");
            }
        }

        [OperationRole(NotApply = true)]
        [HttpGet]
        public virtual ActionResult DownloadXMLString(string fileName)
        {
            if (Session[fileName] != null)
            {
                byte[] data = Convert.FromBase64String(Base64Helper.CodificaBase64(Session[fileName].ToString()));
                Session.Remove(fileName);
                return File(data, "text/xml", fileName);
            }
            else
            {
                return new HttpNotFoundResult("O XML solicitado não está disponível para download.");
            }
        }
    }
}