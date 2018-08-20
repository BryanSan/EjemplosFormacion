using EjemplosFormacion.HelperClasess.FullDotNet.Abstract;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses
{
    public class AzureStreamWriterBlob : IAzureStreamWriterBlob
    {
        private readonly CloudBlobStream _cloudBlobStream;

        public Stream BlobStream { get { return _cloudBlobStream; } }

        public AzureStreamWriterBlob(CloudBlobStream cloudBlobStream)
        {
            _cloudBlobStream = cloudBlobStream;
        }

        public void Commit()
        {
            _cloudBlobStream.Commit();
        }

    }
}