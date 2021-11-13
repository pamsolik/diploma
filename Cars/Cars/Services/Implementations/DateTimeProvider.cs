using System;
using Cars.Services.Interfaces;

namespace Cars.Services.Implementations
{
    public class DateTimeProvider : IDateTimeProvider

    {
        public DateTime GetTimeNow()
        {
            return DateTime.UtcNow;
        }

        public string GetTimeAgoDescription(DateTime dt)
        {
            var ts = new TimeSpan(GetTimeNow().Ticks - dt.Ticks);
            var delta = Math.Abs(ts.TotalSeconds);

            switch (delta)
            {
                case < 60:
                    return ts.Seconds == 1 ? "sekundę temu" : ts.Seconds + " sekund temu";
                case < 120:
                    return "minutę temu";
                case < 2700:
                    return ts.Minutes + " minut(y) temu";
                case < 5400:
                    return "godzinę temu";
                case < 86400:
                    return ts.Hours + " godzin temu";
                case < 172800:
                    return "wczoraj";
                case < 2592000:
                    return ts.Days + " dni temu";
                case < 31104000:
                {
                    var months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                    return months <= 1 ? "miesiąc temu" : months + " miesięcy temu";
                }
                default:
                {
                    var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                    return years <= 1 ? "rok temu" : years + " lat temu";
                }
            }
        }
    }
}