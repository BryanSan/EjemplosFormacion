﻿using System.Collections.Generic;

namespace EjemplosFormacion.WebApi.Stubs.Models
{
    class TestModelError
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