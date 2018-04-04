using Fly01.Financeiro.BL;
using Fly01.Utils.Api;
using Fly01.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Base
{
    [CustomExceptionFilter]
    public class ApiBaseController : ApiController
    {
        private UnitOfWorkInitialize _unitOfWorkInitialize;
        protected UnitOfWorkInitialize unitOfWorkInitialize
        {
            get
            {
                if (_unitOfWorkInitialize == null)
                {
                    _unitOfWorkInitialize = new UnitOfWorkInitialize
                    {
                        PlatformUrl = PlataformaUrl,
                        UserName = UserName
                    };
                }

                return _unitOfWorkInitialize;
            }

            set
            {
                _unitOfWorkInitialize = value;
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

        public string UserName
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
