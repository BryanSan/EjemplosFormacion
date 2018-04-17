using EjemplosFormacion.WebApi.Filters.OrderedFilters.Infraestructure.Abstract;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.OrderedFilters.Infraestructure
{
    /// <summary>
    /// Clase padre de las cuales clases hijas heredaran para dar soporte a Orden de ejecucion
    /// </summary>
    abstract class ActionFilterWithOrderAttribute : ActionFilterAttribute, IOrderedFilter
    {
        public int Order { get; set; }
    }
}