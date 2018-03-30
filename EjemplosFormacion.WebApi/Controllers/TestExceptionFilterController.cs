﻿using EjemplosFormacion.WebApi.Filters.ExceptionFilters;
using EjemplosFormacion.WebApi.Filters.OrderedFilters.ExceptionFilters;
using System;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.Controllers
{
    [TestExceptionFilter] // Excepcion Filter
    [TestOrderedExceptionFilter(Order = 1)] // Ordered Excepcion Filter - Primero en Ejecutar
    [TestOrderedExceptionFilter(Order = 2)] // Ordered Excepcion Filter - Segundo en Ejecutar
    public class TestExceptionFilterController : ApiController
    {
        [TestExceptionFilter] // Excepcion Filter
        [TestOrderedExceptionFilter(Order = 1)] // Ordered Excepcion Filter - Primero en Ejecutar
        [TestOrderedExceptionFilter(Order = 2)] // Ordered Excepcion Filter - Segundo en Ejecutar
        public IHttpActionResult TestExcepcionFilter()
        {
            throw new NotImplementedException();
        }
    }
}
