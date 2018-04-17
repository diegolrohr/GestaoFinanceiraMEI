using System;
using System.Net;

namespace Fly01.Core.Rest
{
    public class ApiException : Exception
    {
        private HttpStatusCode statusCode = HttpStatusCode.OK;
        public HttpStatusCode StatusCode
        {
            get
            {
                if (statusCode == 0)
                    statusCode = HttpStatusCode.InternalServerError;

                return statusCode;
            }

            set { statusCode = value; }
        }

        public ApiException(HttpStatusCode statusCode, string message, Exception ex)
            : base(message, ex)
        {
            this.statusCode = statusCode;
        }

        public ApiException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            this.statusCode = statusCode;
        }

        public ApiException(HttpStatusCode statusCode)
        {
            this.statusCode = statusCode;
        }
    }
}
