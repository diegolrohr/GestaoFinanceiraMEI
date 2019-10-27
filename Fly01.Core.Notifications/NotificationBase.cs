using Fly01.Core.Notifications.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fly01.Core.Notifications
{
    public abstract class NotificationBase : INotification
    {
        public List<Error> Errors { get; set; } = new List<Error>();
        public List<Warning> Warnings { get; set; } = new List<Warning>();
        public List<Message> Messages { get; set; } = new List<Message>();
        public bool HasErrors => 0 != Errors.Count;
        public bool HasWarnings => 0 != Warnings.Count;
        public bool HasMessages => 0 != Messages.Count;
        public bool HasNotifications => HasErrors || HasWarnings || HasWarnings;

        public string Get()
        {
            var innerMessage = new { innerMessage = Errors.ToArray() };
            return JsonConvert.SerializeObject(new { errorMessage = JsonConvert.SerializeObject(innerMessage) });
        }
    }
}