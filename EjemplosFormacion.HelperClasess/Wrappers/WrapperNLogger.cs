using EjemplosFormacion.HelperClasess.Abstract;
using NLog;
using System;

namespace EjemplosFormacion.HelperClasess.Wrappers
{
    public class WrapperNLogger : IWrapperLogger
    {
        private readonly Logger _logger;

        public WrapperNLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }


        public void Debug(string message, Exception exception = null)
        {
            _logger.Debug(exception, message);
        }

        public void Error(string message, Exception exception = null)
        {
            _logger.Error(exception, message);
        }

        public void Fatal(string message, Exception exception = null)
        {
            _logger.Fatal(exception, message);
        }

        public void Info(string message, Exception exception = null)
        {
            _logger.Info(exception, message);
        }

        public void Warn(string message, Exception exception = null)
        {
            _logger.Warn(exception, message);
        }

        public void Trace(string message, Exception exception = null)
        {
            _logger.Trace(exception, message);
        }

        public void Debug(string message, params object[] parametros)
        {
            _logger.Debug(message, parametros);
        }

        public void Info(string message, params object[] parametros)
        {
            _logger.Info(message, parametros);
        }

        public void Error(string message, params object[] parametros)
        {
            _logger.Error(message, parametros);
        }
    }
}