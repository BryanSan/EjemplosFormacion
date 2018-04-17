using System.Web.Http.Filters;

namespace EjemplosFormacion.WebApi.Filters.OrderedFilters.Infraestructure.Abstract
{
    /// <summary>
    /// Interfaz que definira el contrato necesario para hacer que un filtro soporte orden de ejecucion
    /// </summary>
    interface IOrderedFilter : IFilter
    {
        int Order { get; set; }
    }
}