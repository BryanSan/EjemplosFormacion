using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using System;
using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    /// <summary>
    /// https://dotnetcodr.com/2016/10/05/generate-truly-random-cryptographic-keys-using-a-random-number-generator-in-net/
    /// </summary>
    public class SaltGenerator : ISaltGenerator
    {
        readonly Lazy<RNGCryptoServiceProvider> _randomGeneratorServiceProvider;

        public SaltGenerator()
        {
            _randomGeneratorServiceProvider = new Lazy<RNGCryptoServiceProvider>(() => new RNGCryptoServiceProvider());
        }

        public string GenerateSalt(int saltLength)
        {
            byte[] salt = GenerateSaltBytes(saltLength);
            return Convert.ToBase64String(salt);
        }

        public byte[] GenerateSaltBytes(int saltLength)
        {
            byte[] salt = new byte[saltLength];
            _randomGeneratorServiceProvider.Value.GetBytes(salt);

            return salt;
        }

        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_randomGeneratorServiceProvider.IsValueCreated)
                    {
                        _randomGeneratorServiceProvider.Value.Dispose();
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
