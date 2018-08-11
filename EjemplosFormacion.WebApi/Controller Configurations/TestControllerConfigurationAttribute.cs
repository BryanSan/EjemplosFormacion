using EjemplosFormacion.WebApi.MediaTypeFormatters;
using EjemplosFormacion.WebApi.TraceWriters;
using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.Tracing;

namespace EjemplosFormacion.WebApi.ControllerConfigurations
{
    /// <summary>
    /// Custom Controller Configuration que sirve para hacerle override solo a la configuration de un Controller en especifico (el que se ha marcado con este Attribute)
    /// Solo colocalo como un Attribute al Controller que le quieres hacer Override
    /// Puedes hacerle override a 
    ///     Media-type formatters
    ///     Parameter binding rules
    ///     Services
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    class TestControllerConfigurationAttribute : Attribute, IControllerConfiguration
    {
        // Metodo que sera llamado para aplicar la configuracion
        // Inspecciona el HttpControllerDescriptor para obtener informacion acerca del Controller y decidir segun sea el caso
        // Usa el HttpControllerSettings para hacer las configuraciones necesarias
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            controllerSettings.Formatters.Add(new TestAtomMediaTypeFormatter());

            controllerSettings.Services.Replace(typeof(ITraceWriter), new TestTraceWriter());

            ParameterBindingRulesCollection parameterBindingRules = controllerSettings.ParameterBindingRules;
        } 
    }
}