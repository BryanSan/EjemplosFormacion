using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using EjemplosFormacion.HelperClasess.CriptographyHelpers.Models;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    /// <summary>
    /// https://dotnetcodr.com/2013/11/14/introduction-to-digital-signatures-in-net-cryptography/
    /// </summary>
    public class DigitalSignatureVerifier<THashAlgorithm> : IDigitalSignatureVerifier<THashAlgorithm>
        where THashAlgorithm : HashAlgorithm, new()
    {
        readonly Lazy<RSA> _senderCipher;
        readonly Lazy<RSA> _receiverCipher;
        readonly IHasher<THashAlgorithm> _hasher;

        public DigitalSignatureVerifier(string myPublicPrivateRSAKey, string senderPublicRSAKey, IHasher<THashAlgorithm> hasher)
        {
            // Debe ser RSA para que funcione tanto en .Net como en .Net Core
            // Ya que hay implementaciones distintas para cada plataforma
            // https://stackoverflow.com/questions/41986995/implement-rsa-in-net-core
            _receiverCipher = new Lazy<RSA>(() =>
            {
                var receiverCipher = RSA.Create();
                receiverCipher.FromXmlString(myPublicPrivateRSAKey);

                return receiverCipher;
            });

            _senderCipher = new Lazy<RSA>(() =>
            {
                var senderCipher = RSA.Create();
                senderCipher.FromXmlString(senderPublicRSAKey);

                return senderCipher;
            });

            _hasher = hasher;
        }

        public T ExtractMessage<T>(DigitalSignatureResult signatureToExtract)
        {
            if (signatureToExtract == null || string.IsNullOrWhiteSpace(signatureToExtract.CipherText) || string.IsNullOrWhiteSpace(signatureToExtract.SignatureText))
            {
                throw new Exception($"{nameof(signatureToExtract)} invalido o vacio");
            }

            byte[] cipherBytes = Convert.FromBase64String(signatureToExtract.CipherText);
            byte[] signatureBytes = Convert.FromBase64String(signatureToExtract.SignatureText);
            
            byte[] recomputedHash = _hasher.GetByteHash(cipherBytes);

            bool validSignature = VerifySignature(recomputedHash, signatureBytes);
            if (validSignature)
            {
                throw new ApplicationException("Signature did not match from sender");
            }

            T entity = DecrypBytes<T>(cipherBytes);

            return entity;
        }

        public bool VerifySignature(DigitalSignatureResult signatureToVerify)
        {
            if (signatureToVerify == null || string.IsNullOrWhiteSpace(signatureToVerify.CipherText) || string.IsNullOrWhiteSpace(signatureToVerify.SignatureText))
            {
                throw new Exception($"{nameof(signatureToVerify)} invalido o vacio");
            }

            byte[] cipherBytes = Convert.FromBase64String(signatureToVerify.CipherText);
            byte[] signatureBytes = Convert.FromBase64String(signatureToVerify.SignatureText);

            byte[] recomputedHash = _hasher.GetByteHash(cipherBytes);

            bool validSignature = VerifySignature(recomputedHash, signatureBytes);

            return validSignature;
        }

        bool VerifySignature(byte[] recomputedHash, byte[] signatureBytes)
        {
            RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(_senderCipher.Value);
            deformatter.SetHashAlgorithm(_hasher.HashAlgorithmName);
            
            bool validSignature = deformatter.VerifySignature(recomputedHash, signatureBytes);

            return validSignature;
        }

        T DecrypBytes<T>(byte[] cipherBytes)
        {
            byte[] plainTextBytes = _receiverCipher.Value.Decrypt(cipherBytes, RSAEncryptionPadding.OaepSHA512);

            string decryptedEntity = Encoding.UTF8.GetString(plainTextBytes);
            T entity = JsonConvert.DeserializeObject<T>(decryptedEntity);

            return entity;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_senderCipher.IsValueCreated)
                    {
                        _senderCipher.Value.Clear();
                        _senderCipher.Value.Dispose();
                    }

                    if (_receiverCipher.IsValueCreated)
                    {
                        _receiverCipher.Value.Clear();
                        _receiverCipher.Value.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}