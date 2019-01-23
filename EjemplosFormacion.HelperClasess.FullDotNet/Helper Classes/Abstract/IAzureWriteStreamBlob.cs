using System.IO;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses.Abstract
{
    public interface IAzureStreamWriterBlob
    {
        Stream BlobStream { get; }
        void Commit();
    }
}
