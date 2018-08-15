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
    /// Con la ayuda de un Extension Method del HttpConfiguration que llenara esta informacion 
    /// El Direct Route Provider que leera esta informacion para registrar la Route
    /// </summary>
    class TestTypedDirectRouteFactory : IDirectRouteFactory
    {
        public Type ControllerType { get; private set; }

        public string ActionName { get; private set; }

        private readonly int? _order;
        private readonly string _routeName;
        private readonly string _routeTemplate;

        public TestTypedDirectRouteFactory(string routeTemplate, string routeName = null, int? order = null)
        {
            if (string.IsNullOrWhiteSpace(routeTemplate)) throw new ArgumentException("route template vacio!.");

            _order = order;
            _routeName = routeName;
            _routeTemplate = routeTemplate;
        }

        // Metodo llamado por Web Api para crear un RouteEntry segun el template entregado y un Route Name si existe
        RouteEntry IDirectRouteFactory.CreateRoute(DirectRouteFactoryContext context)
        {
            IDirectRouteBuilder routeBuilder = context.CreateBuilder(_routeTemplate);
            routeBuilder.Name = _routeName;
            if (_order.HasValue)
            {
                routeBuilder.Order = _order.Value;
            }

            return routeBuilder.Build();
        }

        // Metodo llamado por nosotros para configurar el Type del Controller y el nombre del Action
        // De esta manera nuestro custom Direct Route Provider leera estas propiedas y asignara esta Route al Controller y Action que hemos configurado en este metodo
        public void ConfigureRoute<T>(Expression<Action<T>> expression) where T : IHttpController
        {
            MethodInfo actionMember = GetMethodInfoInternal(expression);
            ControllerType = actionMember.DeclaringType;
            ActionName = actionMember.Name;
        }

        private static MethodInfo GetMethodInfoInternal(LambdaExpression expressionBody)
        {
            var method = expressionBody.Body as MethodCallExpression;
            if (method != null)
            {
                return method.Method;
            }
            else
            {
                throw new ArgumentException("Expression is incorrect!");
            }
        }
    }
}