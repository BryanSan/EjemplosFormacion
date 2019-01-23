using System;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IAsymmetricRSADecrypter : IDisposable
    {
        byte[] DecryptWithFullKeyXmlBytes(byte[] cipherBytes);
        byte[] DecryptWithFullKeyXmlBytes(string cipherString);

        T DecryptWithFullKeyXml<T>(byte[] cipherBytes) where T : class;
        T DecryptWithFullKeyXml<T>(string cipherString) where T : class;
    }
}
