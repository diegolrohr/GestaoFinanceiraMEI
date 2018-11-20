using System;

namespace Fly01.Core.Notifications
{
    public class Error
    {
        public string DataField { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }

        public Error(string pMessage, string dataFieldId = "", string id = "")
        {
            DataField = dataFieldId;
            Message = pMessage;
            ReferenceId = id; 
        }
    }
}