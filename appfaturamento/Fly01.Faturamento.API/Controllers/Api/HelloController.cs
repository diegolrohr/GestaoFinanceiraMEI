using Fly01.Core.API;
using Fly01.Faturamento.BL;
using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.API.Controllers.Api
{
    public class HelloController : ApiHelloBaseController
    {
        public HelloController()
            : base(System.Web.HttpRuntime.BinDirectory, "Fly01.Faturamento.API.dll") { }

        public override void TestDbConnection()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var exists = unitOfWork.EstadoBL.All.Any();
            }
        }
    }
}