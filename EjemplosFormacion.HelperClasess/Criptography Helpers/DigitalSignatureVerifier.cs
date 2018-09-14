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
        readonly Lazy<RSACryptoServiceProvider> _senderCipher;
        readonly Lazy<RSACryptoServiceProvider> _receiverCipher;
        readonly IHasher<THashAlgorithm> _hasher;

        public DigitalSignatureVerifier(string myPublicPrivateRSAKey, string senderPublicRSAKey, IHasher<THashAlgorithm> hasher)
        {
            _receiverCipher = new Lazy<RSACryptoServiceProvider>(() =>
            {
                var receiverCipher = new RSACryptoServiceProvider();
                receiverCipher.FromXmlString(myPublicPrivateRSAKey);

                return receiverCipher;
            });

            _senderCipher = new Lazy<RSACryptoServiceProvider>(() =>
            {
                var senderCipher = new RSACryptoServiceProvider();
                senderCipher.FromXmlString(senderPublicRSAKey);

                return senderCipher;
            });

            _hasher = hasher;
        }

        public T ExtractMessage<T>(DigitalSignatureResult signatureResult)
        {
            byte[] cipherBytes = Convert.FromBase64String(signatureResult.CipherText);
            byte[] signatureBytes = Convert.FromBase64String(signatureResult.SignatureText);
            
            byte[] recomputedHash = _hasher.GetByteHash(cipherBytes);

            bool validSignature = VerifySignature(recomputedHash, signatureBytes);
            if (validSignature)
            {
                throw new ApplicationException("Signature did not match from sender");
            }

            T entity = DecrypBytes<T>(cipherBytes);

            return entity;
        }

        public bool VerifySignature(DigitalSignatureResult signatureResult)
        {
            byte[] cipherBytes = Convert.FromBase64String(signatureResult.CipherText);
            byte[] signatureBytes = Convert.FromBase64String(signatureResult.SignatureText);

            byte[] recomputedHash = _hasher.GetByteHash(cipherBytes);

            bool validSignature = VerifySignature(recomputedHash, signatureBytes);

            return validSignature;
        }

        bool VerifySignature(byte[] recomputedHash, byte[] signatureBytes)
        {
            RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(_senderCipher.Value);
            deformatter.SetHashAlgorithm("SHA1");
            
            bool validSignature = deformatter.VerifySignature(recomputedHash, signatureBytes);

            return validSignature;
        }

        T DecrypBytes<T>(byte[] cipherBytes)
        {
            byte[] plainTextBytes = _receiverCipher.Value.Decrypt(cipherBytes, false);

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
                        _senderCipher.Value.Dispose();
                    }

                    if (_receiverCipher.IsValueCreated)
                    {
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