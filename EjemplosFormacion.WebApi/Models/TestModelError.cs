using System.Collections.Generic;

namespace EjemplosFormacion.WebApi.Models
{
    public class TestModelError
    {
        public List<string> Errores { get; set; }

        public TestModelError(List<string> errores)
        {
            Errores = errores;
        }

        public TestModelError()
        {

        }
    }
}