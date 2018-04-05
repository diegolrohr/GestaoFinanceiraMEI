using System;
using System.Net;
using System.Text;
using System.Net.Http;
using Fly01.Core.Helpers;
using System.Web.Http.Filters;
using Fly01.Core.ValueObjects;
using System.Data.Entity.Infrastructure;

namespace Fly01.Core.Attribute
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is BusinessException || actionExecutedContext.Exception is ArgumentException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new JsonContent(actionExecutedContext.Exception.Message)
                };
            }
            else if (actionExecutedContext.Exception is DbUpdateException)
            {
                var sb = new StringBuilder();
                var inner = actionExecutedContext.Exception.InnerException;

                while (inner != null)
                {
                    sb.AppendFormat("{0}. ", inner.Message);
                    inner = inner.InnerException;
                }

                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(sb.ToString())
                };
            }
            else
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(actionExecutedContext.Exception.InnerException?.Message ?? actionExecutedContext.Exception.Message)
                };
            }
        }
    }
}