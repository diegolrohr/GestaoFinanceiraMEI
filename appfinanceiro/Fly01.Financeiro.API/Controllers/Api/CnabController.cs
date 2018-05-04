using Fly01.Financeiro.BL;
using System;
using System.Web.Http;
using Fly01.Core.API;
using Fly01.Core.Base;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/cnab")]
    public class CnabController : ApiBaseController
    {
        [HttpGet]
        [Route("imprimeBoleto/{contaReceberId}/{contaBancariaId}")]
        //public void ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId, DateTime dataDesconto, double valorDesconto)
        public IHttpActionResult ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            //http://localhost/Fly01.financeiro.API/api/cnab/imprimeBoleto/f3e8fc87-adbf-4ac8-8ae5-218755567538/f3e8fc87-adbf-4ac8-8ae5-218755567538
            using (var unit = new UnitOfWork(ContextInitialize))
            {
                var boletos = unit.CnabBL.GeraBoletos(contaReceberId, contaBancariaId, DateTime.Now, 0);

                foreach (var item in boletos)
                {
                    using (var imprimeBoleto = new Boleto2Net.BoletoBancario())
                    {
                        imprimeBoleto.Boleto = item;
                        imprimeBoleto.OcultarInstrucoes = false;
                        imprimeBoleto.MostrarComprovanteEntrega = true;
                        imprimeBoleto.MostrarEnderecoCedente = true;
                    }

                    //{
                    //    html.Append("<div style=\"page-break-after: always;\">");
                    //    html.Append(imprimeBoleto.MontaHtml());
                    //    html.Append("</div>");
                    //}

                }

            }
            return Ok(contaReceberId);
        }
    }
}