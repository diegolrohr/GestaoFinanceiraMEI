using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.IO;
using Fly01.Core.Helpers;
using Fly01.Core.Rest;

namespace Fly01.Core.Presentation.Controllers
{
    public abstract class GenericAppController : PrimitiveBaseController
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

        protected string RenderRazorViewToString(string viewName)
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

        protected virtual void LoadDependence() { }

        public virtual ActionResult Index()
            => View();
    }
}