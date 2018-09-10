using Fly01.OrdemServico.BL;
using Fly01.Core.API;
using System.Linq;

namespace Fly01.OrdemServico.API.Controllers.Api
{
    public class HelloController : ApiHelloBaseController
    {
        public HelloController()
            : base(System.Web.HttpRuntime.BinDirectory, "Fly01.OrdemServico.API.dll") { }

        public override void TestDbConnection()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var exists = unitOfWork.EstadoBL.All.Any();
            }
        }
    }
}