using System;
using System.Configuration;
using TOTVS.S1.Messenger.Library;
using TOTVS.S1.Messenger.Library.Entities;

namespace Fly01.Core.Mensageria
{
    public static class MessengerHelper
    {
        private static MessengerLibrary clientMessenger = new MessengerLibrary();

        #region Private Methods
        private static AuthenticationData GetAuthenticationData()
        {
            return clientMessenger.InitializeSt(
                ConfigurationManager.AppSettings["MensageriaURL"],
                null,
                ConfigurationManager.AppSettings["MensageriaUserName"],
                ConfigurationManager.AppSettings["MensageriaUserPassword"]
            );
        }

        private static Message GetEmailMessage(string appData, string targetName, string targetUserName, DateTime scheduleDateTime, string textMessage, bool waitReply = false)
        {
            return new Message()
            {
                AppData = appData,
                ScheduleDateTime = scheduleDateTime,
                WaitReply = waitReply,
                TargetName = targetName,
                TargetUserName = targetUserName,
                ServiceCode = ConfigurationManager.AppSettings["MensageriaServiceCodeEmail"],
                Text = textMessage
            };
        }

        private static bool SendEmailMessage(Message bodyMessage, string productCode, string serialNumber)
        {
            AuthenticationData auth = GetAuthenticationData();

            MessageData response = clientMessenger.SendSt(ConfigurationManager.AppSettings["MensageriaURL"], null, auth.TokenData.AccessToken, productCode, serialNumber, bodyMessage);
            if (response.Error.HasError)
                throw new ApplicationException(response.Error.ErrorMessage);

            return true;
        }
        #endregion

        #region Public Methods
        public static bool SendEmail(string targetEmail, string targetName, string message, string productCode, DateTime scheduleDateTime, string serialNumber = "")
        {
            Message bodyMessage = GetEmailMessage(string.Empty, targetName, targetEmail, scheduleDateTime, message);
            return SendEmailMessage(bodyMessage, productCode, serialNumber);
        }
        #endregion
    }
}