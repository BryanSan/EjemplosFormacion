using System.Reflection;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EjemplosFormacion.EntityFramework6
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// For an Entity Framework IQueryable, returns the SQL with inlined Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ToTraceQuery<T>(this IQueryable<T> query)
        {
            ObjectQuery<T> objectQuery = GetQueryFromQueryable(query);

            var result = objectQuery.ToTraceString();
            foreach (var parameter in objectQuery.Parameters)
            {
                var name = "@" + parameter.Name;
                var value = "'" + parameter.Value.ToString() + "'";
                result = result.Replace(name, value);
            }

            return result;
        }

        /// <summary>
        /// For an Entity Framework IQueryable, returns the SQL and Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ToTraceString<T>(this IQueryable<T> query)
        {
            ObjectQuery<T> objectQuery = GetQueryFromQueryable(query);

            StringBuilder traceString = new StringBuilder();

            traceString.AppendLine(objectQuery.ToTraceString());
            traceString.AppendLine();

            foreach (var parameter in objectQuery.Parameters)
            {
                traceString.AppendLine(parameter.Name + " [" + parameter.ParameterType.FullName + "] = " + parameter.Value);
            }

            return traceString.ToString();
        }

        private static ObjectQuery<T> GetQueryFromQueryable<T>(IQueryable<T> query)
        {
            var internalQueryField = query.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => f.Name.Equals("_internalQuery")).FirstOrDefault();
            var internalQuery = internalQueryField.GetValue(query);
            var objectQueryField = internalQuery.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => f.Name.Equals("_objectQuery")).FirstOrDefault();
            return objectQueryField.GetValue(internalQuery) as ObjectQuery<T>;
        }
    }
}
