using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fly01.Estoque.Models.Utils
{
    public static class ButtonGroupHelper
    {
        public static MvcHtmlString ButtonGroupFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, List<SelectListItem> selectList, object htmlAttributes, bool readOnly = false, string defaultItemMessage = "", params object[] filterValues)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            if (readOnly)
                attributes["readonly"] = "readonly";

            if (selectList != null && selectList.Any())
            {
                if (metadata.Model != null && (metadata.Model is string && !string.IsNullOrWhiteSpace(metadata.Model.ToString())))
                    selectList.ForEach(x => x.Selected = x.Value.Equals(metadata.Model));

                if (filterValues != null && filterValues.Any())
                    selectList = selectList.Where(x => filterValues.Contains(x.Value)).ToList();

                if (!string.IsNullOrWhiteSpace(defaultItemMessage))
                    selectList.Insert(0, new SelectListItem { Text = defaultItemMessage, Value = "", Selected = false });
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(defaultItemMessage))
                {
                    selectList = new List<SelectListItem>();
                    selectList.Insert(0, new SelectListItem { Text = defaultItemMessage, Value = "", Selected = false });
                }
            }

            StringBuilder options = new StringBuilder();
            foreach (var item in selectList)
            {
                options.AppendFormat("<label class=\"btn btn-default {0}\">", item.Selected ? "active" : string.Empty);
                options.AppendFormat("<input type=\"radio\" name=\"{0}\" value=\"{1}\" autocomplete=\"off\" {2}>", metadata.PropertyName, item.Value, item.Selected ? "checked=\"\"" : string.Empty);
                options.AppendFormat(item.Text);
                options.AppendFormat("</label>");
            }

            return new MvcHtmlString(string.Concat("<div class=\"btn-group\" data-toggle=\"buttons\">", options.ToString(), "</div>"));
        }

    }
}