using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Rest;

namespace Fly01.Financeiro.Controllers.Base
{
    public abstract class GenericAppController : Controller
    {
        protected string ResourceName { get; set; }
        protected string ExpandProperties { get; set; }
        protected string APIEnumResourceName { get; set; }
        protected string AppEntitiesResourceName { get; set; }
        protected string AppViewModelResourceName { get; set; }

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
        public List<K> GetAll<K>(string order = "", string filterField = "", string filterValue = "")
        {
            Dictionary<string, string> queryStringRequest = AppDefaults.GetQueryStringDefault(filterField, filterValue, AppDefaults.MaxRecordsPerPageAPI);
            int page = 1;
            queryStringRequest.AddParam("page", page.ToString());

            if (!String.IsNullOrWhiteSpace(order))
                queryStringRequest.AddParam("order", order);

            string resource = AppDefaults.GetResourceName(typeof(K));
            List<K> items = new List<K>();
            bool hasNextRecord = true;
            while (hasNextRecord)
            {
                queryStringRequest.AddParam("page", page.ToString());
                ResultBase<K> response = RestHelper.ExecuteGetRequest<ResultBase<K>>(resource, queryStringRequest);
                items.AddRange(response.Data);
                hasNextRecord = response.HasNext && response.Data.Count > 0;
                page++;
            }

            return items;
        }

        public List<K> GetAll<K>(string resourceName, int maxRecords, string order = "", string filterField = "", string filterValue = "", Dictionary<string, string> querystring = null)
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

            var items = new List<K>();
            var hasNextRecord = true;
            while (hasNextRecord)
            {
                queryStringRequest.AddParam("page", page.ToString());
                var response = RestHelper.ExecuteGetRequest<ResultBase<K>>(resourceName, queryStringRequest.ToDictionary());
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

        //public virtual ContentResult Json(Guid id)
        //{
        //    T entity = Get(id);
        //    var x = Content(JsonConvert.SerializeObject(entity, JsonSerializerSetting.Front), "application/json");
        //    return x;
        //}

        //protected T Get(Guid id)
        //{
        //    string resourceName = AppDefaults.GetResourceName(typeof(T));
        //    string resourceById = String.Format("{0}/{1}", ResourceName, id);

        //    if (string.IsNullOrEmpty(ExpandProperties))
        //    {
        //        return RestHelper.ExecuteGetRequest<T>(resourceById);
        //    }
        //    else
        //    {
        //        var queryString = new Dictionary<string, string> {
        //            { "$expand", ExpandProperties }
        //        };
        //        return RestHelper.ExecuteGetRequest<T>(resourceById, queryString);
        //    }
        //}

        #region Views Methods

        public virtual ActionResult Index()
        {
            return View();
        }

        //public virtual ActionResult List()
        //{
        //    return PartialView("_List");
        //}

        //public virtual ActionResult Create()
        //{
        //    return View("Create");
        //}

        //public virtual ActionResult Edit(Guid id)
        //{
        //    return View("Edit", id);
        //}

        //[HttpPost]
        //public virtual JsonResult Delete(Guid id)
        //{
        //    try
        //    {
        //        RestHelper.ExecuteDeleteRequest(string.Format("{0}/{1}", ResourceName, id));
        //        return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Delete);
        //    }
        //    catch (Exception ex)
        //    {
        //        var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
        //        return JsonResponseStatus.GetFailure(error.Message);
        //    }
        //}

        //[HttpPost]
        //public virtual JsonResult Create(T entityVM)
        //{
        //    try
        //    {
        //        RestHelper.ExecutePostRequest(ResourceName, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
        //        return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create);
        //    }
        //    catch (Exception ex)
        //    {
        //        var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
        //        return JsonResponseStatus.GetFailure(error.Message);
        //    }
        //}

        //[HttpPost]
        //public virtual JsonResult Edit(T entityVM)
        //{
        //    try
        //    {
        //        var resourceNamePut = $"{ResourceName}/{entityVM.Id}";
        //        RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(entityVM, JsonSerializerSettingsEdit));

        //        return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Edit);
        //    }
        //    catch (Exception ex)
        //    {
        //        var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
        //        return JsonResponseStatus.GetFailure(error.Message);
        //    }
        //}

        #endregion
    }
}