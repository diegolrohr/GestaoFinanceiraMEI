using System.Collections.Generic;

namespace Fly01.Core.Helpers
{
    public class ReponseEmailNotification
    {
        public int TotalPlatforms { get; set; }
        public int TotalUsers { get; set; }
        public int TotalPlatformsWithError { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public List<ReponseEmailItemError> Errors { get; set; }

        public ReponseEmailNotification()
        {
            Errors = new List<ReponseEmailItemError>();
        }
    }

    public class ReponseEmailItemError
    {
        public string PlatformUrl { get; set; }
        public string ErrorMsg { get; set; }
    }
}