using EjemplosFormacion.WebApi.ActionsFilters.OrderedFilters.Infraestructure.Abstract;
using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.ActionsFilters.OrderedFilters.Infraestructure
{
    /// <summary>
    /// Clase padre de las cuales clases hijas heredaran para dar soporte a Orden de ejecucion
    /// </summary>
    public abstract class ExceptionFilterWithOrderAttribute : ExceptionFilterAttribute, IOrderedFilter
    {
        public int Order { get; set; }
    }
}