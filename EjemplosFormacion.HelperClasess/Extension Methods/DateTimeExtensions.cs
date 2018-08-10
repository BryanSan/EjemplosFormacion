using System;

namespace EjemplosFormacion.HelperClasess.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Extension Method para hallar el primer dia de la semana actual
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfThisWeek(this DateTime dateTime)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dateTime.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
                diff += 7;
            return dateTime.AddDays(-diff).Date;
        }

        /// <summary>
        /// Extension Method para hallar el ultimo dia de esta semana
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime LastDayOfThisWeek(this DateTime dateTime)
        {
            return dateTime.FirstDayOfThisWeek().AddDays(6);
        }

        /// <summary>
        /// Extension Method para hallar el primer dia de este mes
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfThisMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// Extension Method para hallar el ultimo dia de este mes
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime LastDayOfThisMonth(this DateTime dateTime)
        {
            return dateTime.FirstDayOfThisMonth().AddMonths(1).AddDays(-1);
        }
    }
}
