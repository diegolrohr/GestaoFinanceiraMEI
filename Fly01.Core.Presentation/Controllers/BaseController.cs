using Fly01.Core.Config;
using Fly01.Core.Helpers;
using Fly01.Core.Helpers.Attribute;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.SOAManager;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fly01.Core.Presentation
{

    public abstract class BaseController<T> : PrimitiveBaseController where T : DomainBaseVM
    {

        public class ContentUIBase : ContentUI
        {
            public ContentUIBase(string sidebarUrl)
            {
                SidebarUrl = sidebarUrl;
            }
        }
        protected string ExpandProperties { get; set; }

        protected string SelectPropertiesList { get; set; }

        protected string SelectPropertiesForm { get; set; }

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
            {
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main });
                target.Add(new HtmlUIButton { Id = "saveNew", Label = "Salvar e Novo", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main });
            }

            return target;
        }

        public virtual List<DataTableUIAction> GetActionsInGrid(List<DataTableUIAction> customWriteActions)
        {
            if (UserCanWrite)
                return customWriteActions;

            return new List<DataTableUIAction> { new DataTableUIAction { OnClickFn = "fnVisualizar", Label = "Visualizar" } };
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
                    buscaCepVM.StateCodeIbge = data.Estado.CodigoIbge.ToString() ?? "";
                    buscaCepVM.CityCodeIbge = data.CodigoIbge.ToString();

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
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory() { Default = history },
                Header = new HtmlUIHeader()
                {
                    Title = "Opção indisponível",
                    Buttons = new List<HtmlUIButton>()
                },
                UrlFunctions = ""
            };

            cfg.Content.Add(new FormUI
            {
                Id = "fly01frm",
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
            => View("Create");

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public virtual ActionResult Edit(Guid id)
            => View("Edit", id);

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public virtual ActionResult View(Guid id)
            => View("View", id);

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

            if (string.IsNullOrEmpty(ExpandProperties) && string.IsNullOrEmpty(SelectPropertiesForm))
            {
                return RestHelper.ExecuteGetRequest<T>(resourceById);
            }
            else
            {
                var queryString = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(ExpandProperties))
                {
                    queryString.Add("$expand", ExpandProperties);
                }
                if (!string.IsNullOrEmpty(SelectPropertiesForm))
                {
                    queryString.Add("select", SelectPropertiesForm);
                }

                return RestHelper.ExecuteGetRequest<T>(resourceById, queryString);
            }
        }

        public abstract Func<T, object> GetDisplayData();

        public virtual Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var queryStringDefault = AppDefaults.GetQueryStringDefault();

            if (!string.IsNullOrEmpty(ExpandProperties))
                queryStringDefault.Add("$expand", ExpandProperties);
            if (!string.IsNullOrEmpty(SelectPropertiesList))
                queryStringDefault.Add("$select", SelectPropertiesList);

            return queryStringDefault;
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public virtual JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);
            var fileType = (Request.QueryString.AllKeys.Contains("fileType")) ? Request.QueryString.Get("fileType") : "";

            try
            {
                var gridParams = new Dictionary<string, string>();
                gridParams.AddParam("$count", "true");

                if (string.IsNullOrWhiteSpace(fileType))
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

                            var filterFieldSplit = filterField.Split('_');

                            //nome
                            var className = filterField;
                            propertyInfo = typeof(T).GetProperties().FirstOrDefault(x => x.Name.Equals(className, StringComparison.InvariantCultureIgnoreCase));

                            if (filterFieldSplit.Length == 2)
                            {
                                //pessoa_nome
                                className = filterFieldSplit[0];
                                propertyInfo = typeof(T).GetProperties().FirstOrDefault(x => x.Name.Equals(className, StringComparison.InvariantCultureIgnoreCase));
                            }
                            else if (filterFieldSplit.Length == 3)
                            {
                                //por enquanto ressolvido para buscar em até 2 níveis de navigation
                                //contaBancaria_pessoa_nome
                                className = filterFieldSplit[filterFieldSplit.Length - 2];
                                propertyInfo = typeof(T).GetProperties().FirstOrDefault(x => x.Name.Equals(filterFieldSplit[0], StringComparison.InvariantCultureIgnoreCase));

                                Type type = propertyInfo.PropertyType;
                                propertyInfo = type.GetProperties().FirstOrDefault(x => x.Name.Equals(className, StringComparison.InvariantCultureIgnoreCase));
                            }

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

                if (string.IsNullOrWhiteSpace(fileType))
                    if (param.Start > 0)
                        gridParams.AddParam("$skip", param.Start.ToString());

                if (filters != null && filters.Any())
                {
                    filters.Remove("action");
                    filters.Remove("controller");

                    if (filters.Any())
                    {
                        gridParams.AddParamAndUpdate("$filter", Extensions.ReturnProcessFilter(filters));
                    }
                }


                foreach (var item in GetQueryStringDefaultGridLoad())
                    gridParams.AddParamAndUpdate(item.Key, item.Value);

                var responseGrid = RestHelper.ExecuteGetRequest<ResultBase<T>>(ResourceName, gridParams);
                if (!string.IsNullOrWhiteSpace(fileType))
                {
                    if (responseGrid.Total.Equals(0))
                        throw new Exception("Não existem registros para exportar");
                    DataTable dataTable = GridToDataTable(responseGrid, param, ResourceName, fileType);
                    switch (fileType.ToLower())
                    {
                        case "pdf":
                            GridToPDF(dataTable);
                            break;
                        case "doc":
                            GridToDOC(dataTable);
                            break;
                        case "xls":
                            GridToXLS(dataTable);
                            break;
                        case "csv":
                            GridToCSV(dataTable);
                            break;
                    }

                }

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
        protected abstract ContentUI FormJson();

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
        public virtual ContentResult Form()
            => Content(JsonConvert.SerializeObject(FormJson(), JsonSerializerSetting.Front), "application/json");

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public virtual ContentResult FormView()
        {
            var contentUI = FormJson();

            if (contentUI.History.WithParams.Equals(Url.Action("Edit"), StringComparison.InvariantCultureIgnoreCase))
                contentUI.History.WithParams = Url.Action("View");

            if (contentUI.Header.Buttons != null && !contentUI.Header.Buttons.Any(x => x.OnClickFn.Equals("fnCancelar", StringComparison.InvariantCultureIgnoreCase)))
                contentUI.Header.Buttons.Add(
                    new HtmlUIButton
                    {
                        Id = "cancel",
                        Label = "Voltar",
                        OnClickFn = "fnCancelar",
                        Position = contentUI.Header.Buttons.Any() ? HtmlUIButtonPosition.Out : HtmlUIButtonPosition.Main
                    }
                );

            contentUI.Content.Where(x => x is FormUI).ToList().ForEach(item => ((FormUI)item).Readonly = true);

            return Content(JsonConvert.SerializeObject(contentUI, JsonSerializerSetting.Front), "application/json");
        }

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


        #region ExportGrid 

        protected void GridToDOC(DataTable data)
        {
            GridView gv = new GridView()
            {
                AllowPaging = false,
                DataSource = data
            };
            gv.DataBind();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.doc");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-word ";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        protected void GridToXLS(DataTable data)
        {
            GridView gv = new GridView()
            {
                AllowPaging = false,
                DataSource = data
            };
            gv.DataBind();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            for (int i = 0; i < gv.Rows.Count; i++)
            {
                gv.Rows[i].Attributes.Add("class", "textmode");
            }
            gv.RenderControl(hw);

            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        protected void GridToPDF(DataTable data)
        {
            Font fontDefault = FontFactory.GetFont("Roboto", 8, BaseColor.BLACK);
            int columns = data.Columns.Count;
            PdfPTable table = new PdfPTable(columns);
            int padding = 5;
            float[] widths = new float[columns];
            for (int x = 0; x < columns; x++)
            {
                string cellText = Server.HtmlDecode(data.Columns[x].ColumnName);
                widths[x] = cellText.Length > 4 ? cellText.Length : 4;
                PdfPCell cell = new PdfPCell(new Phrase(new Chunk(cellText, FontFactory.GetFont("Roboto", 8, BaseColor.WHITE))))
                {
                    BorderWidth = 0,
                    BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f37021")),
                    Padding = padding
                };
                if (x != 0)
                {
                    cell.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#eee"));
                    cell.BorderWidthLeft = 1;
                }

                table.AddCell(cell);
            }
            for (int i = 0; i < data.Rows.Count; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    string cellText = Server.HtmlDecode(data.Rows[i].ItemArray[j].ToString());
                    widths[j] = cellText.Length > widths[j] ? cellText.Length : widths[j];
                    PdfPCell cell = new PdfPCell(new Phrase(new Chunk(cellText, FontFactory.GetFont("Roboto", 8, BaseColor.BLACK))))
                    {
                        BorderWidth = 0,
                        Padding = padding
                    };

                    if (i % 2 != 0)
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#eee"));

                    if (i != 0)
                    {
                        cell.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#eee"));
                        cell.BorderWidthLeft = 1;
                    }

                    table.AddCell(cell);
                }
            }
            var totalWidth = widths.Sum();
            for (int j = 0; j < columns; j++)
            {
                widths[j] = (float)((columns > 3 ? PageSize.A4.Height : PageSize.A4.Width) * 0.98) * (1 / totalWidth * widths[j]);
            }
            table.SetWidthPercentage(widths, PageSize.A4);
            table.LockedWidth = true;
            Response.ContentType = "application/pdf";
            Document pdfDoc = new Document(data.Columns.Count > 3 ? PageSize.A4.Rotate() : PageSize.A4, 10f, 10f, 10f, 0f);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            pdfDoc.Add(table);
            pdfDoc.Close();
            Response.End();
        }
        protected void GridToCSV(DataTable data)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";

            StringBuilder sb = new StringBuilder();
            foreach (DataColumn v in data.Columns)
                sb.Append(v.ColumnName + ',');

            sb.Append("\r\n");
            foreach (DataRow v1 in data.Rows)
            {
                for (int k = 0; k < data.Columns.Count; k++)
                    sb.Append(v1[k].ToString().Replace(",", ";") + ',');
                sb.Append("\r\n");
            }
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        protected abstract List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "");

        private List<JQueryDataTableParamsColumn> ParamsColumns(JQueryDataTableParams param, string fileType = "", string ResourceName = "")
        {

            if (fileType == "csv" || fileType == "xls")
            {
                try
                {
                    return GetParamsColumns(ResourceName);
                }
                catch { return param.Columns; }
            }
            else
                return param.Columns;
        }

        protected DataTable GridToDataTable(ResultBase<T> responseGrid, JQueryDataTableParams param, string ResourceName, string fileType)
        {
            DataTable dt = new DataTable();
            dt.Clear();

            var data = responseGrid.Data.Select(GetDisplayData()).ToList();
            Type o = data.FirstOrDefault().GetType();

            ParamsColumns(param, fileType, ResourceName).ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x.Name))
                    dt.Columns.Add(x.Name);
            });

            data.ForEach(x =>
            {
                DataRow dtr = dt.NewRow();
                ParamsColumns(param, fileType, ResourceName).ForEach(y =>
                {
                    if (!string.IsNullOrWhiteSpace(y.Name))
                        dtr[y.Name] = o.GetProperty(y.Data.Replace("/", "_")).GetValue(x, null);
                });
                dt.Rows.Add(dtr);
            });
            dt.Columns.Cast<DataColumn>().ToList().ForEach(column =>
            {
                if (dt.AsEnumerable().All(dr => dr.IsNull(column) || dr[column].ToString().Equals("")))
                    dt.Columns.Remove(column);
            });

            return dt;
        }
        #endregion
    }
}