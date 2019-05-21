using System;
using Fly01.Core.Notifications;
using Quobject.SocketIoClientDotNet.Client;
using Fly01.Core.ViewModels.Presentation.Commons;
using System.Text;
using Newtonsoft.Json;

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
                var socket = IO.Socket(AppDefaults.UrlNotificationSocket);
                socket.Connect();

                //TODO Encoding.UTF8.GetBytes??
                //aqui não precisa ser canal por aplicativo, nem por plataforma, os dados estão na mensagem
                //trocar UrlNotificationSocket web configs
                socket.Emit($"{emitEvent}", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));

                socket.Disconnect();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex?.Message);
            }
        }
    }
}