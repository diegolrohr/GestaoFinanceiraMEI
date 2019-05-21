using System;
using Fly01.Core.Notifications;
using Quobject.SocketIoClientDotNet.Client;
using System.Collections.Generic;
using Newtonsoft.Json;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Core.API
{
    public class SocketIOHelper
    {
        public static void Emit(string emitEvent, SocketMessageVM message)
        {
            try
            {
                var socket = IO.Socket("AppDefaults");
                socket.Connect();

                socket.Emit(emitEvent, message);

                socket.Disconnect();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex?.Message);
            }
        }
    }
}