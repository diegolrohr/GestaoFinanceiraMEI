using Fly01.Core.API;
using Fly01.Financeiro.BL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using Boleto2Net;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/boleto")]
    public class BoletoController : ApiBaseController
    {
        [HttpGet]
        [Route("imprimeBoleto")]
        public async Task<IHttpActionResult> ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var boleto = unitOfWork.CnabBL.ImprimeBoleto(contaReceberId, contaBancariaId);

                if (boleto == null) Ok();

                unitOfWork.CnabBL.SalvaBoleto(boleto, contaReceberId, contaBancariaId, false);
                await unitOfWork.Save();

                return Ok($"<div style=\"margin: 15px;\">{boleto.MontaHtml()}</div>");
            }
        }

        [HttpGet]
        [Route("getListaBoletos")]
        public IHttpActionResult GetListaBoletos(string listIdCnab)
        {
            var dictBoletos = new List<KeyValuePair<Guid?, BoletoBancario>>();

            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                foreach (var id in listIdCnab.Split(',').Select(x => Guid.Parse(x)))
                {
                    var cnabBL = unitOfWork.CnabBL;
                    var data = cnabBL.GetCnab(id);
                    var boletoVM = cnabBL.GetDadosBoleto(data.ContaReceberId.Value, data.ContaBancariaCedenteId.Value);
                    //var banco = Banco.Instancia(codigoBancConvert.ToInt32(data.ContaBancariaCedente.Banco.Codigo));
                    //cedente
                    //banco.Cedente = boletoVM.Cedente;
                    //var boletoBancario = new Boleto(banco);

                    var boletoBancario = cnabBL.GeraBoleto(boletoVM);

                    dictBoletos.Add(new KeyValuePair<Guid?, BoletoBancario>(data.ContaBancariaCedenteId, boletoBancario));
                }
            }

            return Ok(dictBoletos);
        }
    }
}