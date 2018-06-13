using Fly01.Core.API;
using Fly01.Financeiro.BL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using Boleto2Net;
using Fly01.Core.ViewModels.Presentation;

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
            var dadosArquivoRemessa = new List<DadosArquivoRemessaVM>();

            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                foreach (var id in listIdCnab.Split(',').Select(x => Guid.Parse(x)))
                {
                    var cnabBL = unitOfWork.CnabBL;
                    var data = cnabBL.GetCnab(id);
                    var boletoVM = cnabBL.GetDadosBoleto(data.ContaReceberId.Value, data.ContaBancariaCedenteId.Value);

                    var boletoBancario = cnabBL.GeraBoleto(boletoVM);

                    dictBoletos.Add(new KeyValuePair<Guid?, BoletoBancario>(data.ContaBancariaCedenteId, boletoBancario));
                }

                foreach (var item in dictBoletos.GroupBy(x => x.Key).OrderByDescending(x => x.Key).ToList())
                {
                    var lstBoletos = dictBoletos.Where(x => x.Key == item.Key).Select(x => x.Value).ToList();
                    var banco = lstBoletos.FirstOrDefault().Boleto.Banco;
                    var total = (double)lstBoletos.Sum(x => x.Boleto.ValorTitulo);
                    var boletos = new Boletos() { Banco = banco };
                    boletos.AddRange(lstBoletos.Select(x => x.Boleto));

                    var arquivoRemessa = new ArquivoRemessa(banco, BoletoBL.GetTipoCnab(banco.Codigo), 1); // tem que avaliar os dados passados(tipoArquivo, NumeroArquivo)
                    dadosArquivoRemessa.Add(new DadosArquivoRemessaVM
                    {
                        ContaBancariaCedenteId = item.Key.Value,
                        CodigoBanco = banco.Codigo,
                        TotalBoletosGerados = boletos.Count(),
                        ValorTotalArquivoRemessa = total,
                        ConteudoArquivoRemessa = arquivoRemessa.GerarArquivoRemessa(boletos)
                    });
                }
            }

            return Ok(dadosArquivoRemessa);
        }
    }
}