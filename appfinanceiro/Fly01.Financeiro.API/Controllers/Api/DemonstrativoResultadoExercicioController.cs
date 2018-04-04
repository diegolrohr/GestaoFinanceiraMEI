//using System;
//using System.Web.Http;
//using System.Web.OData;
//using Fly01.Financeiro.BL;
//using System.Web.OData.Routing;
//using Fly01.Financeiro.Domain.Entities;

//namespace Fly01.Financeiro.API.Controllers.Api
//{
//    [ODataRoutePrefix("demonstrativoresultadoexercicio")]
//    public class DemonstrativoResultadoExercicioController : ApiPlataformaController<DemonstrativoResultadoExercicio, DemonstrativoResultadoExercicioBL>
//    {
//        [EnableQuery(EnsureStableOrdering = false)]
//        public IHttpActionResult Get(DateTime dataInicial, DateTime dataFinal)
//        {
//            return Ok(UnitOfWork.DemonstrativoResultadoExercicioBL.Get(dataInicial, dataFinal));        }
//    }
//}