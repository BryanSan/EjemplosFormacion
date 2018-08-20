using System.IO;

namespace EjemplosFormacion.HelperClasess.FullDotNet.Abstract
{
    public interface IAzureStreamWriterBlob
    {
        Stream BlobStream { get; }
        void Commit();
    }
}
