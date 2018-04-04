using System;
using System.Configuration;
using TOTVS.S1.Messenger.Library;
using TOTVS.S1.Messenger.Library.Entities;

namespace Fly01.Core.Messages
{
    public static class Mensageria
    {
        public static void Send(string conteudo, string contatoDestino, DayOfWeek diaSemana, TimeSpan horarioEnvio, string serviceCode, string plataformaId, string appId, bool isScheduled)
        {
            var clientMessenger = new MessengerLibrary();
            var auth = clientMessenger.InitializeSt(
                ConfigurationManager.AppSettings["MensageriaURL"],
                null,
                ConfigurationManager.AppSettings["MensageriaUserName"],
                ConfigurationManager.AppSettings["MensageriaUserPassword"]
            );

            var messageBody = new Message()
            {
                AppData = "",
                ScheduleDateTime = isScheduled ? DateTime.SpecifyKind(DateTime.Now.Date.Add(horarioEnvio), DateTimeKind.Utc) : new DateTime(),
                WaitReply = false,
                TargetName = plataformaId,
                TargetUserName = contatoDestino,
                ServiceCode = serviceCode,
                Text = conteudo
            };
            
            var response = clientMessenger.SendSt(ConfigurationManager.AppSettings["MensageriaURL"], null, auth.TokenData.AccessToken, appId, "", messageBody);

            if (response.Error.HasError)
                throw new ApplicationException(response.Error.ErrorMessage);
        }
    }
}
