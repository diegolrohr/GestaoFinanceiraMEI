using Fly01.uiJS.Defaults;
using Fly01.Core.Api;
using Fly01.Core.Helpers;
using Fly01.Core.JQueryDataTable;
using Fly01.Core.SOAManager;
using Fly01.Core.VM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Fly01.Core.Presentation
{
    public abstract class WebBaseController<T> : Controller where T : DomainBaseVM
    {
        protected string ResourceName { get; set; }
        protected string ExpandProperties { get; set; }
        protected string APIEnumResourceName { get; set; }
        protected string AppEntitiesResourceName { get; set; }
        protected string AppViewModelResourceName { get; set; }


        #region BUSCA DE DADOS MASHUP

        public JsonResult GetZipCode(string zipCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(zipCode))
                {
                    return JsonResponseStatus.GetFailure("Busca de CEP: Os parâmetros informados não são válidos.");
                }

                dynamic dynamicObj = SOADataManager.BuscaCEP(zipCode, new SOAConnectionConfig(AppDefaults.MashupClientId, AppDefaults.MashupUser, AppDefaults.MashupPassword));
                if (dynamicObj != null)
                {
                    bool retorno = string.IsNullOrWhiteSpace(dynamicObj.ZipCode) &&
                        string.IsNullOrWhiteSpace(dynamicObj.Address) &&
                        string.IsNullOrWhiteSpace(dynamicObj.Neighborhood) &&
                        string.IsNullOrWhiteSpace(dynamicObj.City) &&
                        string.IsNullOrWhiteSpace(dynamicObj.State);

                    if (retorno)
                    {
                        return JsonResponseStatus.GetFailure("Busca de CEP: Os parâmetros informados não retornaram nenhum resultado válido.");
                    }
                }

                return JsonResponseStatus.GetJson(dynamicObj);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(string.Format("Busca de CEP: {0}", ex.Message));
            }
        }

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

        #endregion

        public K GetBranchInformation<K>()
        {
            //ver rota v1 e v2 branch
            var resourceById = string.Format("{0}/{1}", AppDefaults.GetResourceName(typeof(K)), "01");
            return RestHelper.ExecuteGetRequest<K>(AppDefaults.UrlGateway.Replace("/v2/", "/v1/"), resourceById, RestHelper.DefaultHeader, null);
        }

        public virtual ContentResult Functions(string fns)
        {
            string content = fns.Split(',')
                .ToList()
                .Aggregate(string.Empty, (current, function) => current + RenderRazorViewToString("Functions/_" + function));

            content = content.Replace("<script>", "").Replace("</script>", "");
            return Content(content, "text/javascript");
        }

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

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization =
                filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            if (!skipAuthorization)
            {
                base.OnAuthorization(filterContext);
            }
        }

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

        /// <summary>
        /// Função responsável por carregar as depencias do entidade
        /// Isso é necessário quando a entidade em questão possui combos,
        /// para isso utiliza-se do ViewBag.
        /// </summary>
        protected virtual void LoadDependence() { }

        #region Views Methods

        public virtual ActionResult Index()
        {
            return View();
        }

        //public virtual ActionResult List()
        //{
        //    return PartialView("_List");
        //}

        public virtual ActionResult Create()
        {
            return View("Create");
        }

        public virtual ActionResult Edit(Guid id)
        {
            return View("Edit", id);
        }

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

        #endregion

        private static List<BatchVM> batchTasks = new List<BatchVM>();

        private string GetOrderDir(string dir)
        {
            dir = dir.Trim();
            if (dir.Equals("asc", StringComparison.InvariantCultureIgnoreCase))
                return string.Empty;
            return string.Format(" {0}", dir.Trim());
        }

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

        /// <summary>
        /// Função para identificar quais as colunas serão apresentadas na listagem
        /// Esta função é abstract, visto que somente será implementada no Controller que herda de BaseController
        /// </summary>
        /// <returns></returns>
        public abstract Func<T, object> GetDisplayData();

        public virtual Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var queryStringDefault = AppDefaults.GetQueryStringDefault();

            if (!string.IsNullOrEmpty(ExpandProperties))
                queryStringDefault.Add("$expand", ExpandProperties);

            return queryStringDefault;
        }

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
                                        valueFilter = string.Format("{0}'{1}'", ((APIEnumAttribute[])(specialType))[0].Get(APIEnumResourceName), item.Search.Value);
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

        #region SYSTEM VALUES

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

        //public List<KeyValueVM> GetSystemKeyValue(string systemValue)
        //{
        //    return this.ExecuteGetRequest<List<KeyValueVM>>(string.Format("systemvalue/{0}", systemValue));
        //}

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
        #endregion

        #region Importação

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

        [HttpPost]
        public string DecodeBase64(string content)
        {
            var data = Convert.FromBase64String(content);
            var text = Encoding.UTF8.GetString(data);
            return text;
        }

        #endregion

        public abstract ContentResult List();

        public abstract ContentResult Form();
    }
}