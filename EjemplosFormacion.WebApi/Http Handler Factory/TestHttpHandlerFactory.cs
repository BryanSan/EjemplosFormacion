﻿using EjemplosFormacion.WebApi.HttpHandler;
using System;
using System.Web;

namespace EjemplosFormacion.WebApi.HttpHandlerFactory
{
    /// <summary>
    /// HttpHandlerFactory es un generador de controladores 
    /// Ejecuta codigo para crear y retornar HttpHandlers segun alguna logica que hallas creado
    /// Normalmente inspeccionaras la URL en busca de un Match y crearas y retornaras un HttpHandler para esta URL que has hecho match
    /// You can generate a new handler instance for each HTTP request by creating a class that implements the IHttpHandlerFactory interface. 
    /// </summary>
    public class TestHttpHandlerFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, String url, String pathTranslated)
        {
            IHttpHandler handlerToReturn;

            context.Response.Write("Hello from Http Handler Factory!" + Environment.NewLine);

            if ("get" == context.Request.RequestType.ToLower())
            {
                handlerToReturn = new TestSyncHttpHandler();
            }
            else if ("post" == context.Request.RequestType.ToLower())
            {
                handlerToReturn = new TestAsyncHttpHandler();
            }
            else
            {
                handlerToReturn = null;
            }

            context.Response.Write("EndProcessRequest of Http Handler Factory.");

            return handlerToReturn;
        }
        public void ReleaseHandler(IHttpHandler handler)
        {
        }
        public bool IsReusable
        {
            get
            {
                // To enable pooling, return true here.
                // This keeps the handler in memory.
                return false;
            }
        }
    }
}