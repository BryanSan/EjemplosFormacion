using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using EjemplosFormacion.HelperClasess.CriptographyHelpers.Models;
using System;
using System.Security.Cryptography.X509Certificates;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    public class DigitalCertificateService : IDigitalCertificateService
    {
        public X509Certificate2 GetCertificate(string searchValue, X509FindType findType, StoreName storeName, StoreLocation storeLocation, bool validOnly)
        {
            using (X509Store computerCaStore = new X509Store(storeName, storeLocation))
            {
                computerCaStore.Open(OpenFlags.ReadOnly);
                X509Certificate2 certificate = GetCertificate(searchValue, findType, validOnly, computerCaStore);

                return certificate;
            }
        }

        public AsymmetricKeyPairGenerationResult GetKeysFromCertificate(string searchValue, X509FindType findType, StoreName storeName, StoreLocation storeLocation, bool validOnly)
        {
            using (X509Store computerCaStore = new X509Store(storeName, storeLocation))
            {
                computerCaStore.Open(OpenFlags.ReadOnly);
                X509Certificate2 certificate = GetCertificate(searchValue, findType, validOnly, computerCaStore);

                if (certificate != null)
                {
                    AsymmetricKeyPairGenerationResult asymmetricKeyPairGenerationResult = GetKeysFromCertificate(certificate);
                    certificate.Dispose();

                    return asymmetricKeyPairGenerationResult;
                }
                else
                {
                    throw new Exception("Certificado no encontrado");
                }
            }
        }

        public AsymmetricKeyPairGenerationResult GetKeysFromCertificate(X509Certificate2 certificate)
        {
            // Debe ser RSA para que funcione tanto en .Net como en .Net Core
            // Ya que hay implementaciones distintas para cada plataforma
            // https://stackoverflow.com/questions/41986995/implement-rsa-in-net-core
            string publicKeyXml = certificate.GetRSAPublicKey().ToXmlString(false);
            string publicPrivateKeyPairXml = certificate.GetRSAPrivateKey().ToXmlString(true);

            var asymmetricKeyPairGenerationResult = new AsymmetricKeyPairGenerationResult(publicKeyXml, publicPrivateKeyPairXml);

            return asymmetricKeyPairGenerationResult;
        }

        public void AddCertificate(X509Certificate2 certificateToAdd, StoreName storeName, StoreLocation storeLocation)
        {
            using (X509Store computerCaStore = new X509Store(storeName, storeLocation))
            {
                computerCaStore.Open(OpenFlags.ReadWrite);
                computerCaStore.Add(certificateToAdd);
            }
        }

        public void AddCertificate(string certificatePath, StoreName storeName, StoreLocation storeLocation)
        {
            using (X509Certificate2 certificateToAdd = new X509Certificate2(certificatePath))
            {
                AddCertificate(certificateToAdd, storeName, storeLocation);
            }
        }

        public void RemoveCertificate(string searchValue, X509FindType findType, StoreName storeName, StoreLocation storeLocation, bool validOnly)
        {
            using (X509Store certificateStore = new X509Store(storeName, storeLocation))
            {
                certificateStore.Open(OpenFlags.ReadWrite);

                X509Certificate2 certificateToDelete = GetCertificate(searchValue, findType, validOnly, certificateStore);
                if (certificateToDelete != null)
                {
                    certificateStore.Remove(certificateToDelete);
                    certificateToDelete.Dispose();
                }
                else
                {
                    throw new Exception("Certificado no encontrado");
                }
            }
        }

        public byte[] ExportCertificate(string searchValue, X509ContentType contentType, string password, X509FindType findType, StoreName storeName, StoreLocation storeLocation, bool validOnly)
        {
            using (X509Store computerCaStore = new X509Store(storeName, storeLocation))
            {
                computerCaStore.Open(OpenFlags.ReadOnly);

                X509Certificate2 certificateToExport = GetCertificate(searchValue, findType, validOnly, computerCaStore);
                if (certificateToExport != null)
                {
                    byte[] certificateBytes = null;
                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        certificateBytes = certificateToExport.Export(contentType, password);
                    }
                    else
                    {
                        certificateBytes = certificateToExport.Export(contentType);
                    }

                    return certificateBytes;
                }
                else
                {
                    throw new Exception("Certificado no encontrado");
                }
            }
        }

        public byte[] ExportCertificate(string searchValue, X509ContentType contentType, X509FindType findType, StoreName storeName, StoreLocation storeLocation, bool validOnly)
        {
            byte[] certificateBytes = ExportCertificate(searchValue, contentType, null, findType, storeName, storeLocation, validOnly);

            return certificateBytes;
        }

        static X509Certificate2 GetCertificate(string searchValue, X509FindType findType, bool validOnly, X509Store computerCaStore)
        {
            X509Certificate2Collection certificatesInStore = computerCaStore.Certificates.Find(findType, searchValue, validOnly);

            if (certificatesInStore.Count > 0)
            {
                return certificatesInStore[0];
            }
            else
            {
                return null;
            }
        }
    }
}