using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EjemplosFormacion.WebApi.Models
{
    /// <summary>
    /// Entidad usada como modelo para los Action que reciban objetos
    /// </summary>
    public class TestModel
    {
        public string Nombre { get; set; }

        public int Edad { get; set; }
    }
}