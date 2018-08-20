using EjemplosFormacion.HelperClasess.FullDotNet.Abstract;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    public class AzureReadOnlyBlob : IAzureReadOnlyBlob
    {
        private readonly CloudBlob _cloudBlob;

        public string Name { get => _cloudBlob.Name; }
        public Uri Url { get => _cloudBlob.Uri; }
        public IDictionary<string, string> Metadata { get => _cloudBlob.Metadata; }

        public AzureReadOnlyBlob(CloudBlob cloudBlob)
        {
            _cloudBlob = cloudBlob;
        }
    }
}
