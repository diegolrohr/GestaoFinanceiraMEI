using Fly01.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Fly01.Core.API
{
    [CustomExceptionFilter]
    public abstract class ApiBaseController : ApiController
    {
        private ContextInitialize _contextInitialize;
        protected ContextInitialize ContextInitialize
        {
            get
            {
                return _contextInitialize
                    ?? (_contextInitialize = new ContextInitialize
                    {
                        PlataformaUrl = PlataformaUrl,
                        AppUser = AppUser
                    });
            }

            set
            {
                _contextInitialize = value;
            }
        }

        public string PlataformaUrl
        {
            get
            {
                IEnumerable<string> values;
                if (Request.Headers.TryGetValues("PlataformaUrl", out values))
                    return values.FirstOrDefault();

                throw new ArgumentException("PlataformaUrl não informada.");
            }
        }

        public string AppUser
        {
            get
            {
                IEnumerable<string> values;
                if (Request.Headers.TryGetValues("AppUser", out values))
                    return values.FirstOrDefault();

                throw new ArgumentException("AppUser não informado.");
            }
        }
    }
}
