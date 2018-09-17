using System;
using System.Collections.Generic;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IAsymmetricRSADecrypter
    {
        byte[] DecryptWithFullKeyXmlBytes(byte[] cipherBytes);
        byte[] DecryptWithFullKeyXmlBytes(string cipherString);

        T DecryptWithFullKeyXml<T>(byte[] cipherBytes) where T : class;
        T DecryptWithFullKeyXml<T>(string cipherString) where T : class;
    }
}
