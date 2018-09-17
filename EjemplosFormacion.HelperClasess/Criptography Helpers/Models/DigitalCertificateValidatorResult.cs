using System;
using System.Collections.Generic;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Models
{
    public class DigitalCertificateValidatorResult
    {
        public bool Valid { get; internal set; }
        public List<string> StatusInformation { get; internal set; }

        public DigitalCertificateValidatorResult(bool valid, List<string> statusInformation)
        {
            Valid = valid;
            StatusInformation = statusInformation;
        }

        public DigitalCertificateValidatorResult(bool valid, string statusInformation) : this(valid, new List<string> { statusInformation })
        {

        }

        public DigitalCertificateValidatorResult()
        {

        }
    }
}