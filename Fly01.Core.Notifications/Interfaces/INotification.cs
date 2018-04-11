using System.Collections;
using System.Collections.Generic;

namespace Fly01.Core.Notifications.Interfaces
{
    public interface INotification
    {
        List<Error> Errors { get; set; }
        List<Warning> Warnings { get; set; }
        List<Message> Messages { get; set; }
        bool HasErrors { get; }
        bool HasWarnings { get; }
        bool HasMessages { get; }
        bool HasNotifications { get; }

        string Get();
    }
}
