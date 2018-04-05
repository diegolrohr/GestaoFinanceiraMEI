using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Fly01.Core.Presentation.Commons
{
    public static class AutoCompleteHelper
    {
        private static string GenerateAutoCompleteUrl(HtmlHelper html, string actionName, string controllerName)
        {
            return UrlHelper.GenerateUrl(null, actionName, controllerName, null, html.RouteCollection,
                html.ViewContext.RequestContext, includeImplicitMvcValues: true);
        }

        public static MvcHtmlString AutoCompleteFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string actionName, string controllerName, string style = "")
        {
            string autocompleteUrl = GenerateAutoCompleteUrl(html, actionName, controllerName);

            return html.TextBoxFor(expression, new { data_autocomplete_url = autocompleteUrl, style });
        }

        public static MvcHtmlString AutoCompleteFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string actionName, string controllerName, IDictionary<string, object> htmlAttributes = null)
        {
            string autocompleteUrl = GenerateAutoCompleteUrl(html, actionName, controllerName);

            htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();
            htmlAttributes.Add("data-autocomplete-url", autocompleteUrl);

            return html.TextBoxFor(expression, htmlAttributes);
        }

        public static MvcHtmlString AutoComplete(this HtmlHelper html, string name, string actionName, string controllerName, string defaultValue = null, IDictionary<string, object> htmlAttributes = null)
        {
            string autocompleteUrl = GenerateAutoCompleteUrl(html, actionName, controllerName);

            htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();
            htmlAttributes.Add("data-autocomplete-url", autocompleteUrl);

            return html.TextBox(name, defaultValue, htmlAttributes);
        }
    }
}