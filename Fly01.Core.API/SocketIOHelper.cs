using System;
using Fly01.Core.Notifications;
//using Quobject.SocketIoClientDotNet.Client;
using Fly01.Core.ViewModels.Presentation.Commons;
using System.Text;
using Newtonsoft.Json;
using SocketIOClient;

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
                //IEmitter io = new Emitter(new EmitterOptions
                //{
                //    Host = "http://10.51.4.35:5000/",
                //    Port = 5000
                //})
                var socket = new Client("ws://10.51.4.35:5000/socket.io/?EIO=3&transport=polling&t=MhXJhSH/");
                //http://10.51.4.35:5000/socket.io/?EIO=3&transport=polling&t=MhXJhSH
                socket.Connect("AppsGestao");
                socket.Emit(emitEvent, message);

                var socket2 = new Client("http://10.51.4.35:5000/socket.io/?EIO=3&transport=polling&t=MhXJhSH/");
                socket2.Connect("AppsGestao");
                socket2.Emit(emitEvent, message);
                //var socket = IO.Socket(AppDefaults.UrlNotificationSocket);
                //var socket = IO.Socket("http://10.51.4.35:5000/AppsGestao");
                //socket.Connect();

                //TODO Encoding.UTF8.GetBytes??
                //aqui não precisa ser canal por aplicativo, nem por plataforma, os dados estão na mensagem
                //trocar UrlNotificationSocket web configs
                //socket.Emit($"{emitEvent}", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));

                //socket.Disconnect();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex?.Message);
            }
        }
    }
}