namespace Fly01.Core.Notifications
{
    public class Error
    {
        public string DataField { get; set; }
        public string Message { get; set; }

        public Error(string pMessage, string dataFieldId = "")
        {
            DataField = dataFieldId;
            Message = pMessage;
        }
    }
}