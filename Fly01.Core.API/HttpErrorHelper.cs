using System;
using System.Web.Http;
using System.Reflection;
using Fly01.Core.Notifications;

namespace Fly01.Core.Helpers
{
    public class HttpErrorHelper
    {
        public static HttpError Create<T>(BusinessException exception) where T : Exception
        {
            var properties = exception.GetType().GetProperties(BindingFlags.Instance
                                                             | BindingFlags.Public
                                                             | BindingFlags.DeclaredOnly);
            var error = new HttpError();

            foreach (var propertyInfo in properties)
                error.Add(propertyInfo.Name, propertyInfo.GetValue(exception, null));

            return error;
        }
    }
}