using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        #region ToLocal & ToUtc
        public static DateTime ToLocal(this DateTime dt) => dt.ToLocalTime();

        public static DateTime ToUtc(this DateTime dt) => dt.ToUniversalTime();

        public static DateTime ToUtcCheckKind(this DateTime dt) => dt.Kind != DateTimeKind.Utc ? dt.ToUniversalTime() : dt;

        public static DateTime? ToLocal(this DateTime? dt) => dt?.ToLocalTime();

        public static DateTime? ToUtc(this DateTime? dt) => dt?.ToUniversalTime();

        public static DateTime? ToUtcCheckKind(this DateTime? dt) => dt.HasValue && dt.Value.Kind != DateTimeKind.Utc ? dt?.ToUniversalTime() : dt;

        public static DateTime? TryToLocal(this string dtText) => !string.IsNullOrEmpty(dtText) && DateTime.TryParse(dtText, out DateTime dt) ? (DateTime?)dt.ToLocalTime() : null;

        public static DateTime? TryToUtc(this string dtText) => !string.IsNullOrEmpty(dtText) && DateTime.TryParse(dtText, out DateTime dt) ? (DateTime?)dt.ToUniversalTime() : null;

        public static T ToLocal<T>(this T item, params Expression<Func<T, DateTime>>[] propertyExpressions) where T : class
        {
            return new List<T>() { item }.SetValueFromDelegate((a) => a.ToLocalTime(), propertyExpressions).First();
        }

        public static T ToLocal<T>(this T item, params Expression<Func<T, DateTime?>>[] propertyExpressions) where T : class
        {
            return new List<T>() { item }.SetValueFromDelegate((a) => a?.ToLocalTime(), propertyExpressions).First();
        }

        public static T ToUtc<T>(this T item, params Expression<Func<T, DateTime>>[] propertyExpressions) where T : class
        {
            return new List<T>() { item }.SetValueFromDelegate((a) => a.ToUniversalTime(), propertyExpressions).First();
        }

        public static T ToUtc<T>(this T item, params Expression<Func<T, DateTime?>>[] propertyExpressions) where T : class
        {
            return new List<T>() { item }.SetValueFromDelegate((a) => a?.ToUniversalTime(), propertyExpressions).First();
        }

        public static List<T> ToLocal<T>(this List<T> items, params Expression<Func<T, DateTime>>[] propertyExpressions) where T : class
        {
            return items.SetValueFromDelegate((a) => a.ToLocalTime(), propertyExpressions);
        }

        public static List<T> ToLocal<T>(this List<T> items, params Expression<Func<T, DateTime?>>[] propertyExpressions) where T : class
        {
            return items.SetValueFromDelegate((a) => a?.ToLocalTime(), propertyExpressions);
        }

        public static List<T> ToUtc<T>(this List<T> items, params Expression<Func<T, DateTime>>[] propertyExpressions) where T : class
        {
            return items.SetValueFromDelegate((a) => a.ToUniversalTime(), propertyExpressions);
        }

        public static List<T> ToUtc<T>(this List<T> items, params Expression<Func<T, DateTime?>>[] propertyExpressions) where T : class
        {
            return items.SetValueFromDelegate((a) => a?.ToUniversalTime(), propertyExpressions);
        }

        private static List<T> SetValueFromDelegate<T, R>(this List<T> items, Func<R, R> getValue, params Expression<Func<T, R>>[] propertyExpressions) where T : class
        {
            var itemDummy = default(T);
            var propertiesNames = propertyExpressions?.Select(r => itemDummy.GetPropertyName(r)).ToList();
            items?.ForEach(item =>
            {
                propertiesNames?.ForEach(name =>
                {
                    var currentValue = item.GetPropertyValue<T, R>(name);
                    item.SetPropertyValue(name, getValue(currentValue));
                });
            });
            return items;
        }
        #endregion

        #region TimeZoneInfo
        private static object objLock = new object();
        private static DateTime timeLife = DateTime.Now;

        public static bool IsAmbiguousTime(this DateTime dt)
        {
            return IsAmbiguousTime(dt as DateTime?);
        }

        public static bool IsAmbiguousTime(this DateTime? dt)
        {
            lock (objLock)
            {
                if (timeLife.AddHours(4) <= DateTime.Now)
                {
                    TimeZoneInfo.ClearCachedData();
                }
                if (dt.HasValue)
                {
                    return TimeZoneInfo.Local.IsAmbiguousTime(dt.Value);
                }
                return false;
            }
        }
        #endregion

        #region Truncate
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime;
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        public static DateTime TruncateMiliSeconds(this DateTime dateTime)
        {
            return Truncate(dateTime, TimeSpan.FromMilliseconds(10));
        }
        #endregion

        public static bool IsBetween(this DateTime input, DateTime date1, DateTime date2)
        {
            return input > date1 && input < date2;
        }

        public static bool IsBetweenUtcNowSeconds(this DateTime input, int seconds)
        {
            var date = DateTime.UtcNow;
            return date > input.AddSeconds(-seconds) && date < input.AddSeconds(seconds);
        }

        public static int NumberOfMonthBetweenThisDateAnd(this DateTime input, DateTime secondDate)
        {
            return Math.Abs(((secondDate.Year - input.Year) * 12) + secondDate.Month - input.Month);
        }

        public static int NumberOfSecondsBetweenThisDateAnd(this DateTime input, DateTime secondDate)
        {
            return Convert.ToInt32(Math.Abs((secondDate - input).TotalSeconds));
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
