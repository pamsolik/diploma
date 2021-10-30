using System;

namespace Cars.Services.Interfaces
{
    public interface IDateTimeProvider
    {
        public DateTime GetTimeNow();

        public string GetTimeAgoDescription(DateTime time);
    }
}