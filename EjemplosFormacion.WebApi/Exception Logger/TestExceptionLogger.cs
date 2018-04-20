﻿using EjemplosFormacion.HelperClasess.Abstract;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Unity.Attributes;

namespace EjemplosFormacion.WebApi.ExceptionLogger
{
    /// <summary>
    /// Exception loggers are the solution to seeing all unhandled exception caught by Web API.
    /// Se usa para hacer Logger a cualquier Exception que ocurra en el Web Api que no haya sido handleada por un Action, esta clase hara Logger de la Exception indiferentemente que la Excepcion halla sido handleada por un ExceptionFilter o un ExceptionHandler, *de hecho esta clase es llamada primero que los ExceptionFilter y ExceptionHandler*
    /// Las excepciones del tipo HttpResponseException son un caso especial y no llamaran al ExceptionHandler
    /// </summary>
    class TestExceptionLogger : IExceptionLogger
    {
        [Dependency]
        public IWrapperLogger Logger { get; set; }

        // Metodo del contrato de IExceptionLogger que se debe implementar
        // Aqui puedes revisar la informacion del ExceptionLoggerContext para obtener toda la informacion acerca de la excepcion y loggear segun sea el caso
        public virtual Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            Logger.Error(context.ExceptionContext.Exception.ToString());

            return Task.FromResult(0);
        }
    }
}