using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using TOTVS.S1.SaaS.Commons.Exceptions;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using Fly01.Core.Helpers;
using Fly01.Core;

namespace Fly01.Estoque.Controllers.Base
{
    public abstract class GenericAppController : Controller
    {
        protected string ResourceName { get; set; }
        protected string ExpandProperties { get; set; }

        protected string APIEnumResourceName { get; set; }
        protected string AppEntitiesResourceName { get; set; }
        protected string AppViewModelResourceName { get; set; }

        public JsonSerializerSettings JsonSerializerSettingsNullDefaultValue
        {
            get
            {
                return new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    Formatting = Formatting.None,
                    FloatFormatHandling = FloatFormatHandling.DefaultValue,
                    FloatParseHandling = FloatParseHandling.Decimal,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                };
            }
        }

        public JsonSerializerSettings JsonSerializerSettingsEdit
        {
            get
            {
                return new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    Formatting = Formatting.None,
                    FloatFormatHandling = FloatFormatHandling.DefaultValue,
                    FloatParseHandling = FloatParseHandling.Decimal,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                };
            }
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

        /// <summary>
        /// Função responsável por carregar as depencias do entidade
        /// Isso é necessário quando a entidade em questão possui combos,
        /// para isso utiliza-se do ViewBag.
        /// </summary>
        protected virtual void LoadDependence() { }

        public virtual ActionResult Index()
        {
            return View();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is ApiException && ((ApiException)filterContext.Exception).StatusCode == HttpStatusCode.Unauthorized)
            {
                //401 token expirado or invalido
                if (Request.Headers["X-Requested-With"] != null && Request.Headers["X-Requested-With"].ToUpper().Equals("XMLHTTPREQUEST"))
                {
                    Response.ClearContent();
                    Response.StatusCode = 401;
                    Response.AddHeader("WWW-Authenticate", "Bearer");
                }
                else
                {
                    Response.Write(
                        String.Format("<script type=\"text/javascript\">top.location.href='{0}';</script>",
                            FormsAuthentication.LoginUrl));
                }

                Response.End();
                return;
            }
            else
            {
                base.OnException(filterContext);
            }
        }

        public List<K> GetAll<K>(string order = "", string filterField = "", string filterValue = "", Dictionary<string, string> queryStringRequest = null)
        {
            return GetAll<K>(AppDefaults.GetResourceName(typeof(K)), AppDefaults.MaxRecordsPerPageAPI, order, filterField, filterValue, queryStringRequest);
        }

        public List<K> GetAll<K>(string resourceName, int maxRecords, string order = "", string filterField = "", string filterValue = "", Dictionary<string, string> querystring = null)
        {
            List<KeyValuePair<string, string>> queryStringRequest = new List<KeyValuePair<string, string>>();
            queryStringRequest.AddRange(AppDefaults.GetQueryStringDefault(filterField, filterValue, maxRecords));
            if (querystring != null)
            {
                queryStringRequest.AddRange(querystring);
            }

            int page = 1;
            queryStringRequest.AddParam("page", page.ToString());

            if (!String.IsNullOrWhiteSpace(order))
                queryStringRequest.AddParam("order", order);

            List<K> items = new List<K>();
            bool hasNextRecord = true;
            while (hasNextRecord)
            {
                queryStringRequest.AddParam("page", page.ToString());
                ResultBase<K> response = RestHelper.ExecuteGetRequest<ResultBase<K>>(resourceName, queryStringRequest.ToDictionary());
                items.AddRange(response.Data);
                hasNextRecord = response.HasNext && response.Data.Count > 0;
                page++;
            }

            return items;
        }
    }
}