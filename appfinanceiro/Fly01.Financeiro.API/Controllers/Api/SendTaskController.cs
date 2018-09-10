using Fly01.Core;
using Fly01.Core.API;
using Fly01.Core.Mensageria;
using Fly01.Financeiro.API.Models;
using Fly01.Financeiro.BL;
using Fly01.Core.Entities.Domains.Commons;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("SendTask")]
    public class SendTaskController : ApiBaseController
    {
        #region Public Methods
        [HttpPost]
        public IHttpActionResult Envia()
        {
            try
            {
                var validsPlataformaUrl = new List<string>()
                {
                    "schedulerAzure3D587913-4BB8-4135-B40B-8177DE7F99F8.fly01dev.com.br",
                    "schedulerAzure40D25BFC-D5CC-4F3B-8312-B40124E02565.fly01.com.br"
                };

                if (!validsPlataformaUrl.Contains(PlataformaUrl))
                    return BadRequest("Chamada Inválida");

                Task.Factory.StartNew(action: () => {
                    var messageDefault = "Bemacash Gestão Informa! Não há projeção de contas a pagar/receber para os próximos 7 dias.";

                    using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                    {
                        var plataformasNotificacoes = unitOfWork.ConfiguracaoNotificacaoBL.Everything.Where(
                            x => x.DiaSemana == DateTime.Today.DayOfWeek && (x.NotificaViaEmail || x.NotificaViaSMS)).ToList();

                        foreach (var item in plataformasNotificacoes.ToList())
                        {
                            var fluxoCaixaProjecao = unitOfWork.ContaFinanceiraBL.GetAllContasNextDays(DateTime.Now.Date.Add(item.HoraEnvio), DateTime.Now.AddDays(7), item.PlataformaId);

                            if (item.NotificaViaSMS && fluxoCaixaProjecao != null)
                                SendMensageria(FormatTextSMS(fluxoCaixaProjecao), item.ContatoDestino, item.DiaSemana, item.HoraEnvio, ConfigurationManager.AppSettings["MensageriaServiceCodeSMS"], item.PlataformaId);
                            else if (item.NotificaViaSMS && fluxoCaixaProjecao == null)
                                SendMensageria(messageDefault, item.ContatoDestino, item.DiaSemana, item.HoraEnvio, ConfigurationManager.AppSettings["MensageriaServiceCodeSMS"], item.PlataformaId);

                            if (item.NotificaViaEmail && fluxoCaixaProjecao != null)
                                SendMensageria(FormatTextMail(fluxoCaixaProjecao), item.EmailDestino, item.DiaSemana, item.HoraEnvio, ConfigurationManager.AppSettings["MensageriaServiceCodeEmail"], item.PlataformaId);
                            else if (item.NotificaViaEmail && fluxoCaixaProjecao == null)
                                SendMensageria(messageDefault, item.EmailDestino, item.DiaSemana, item.HoraEnvio, ConfigurationManager.AppSettings["MensageriaServiceCodeEmail"], item.PlataformaId);
                        }
                    }
                });

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        #endregion

        #region Private Methods

        private string FormatTextMail(FluxoCaixaProjecao fluxoCaixaProjecao)
        {
            return "Notificações de Contas Financeiras#!#" + new StringBuilder(EmailFilesHelper.NotificaEmailContasFinanceira.Value
                .Replace("{TOTAL_PAGAMENTOS}", fluxoCaixaProjecao.TotalPagamentos.ToString("C", AppDefaults.CultureInfoDefault))
                .Replace("{TOTAL_RECEBIMENTOS}", fluxoCaixaProjecao.TotalRecebimentos.ToString("C", AppDefaults.CultureInfoDefault))
                .Replace("{SALDO_FINAL}", fluxoCaixaProjecao.SaldoFinal.ToString("C", AppDefaults.CultureInfoDefault))
            ).ToString();
        }

        private string FormatTextSMS(FluxoCaixaProjecao fluxoCaixaProjecao)
        {
            return $"Bemacash Gestão Informa! " +
                $"Projeção para os próximos 7 Dias. " +
                $"Total Pagamentos: {fluxoCaixaProjecao.TotalPagamentos.ToString("C", AppDefaults.CultureInfoDefault)}. " +
                $"Total Recebimentos: {fluxoCaixaProjecao.TotalRecebimentos.ToString("C", AppDefaults.CultureInfoDefault)}. " +
                $"Saldo Final: {fluxoCaixaProjecao.SaldoFinal.ToString("C", AppDefaults.CultureInfoDefault)}. ";
        }

        private void SendMensageria(string conteudo, string contatoDestino, DayOfWeek diaSemana, TimeSpan horarioEnvio, string serviceCode, string plataformaId)
        {
            try
            {
                var response = MessengerHelper.Send(contatoDestino, plataformaId, conteudo, AppDefaults.LicenciamentoProdutoId.ToString(), DateTime.SpecifyKind(DateTime.Now.Date.Add(horarioEnvio), DateTimeKind.Utc), serviceCode, "");
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
        #endregion
    }
}