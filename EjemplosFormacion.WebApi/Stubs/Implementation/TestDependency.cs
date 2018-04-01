using EjemplosFormacion.WebApi.Stubs.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EjemplosFormacion.WebApi.Stubs.Implementation
{
    public class TestDependency : ITestDependency, IDisposable
    {
        public void Dispose()
        {
            
        }
    }
}