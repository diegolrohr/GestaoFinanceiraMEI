using Fly01.Financeiro.BL;
using Fly01.Core.API;
using System.Linq;

namespace Fly01.Financeiro.API.Controllers.Api
{
    public class HelloController : ApiHelloBaseController
    {
        public HelloController()
            : base(System.Web.HttpRuntime.BinDirectory, "GestaoFinanceiraMEI.API.dll") { }

        public override void TestDbConnection()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var exists = unitOfWork.EstadoBL.All.Any();
            }
        }
    }
}