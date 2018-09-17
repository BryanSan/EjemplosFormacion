using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IDigitalCertificateService
    {
        X509Certificate2 GetCertificate(string searchValue, X509FindType findType, StoreName storeName, StoreLocation storeLocation, bool validOnly);

        void AddCertificate(X509Certificate2 certificateToAdd, StoreName storeName, StoreLocation storeLocation);
        void AddCertificate(string certificatePath, StoreName storeName, StoreLocation storeLocation);

        void RemoveCertificate(string searchValue, X509FindType findType, StoreName storeName, StoreLocation storeLocation, bool validOnly);

        byte[] ExportCertificate(string searchValue, X509ContentType contentType, X509FindType findType, StoreName storeName, StoreLocation storeLocation, bool validOnly);
        byte[] ExportCertificate(string searchValue, X509ContentType contentType, string password, X509FindType findType, StoreName storeName, StoreLocation storeLocation, bool validOnly);
    }
}
