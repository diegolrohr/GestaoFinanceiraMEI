using System;
using Fly01.Core.Notifications;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using Fly01.Core.Rest;

namespace Fly01.Core.API
{
    public class SocketIOHelper
    {
        public static void NewMessage(SocketMessageVM message)
        {
            Emit("newMessage", message);
        }

        public static void AnotherEventExample(SocketMessageVM message)
        {
            Emit("anotherEventExample", message);
        }

        private static void Emit(string emitEvent, SocketMessageVM message)
        {
            try
            {
                RestHelper.ExecutePostRequest(AppDefaults.UrlGatewayNew, "notifications/newmessage", JsonConvert.SerializeObject(message), null, null, 300);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex?.Message);
            }
        }
    }
}