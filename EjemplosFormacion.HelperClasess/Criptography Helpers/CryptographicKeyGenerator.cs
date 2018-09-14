using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using System;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    public class CryptographicKeyGenerator : ICryptographicKeyGenerator
    {
        readonly IRandomBytesGenerator _randomBytesGenerator;

        public CryptographicKeyGenerator(IRandomBytesGenerator randomBytesGenerator)
        {
            _randomBytesGenerator = randomBytesGenerator;
        }

        public string GenerateCryptographicKey(int keyLength)
        {
            byte[] cryptographicKey = GenerateCryptographicKeyBytes(keyLength);
            string cryptographicKeyString = Convert.ToBase64String(cryptographicKey);

            return cryptographicKeyString;
        }

        public byte[] GenerateCryptographicKeyBytes(int keyLength)
        {
            var randomBytes = new byte[keyLength];
            _randomBytesGenerator.GenerateRandomBytes(randomBytes);

            return randomBytes;
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
