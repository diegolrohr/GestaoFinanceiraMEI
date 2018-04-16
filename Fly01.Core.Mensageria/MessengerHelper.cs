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

        private static Message GetMessage(string appData, string targetName, string targetUserName, DateTime scheduleDateTime, string textMessage, string serviceCode, bool waitReply = false)
        {
            return new Message()
            {
                AppData = appData,
                ScheduleDateTime = scheduleDateTime,
                WaitReply = waitReply,
                TargetName = targetName,
                TargetUserName = targetUserName,
                ServiceCode = serviceCode,
                Text = textMessage
            };
        }

        private static bool SendMessage(Message bodyMessage, string productCode, string serialNumber)
        {
            AuthenticationData auth = GetAuthenticationData();

            MessageData response = clientMessenger.SendSt(ConfigurationManager.AppSettings["MensageriaURL"], null, auth.TokenData.AccessToken, productCode, serialNumber, bodyMessage);
            if (response.Error.HasError)
                throw new ApplicationException(response.Error.ErrorMessage);

            return true;
        }
        #endregion

        #region Public Methods
        public static bool Send(string targetUserName, string targetName, string textMessage, string productCode, DateTime scheduleDateTime, string serviceCode, string serialNumber = "")
        {
            Message bodyMessage = GetMessage(string.Empty, targetName, targetUserName, scheduleDateTime, textMessage, serviceCode);
            return SendMessage(bodyMessage, productCode, serialNumber);
        }
        #endregion
    }
}