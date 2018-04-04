using System.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Fly01.Core.Controllers.API;
using System.Web.Http;
using Fly01.Financeiro.BL;
using Fly01.Financeiro.Domain.Entities;
using TOTVS.S1.Messenger.Library.Entities;
using System.Configuration;
using Fly01.Core;
using TOTVS.S1.Messenger.Library;
using Fly01.Financeiro.API.Models;
using System.Text;

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

                Task.Factory.StartNew(EnviaNotificacoes);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        #endregion

        #region Private Methods
        private void EnviaNotificacoes()
        {
            var messageDefault = "FLY01 Gestão Informa! Não há projeção de contas a pagar/receber para os próximos 7 dias.";

            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var plataformasNotificacoes = unitOfWork.ConfiguracaoNotificacaoBL.AllWithoutPlataformaId.Where(
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
                    else if(item.NotificaViaEmail && fluxoCaixaProjecao == null)
                        SendMensageria(messageDefault, item.EmailDestino, item.DiaSemana, item.HoraEnvio, ConfigurationManager.AppSettings["MensageriaServiceCodeEmail"], item.PlataformaId);
                }
            }
        }

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
            var totalPagamentos = fluxoCaixaProjecao.TotalPagamentos.ToString("C", AppDefaults.CultureInfoDefault);
            var totalRecebimentos = fluxoCaixaProjecao.TotalRecebimentos.ToString("C", AppDefaults.CultureInfoDefault);
            var saldoFinal = fluxoCaixaProjecao.SaldoFinal.ToString("C", AppDefaults.CultureInfoDefault);

            return $"FLY01 Gestão Informa! " +
                $"Projeção para os próximos 7 Dias. " +
                $"Total Pagamentos: {totalPagamentos}. " +
                $"Total Recebimentos: {totalRecebimentos}. " +
                $"Saldo Final: {saldoFinal}. ";
        }

        private void SendMensageria(string conteudo, string contatoDestino, DayOfWeek diaSemana, TimeSpan horarioEnvio, string serviceCode, string plataformaId)
        {
            MessengerLibrary clientMessenger = new MessengerLibrary();
            AuthenticationData auth = clientMessenger.InitializeSt(
                ConfigurationManager.AppSettings["MensageriaURL"],
                null,
                ConfigurationManager.AppSettings["MensageriaUserName"],
                ConfigurationManager.AppSettings["MensageriaUserPassword"]
            );

            Message smsBody = new Message()
            {
                AppData = "",
                ScheduleDateTime = DateTime.SpecifyKind(DateTime.Now.Date.Add(horarioEnvio), DateTimeKind.Utc),
                WaitReply = false,
                TargetName = plataformaId,
                TargetUserName = contatoDestino,
                ServiceCode = serviceCode,
                Text = conteudo
            };

            MessageData response = clientMessenger.SendSt(ConfigurationManager.AppSettings["MensageriaURL"], null, auth.TokenData.AccessToken, AppDefaults.AppIdFinanceiro, "", smsBody);
            if (response.Error.HasError)
                throw new ApplicationException(response.Error.ErrorMessage);
        } 
        #endregion
    }
}