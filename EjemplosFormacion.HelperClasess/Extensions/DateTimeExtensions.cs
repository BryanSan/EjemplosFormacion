using System;

namespace EjemplosFormacion.HelperClasess.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfThisWeek(this DateTime dateTime)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dateTime.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
                diff += 7;
            return dateTime.AddDays(-diff).Date;
        }

        public static DateTime LastDayOfThisWeek(this DateTime dateTime)
        {
            return dateTime.FirstDayOfThisWeek().AddDays(6);
        }

        public static DateTime FirstDayOfThisMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime LastDayOfThisMonth(this DateTime dateTime)
        {
            return dateTime.FirstDayOfThisMonth().AddMonths(1).AddDays(-1);
        }

        public static DateTime FirstDayOfNextMonth(this DateTime dateTime)
        {
            return dateTime.FirstDayOfThisMonth().AddMonths(1);
        }
    }
}
