using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EjemplosFormacion.HelperClasess.Abstract
{
    public interface IConstruirTableExcel
    {
        byte[] ConstruirTableExcelFrom<T>(List<T> listaParaExportar, string nombreHoja, bool modoPropiedadInclusivas, params Expression<Func<T, object>>[] propertiesToExport) where T : class;
    }
}
