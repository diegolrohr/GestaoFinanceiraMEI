using Newtonsoft.Json;

namespace Fly01.Core.Api
{
    public class APIErrorInfo
    {
        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("error")]
        public APIError APIError
        {
            get
            {
                return JsonConvert.DeserializeObject<APIError>(ErrorMessage);
            }
        }

        public string GetMessage()
        {
            if (APIError != null)
            {
                if (APIError.Error != null)
                {
                    if (APIError.Error.APIErrorInnerMessage != null)
                    {
                        if (APIError.Error.APIErrorInnerMessage != null)
                            return APIError.Error.APIErrorInnerMessage.Message;
                    }
                    else
                        return APIError.Error.Message;
                }
                else
                    return "";
            }
            else
                return ErrorMessage;

            return "";
        }
    }

    public class APIError
    {
        [JsonProperty("error")]
        public APIErrorMessage Error { get; set; }
    }

    public class APIErrorMessage
    {
        [JsonProperty("code")]
        public int? Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("innererror")]
        public APIErrorInnerMessage APIErrorInnerMessage { get; set; }
    }

    public class APIErrorInnerMessage
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("stacktrace")]
        public string StackTrace { get; set; }
    }
}
