using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace EjemplosFormacion.WebApi.DirectRouteFactories
{
    /// <summary>
    /// Custom Direct Route Factory (Es lo equivalente al RouteAttribute), se usa para albergar custom informacion sobre una Direct Route para un Action
    /// En este caso esta implementacion se usa para albergar informacion que sera usada para implementar unas Custom Direct Route de manera Tipada 
    /// Con la ayuda de un Extension Method del HttpConfiguration que llenara esta informacion y el Direct Route Provider que leera esta informacion para registrar la Route
    /// </summary>
    class TestTypedDirectRouteFactory : IDirectRouteFactory
    {
        public Type ControllerType { get; private set; }

        public string RouteName { get; private set; }

        public string Template { get; private set; }

        public string ControllerName
        {
            get { return ControllerType != null ? ControllerType.FullName : string.Empty; }
        }

        public string ActionName { get; private set; }

        public MethodInfo ActionMember { get; private set; }

        public TestTypedDirectRouteFactory(string template)
        {
            if (string.IsNullOrWhiteSpace(template)) throw new ArgumentException("template vacio!.");

            Template = template;
        }

        RouteEntry IDirectRouteFactory.CreateRoute(DirectRouteFactoryContext context)
        {
            IDirectRouteBuilder builder = context.CreateBuilder(Template);

            builder.Name = RouteName;
            return builder.Build();
        }

        public TestTypedDirectRouteFactory Controller<TController>() where TController : IHttpController
        {
            ControllerType = typeof(TController);
            return this;
        }

        public TestTypedDirectRouteFactory Action<T, U>(Expression<Func<T, U>> expression)
        {
            ActionMember = GetMethodInfoInternal(expression);
            ControllerType = ActionMember.DeclaringType;
            ActionName = ActionMember.Name;
            return this;
        }

        public TestTypedDirectRouteFactory Action<T>(Expression<Action<T>> expression)
        {
            ActionMember = GetMethodInfoInternal(expression);
            ControllerType = ActionMember.DeclaringType;
            ActionName = ActionMember.Name;
            return this;
        }

        public TestTypedDirectRouteFactory Name(string name)
        {
            RouteName = name;
            return this;
        }

        public TestTypedDirectRouteFactory Action(string actionName)
        {
            ActionName = actionName;
            return this;
        }

        private static MethodInfo GetMethodInfoInternal(LambdaExpression expressionBody)
        {
            var method = expressionBody.Body as MethodCallExpression;
            if (method != null)
                return method.Method;

            throw new ArgumentException("Expression is incorrect!");
        }
    }
}