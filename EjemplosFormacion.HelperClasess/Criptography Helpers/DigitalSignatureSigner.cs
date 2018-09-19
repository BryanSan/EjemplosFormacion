using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using EjemplosFormacion.HelperClasess.CriptographyHelpers.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    /// <summary>
    /// https://dotnetcodr.com/2013/11/14/introduction-to-digital-signatures-in-net-cryptography/
    /// </summary>
    public class DigitalSignatureSigner<THashAlgorithm> : IDigitalSignatureSigner<THashAlgorithm>
        where THashAlgorithm : HashAlgorithm, new()
    {
        readonly Lazy<RSA> _senderCipher;
        readonly Lazy<RSA> _receiverCipher;
        readonly IHasher<THashAlgorithm> _hasher;

        public DigitalSignatureSigner(string myPublicPrivateRSAKey, string _receiversPublicRSAKey, IHasher<THashAlgorithm> hasher)
        {
            // Debe ser RSA para que funcione tanto en .Net como en .Net Core
            // Ya que hay implementaciones distintas para cada plataforma
            // https://stackoverflow.com/questions/41986995/implement-rsa-in-net-core
            _receiverCipher = new Lazy<RSA>(() =>
            {
                var receiverCipher = RSA.Create();
                receiverCipher.FromXmlString(_receiversPublicRSAKey);

                return receiverCipher;
            }); 

            _senderCipher = new Lazy<RSA>(() =>
            {
                var senderCipher = RSA.Create();
                senderCipher.FromXmlString(myPublicPrivateRSAKey);

                return senderCipher;
            });

            _hasher = hasher;
        }

        public DigitalSignatureResult BuildSignedMessage(byte[] bytesToSign)
        {
            if (bytesToSign == null || bytesToSign.Count() <= 0) throw new ArgumentException($"{nameof(bytesToSign)} a sign no puede estar vacio!.");

            byte[] cipherBytes = _receiverCipher.Value.Encrypt(bytesToSign, RSAEncryptionPadding.OaepSHA512);
            byte[] cipherBytesHash = _hasher.GetByteHash(cipherBytes);
            byte[] cipherBytesHashSignature = CalculateSignatureBytes(cipherBytesHash);

            string cipher = Convert.ToBase64String(cipherBytes);
            string signature = Convert.ToBase64String(cipherBytesHashSignature);

            return new DigitalSignatureResult() { CipherText = cipher, SignatureText = signature };
        }

        public DigitalSignatureResult BuildSignedMessage<T>(T objectToSign)
        {
            if (objectToSign == null) throw new ArgumentException($"{nameof(objectToSign)} a sign no puede estar vacio!.");

            // Serializamos el objeto a json 
            string serializedEntity = JsonConvert.SerializeObject(objectToSign);
            byte[] messageBytes = Encoding.UTF8.GetBytes(serializedEntity);

            DigitalSignatureResult digitalSignatureResult = BuildSignedMessage(messageBytes);

            return digitalSignatureResult;
        }

        byte[] CalculateSignatureBytes(byte[] hashToSign)
        {
            RSAPKCS1SignatureFormatter signatureFormatter = new RSAPKCS1SignatureFormatter(_senderCipher.Value);
            signatureFormatter.SetHashAlgorithm(_hasher.HashAlgorithmName);
            
            byte[] signature = signatureFormatter.CreateSignature(hashToSign);

            return signature;
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