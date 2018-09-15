using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using System;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    /// <summary>
    /// https://dotnetcodr.com/2016/10/05/generate-truly-random-cryptographic-keys-using-a-random-number-generator-in-net/
    /// </summary>
    public class CryptographicKeyGenerator : ICryptographicKeyGenerator
    {
        readonly IRandomBytesGenerator _randomBytesGenerator;

        public CryptographicKeyGenerator(IRandomBytesGenerator randomBytesGenerator)
        {
            _randomBytesGenerator = randomBytesGenerator;
        }

        public byte[] GenerateCryptographicKeyBytes(int keyLength)
        {
            if (keyLength <= 0) throw new ArgumentException($"{nameof(keyLength)} no puede ser negativo");

            var randomBytes = new byte[keyLength];
            _randomBytesGenerator.GenerateRandomBytes(randomBytes);

            return randomBytes;
        }

        public string GenerateCryptographicKey(int keyLength)
        {
            byte[] cryptographicKey = GenerateCryptographicKeyBytes(keyLength);
            string cryptographicKeyString = Convert.ToBase64String(cryptographicKey);

            return cryptographicKeyString;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _randomBytesGenerator.Dispose();
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
