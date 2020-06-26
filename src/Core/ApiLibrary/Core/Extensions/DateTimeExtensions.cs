using System;

namespace ApiLibrary.Core.Extensions
{
    public static class DateTimeExtensions
    {

        public static DateTime FirstDayOfWeek(this DateTime dt)
        {
            int diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
