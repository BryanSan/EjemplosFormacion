using System;
using System.Net.Http;
using System.Web.Http.Tracing;

namespace EjemplosFormacion.WebApi.TraceWriters
{
    /// <summary>
    /// Custom Implementacion del servicio Web Api ITraceWriter para hacerle Trace a cada etapa del messaging pipeline
    /// Web Api llamara el metodo Trace antes y despues de cada etapa del message pipeline, pasandole los parametros que el considere
    /// Usalo para hacerle Trace (el de System.Diagnostics) a esta informacion y que los TraceListener lo persistan como consideren oportuno
    /// O simplemente no le hagas Trace y persistelo tu mismo
    /// POR DEFECTO WEB API NO TIENE UN ITRACEWRITER CONFIGURADO Y NO TRACEARA NADA, DEBES DAR TU PROPIA IMPLEMENTACION PARA QUE SE ACTIVE EL TRACING
    /// https://docs.microsoft.com/es-es/aspnet/web-api/overview/testing-and-debugging/tracing-in-aspnet-web-api
    /// </summary>
    public class TestTraceWriter : ITraceWriter
    {
        // Metodo que Web Api llamara antes y despues de cada etapa del message pipeline, pasandole los parametros que el considere
        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            // Inicializas un TraceRecord con los parametros del Trace Method
            var record = new TraceRecord(request, category, level);

            // Llamas al Action pasado como parametro al metodo Trace pasandole el TraceRecord
            // Este Action llenara el TraceRecord que has pasado con la informacion que el considere oportuna
            traceAction(record);

            // Custom method propio para que persista el TraceRecord
            // En este caso solo le hace un Trace para que los TraceListener lo persistan como lo consideren oportuno
            PersistTrace(record);
        }

        // Custom Method propio para hacerle Trace al TraceRecord buildeado y que los TraceListener configurados lo persistan como consideren oportuno
        protected void PersistTrace(TraceRecord record)
        {
            string message = string.Format("The message is: {0};{1};{2}", record.Operator, record.Operation, record.Message);
            System.Diagnostics.Trace.WriteLine(message, record.Category);
        }
    }
}