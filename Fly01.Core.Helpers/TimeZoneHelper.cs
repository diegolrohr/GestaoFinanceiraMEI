using System;

namespace Fly01.Core.Helpers
{
    public static class TimeZoneHelper
    {
        public static DateTime ToClientTimeZone(this DateTime data, string timeZoneId = "E. South America Standard Time")
        {
            TimeZoneInfo clientTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(data, clientTimeZone);
        }

        public static DateTime DateTimeWithKind(DateTime data, DateTimeKind dateTimeKind = DateTimeKind.Unspecified)
        {
            return DateTime.SpecifyKind(data, dateTimeKind);
        }

        public static DateTime GetDateTimeNow(bool isLocal, string timeZoneId = "E. South America Standard Time")
        {
            if (isLocal)
            {
                return DateTime.Now;
            }
            else
            {
                //Azure é default utc 0, precisa retornar para South America
                //a DataHora estava saindo 3 horas adiantado da atual
                return ToClientTimeZone(TimeZoneHelper.DateTimeWithKind(DateTime.Now), timeZoneId);
            }
        }

        public static DateTime ToUniversalTimeZone(this DateTime data, string timeZoneId = "E. South America Standard Time")
        {
            DateTime dt = DateTime.SpecifyKind(data, DateTimeKind.Unspecified);
            TimeZoneInfo clientTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            if (clientTimeZone.IsDaylightSavingTime(dt.AddHours(1)))
            {
                DateTime PeriodoHorarioVerao = TimeZoneInfo.ConvertTimeToUtc(dt.AddHours(1), clientTimeZone);
                return PeriodoHorarioVerao.AddHours(-1);
            }

            return TimeZoneInfo.ConvertTimeToUtc(dt, clientTimeZone);
        }
    }
}
