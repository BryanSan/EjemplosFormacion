using System;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IAsymmetricRSAEncrypter : IDisposable
    {
        byte[] EncryptWithPublicKeyXmlBytes(byte[] bytesToEncrypt);
        byte[] EncryptWithPublicKeyXmlBytes<T>(T objectToEncrypt) where T : class;

        string EncryptWithPublicKeyXml(byte[] bytesToEncrypt);
        string EncryptWithPublicKeyXml<T>(T objectToEncrypt) where T : class;
    }
}