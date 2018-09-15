using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EjemplosFormacion.HelperClasess.ExtensionMethods
{
    public static class PropertyExtensions
    {
        public static TReturnType GetPropertyValue<T, TReturnType>(this T source, string propertyName)
        {
            var sourceType = source.GetType();
            var propertyValue = sourceType.GetProperties().Where(r => r.Name.Equals(propertyName)).Select(r => r.GetValue(source, null)).FirstOrDefault();
            return propertyValue != null ? (TReturnType)propertyValue : default(TReturnType);
        }

        public static PropertyInfo GetPropertyInfo<T, TReturnType>(this T source, Expression<Func<T, TReturnType>> propertyExpressio)
        {
            var sourceType = typeof(T);
            return sourceType.GetProperties().Where(r => r.Name.Equals(source.GetPropertyName(propertyExpressio))).FirstOrDefault();
        }

        public static Type GetPropertyType<T, TReturnType>(this T source, Expression<Func<T, TReturnType>> propertyExpression)
        {
            var sourceType = source.GetType();
            return sourceType.GetProperties().Where(r => r.Name.Equals(source.GetPropertyName(propertyExpression))).Select(r => r.PropertyType).FirstOrDefault();
        }

        public static TReturnType GetPropertyValue<T, TReturnType>(this T source, Expression<Func<T, TReturnType>> propertyExpression)
        {
            return source.GetPropertyValue<T, TReturnType>(source.GetPropertyName(propertyExpression));
        }

        public static void SetPropertyValue<T, TReturnType>(this T source, Expression<Func<T, TReturnType>> propertyExpression, TReturnType value)
        {
            source.SetPropertyValue(source.GetPropertyName(propertyExpression), value);
        }

        public static void SetPropertyValue<T, TReturnType>(this T source, string propertyName, TReturnType value)
        {
            var sourceType = source.GetType();
            var propertyInfo = sourceType.GetProperties().Where(r => r.Name.Equals(propertyName)).FirstOrDefault();
            propertyInfo.SetValue(source, value, null);
        }

        public static object GetPropertyValue(this object source, string property)
        {
            if (source == null)
                throw new ArgumentException("source");

            var sourceType = source.GetType();
            var sourceProperties = sourceType.GetProperties();

            var propertyValue = (from s in sourceProperties
                                 where s.Name.Equals(property)
                                 select s.GetValue(source, null)).FirstOrDefault();

            return propertyValue != null ? propertyValue : null;
        }

        public static string GetPropertyName<TSource, TReturnType>(this TSource obj, Expression<Func<TSource, TReturnType>> propertyExpression)
        {
            var visitor = new ExprVisitor();
            visitor.Visit(propertyExpression);
            if (visitor.IsFound)
                return visitor.MemberName;
            else
                throw new ArgumentException(string.Format("Expression is not a MemberExpression to: {0}", obj), "propertyExpression");
        }

        public static string GetPropertyName<TEntity, TReturnType>(this Expression<Func<TEntity, TReturnType>> propertyExpression) where TEntity : class
        {
            var visitor = new ExprVisitor();
            visitor.Visit(propertyExpression);
            if (visitor.IsFound)
                return visitor.MemberName;
            else
                throw new ArgumentException("Expression is not a MemberExpression", "propertyExpression");
        }

        public static List<string> GetAllPropertiesNamesToLower(this Type currentType)
        {
            return currentType.GetProperties().Select(r => r.Name.ToLower()).ToList();
        }

        internal class ExprVisitor : ExpressionVisitor
        {
            public bool IsFound { get; private set; }
            public string MemberName { get; private set; }
            protected override Expression VisitMember(MemberExpression node)
            {
                if (!IsFound && node.Member.MemberType == MemberTypes.Property)
                {
                    IsFound = true;
                    MemberName = node.Member.Name;
                }
                return base.VisitMember(node);
            }
        }
    }
}
