using System;
using System.Collections.Generic;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses.Abstract
{
    public interface IAzureReadOnlyBlob
    {
        string Name { get; }
        Uri Url { get; }
        IDictionary<string, string> Metadata { get; }
    }
}
