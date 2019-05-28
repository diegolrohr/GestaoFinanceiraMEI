using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum SocketMessageType
    {
        [Subtitle("INFO", "INFO")]
        INFO = 0,
        [Subtitle("SUCCESS", "SUCCESS")]
        SUCCESS = 1,
        [Subtitle("ERROR", "ERROR")]
        ERROR = 2,
        [Subtitle("WARNING", "WARNING")]
        WARNING = 3
    }
}