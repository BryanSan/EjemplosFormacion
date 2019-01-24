using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    /// <summary>
    /// https://dotnetcodr.com/2016/10/05/generate-truly-random-cryptographic-keys-using-a-random-number-generator-in-net/
    /// </summary>
    public class CryptographicRandomBytesGenerator : ICryptographicRandomBytesGenerator
    {
        readonly Lazy<RNGCryptoServiceProvider> _randomGeneratorServiceProvider;

        public CryptographicRandomBytesGenerator()
        {
            _randomGeneratorServiceProvider = new Lazy<RNGCryptoServiceProvider>(() => new RNGCryptoServiceProvider());
        }

        public void GenerateRandomBytes(byte[] bufferToFill)
        {
            if (bufferToFill == null || bufferToFill.Count() <= 0) throw new ArgumentException($"{nameof(bufferToFill)} no puede estar vacio");

            _randomGeneratorServiceProvider.Value.GetBytes(bufferToFill);
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