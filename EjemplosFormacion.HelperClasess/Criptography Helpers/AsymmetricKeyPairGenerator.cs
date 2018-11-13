using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using EjemplosFormacion.HelperClasess.CriptographyHelpers.Models;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    /// <summary>
    /// https://dotnetcodr.com/2016/10/25/overview-of-asymmetric-encryption-in-net/
    /// </summary>
    public class AsymmetricKeyPairGenerator<TAsymmetricAlgorithm> : IAsymmetricKeyPairGenerator<TAsymmetricAlgorithm>
        where TAsymmetricAlgorithm : AsymmetricAlgorithm, new()
    {
        public AsymmetricKeyPairGenerationResult GenerateKeysAsXml(int keySizeBits)
        {
            if (keySizeBits <= 0) throw new ArgumentException($"{nameof(keySizeBits)} no puede ser negativo");

            using (TAsymmetricAlgorithm asymmetricAlgorithm = new TAsymmetricAlgorithm())
            {
                asymmetricAlgorithm.KeySize = keySizeBits;
                
                try
                {
                    string publicKeyXml = asymmetricAlgorithm.ToXmlString(false);
                    string publicPrivateKeyPairXml = asymmetricAlgorithm.ToXmlString(true);

                    var asymmetricKeyPairGenerationResult = new AsymmetricKeyPairGenerationResult(publicKeyXml, publicPrivateKeyPairXml);

                    return asymmetricKeyPairGenerationResult;
                }
                catch (CryptographicException cex)
                {
                    string NL = Environment.NewLine;
                    StringBuilder validKeySizeBuilder = new StringBuilder();

                    KeySizes[] validKeySizes = asymmetricAlgorithm.LegalKeySizes;
                    foreach (KeySizes keySizes in validKeySizes)
                    {
                        validKeySizeBuilder.Append("Min: ")
                            .Append(keySizes.MinSize).Append(NL)
                            .Append("Max: ").Append(keySizes.MaxSize).Append(NL)
                            .Append("Step: ").Append(keySizes.SkipSize);
                    }

                    throw new Exception($"Cryptographic exception when generating a key-pair of size {keySizeBits}. Exception: {cex.Message}{NL}Make sure you provide a valid key size. Here are the valid key size boundaries:{NL}{validKeySizeBuilder.ToString()}", cex);
                }
                catch (Exception otherEx)
                {
                    throw new Exception($"Other exception caught while generating the key pair: {otherEx.Message}", otherEx);
                }
            }
        }

        public AsymmetricKeyPairGenerationResult GenerateKeysFromCertificateAsXml(X509Certificate2 certificate)
        {
            if (certificate == null) throw new ArgumentException($"{nameof(certificate)} no puede ser nulo");

            // Debe ser RSA para que funcione tanto en .Net como en .Net Core
            // Ya que hay implementaciones distintas para cada plataforma
            // https://stackoverflow.com/questions/41986995/implement-rsa-in-net-core
            string publicKeyXml = certificate.GetRSAPublicKey().ToXmlString(false);
            string publicPrivateKeyPairXml = certificate.GetRSAPrivateKey().ToXmlString(true);

            var asymmetricKeyPairGenerationResult = new AsymmetricKeyPairGenerationResult(publicKeyXml, publicPrivateKeyPairXml);

            return asymmetricKeyPairGenerationResult;
        }
    }
}
