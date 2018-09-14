﻿using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using System;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers
{
    /// <summary>
    /// https://dotnetcodr.com/2016/10/05/generate-truly-random-cryptographic-keys-using-a-random-number-generator-in-net/
    /// </summary>
    public class SaltGenerator : ISaltGenerator
    {
        readonly IRandomBytesGenerator _randomBytesGenerator;

        public SaltGenerator(IRandomBytesGenerator randomBytesGenerator)
        {
            _randomBytesGenerator = randomBytesGenerator;
        }

        public string GenerateSalt(int saltLength)
        {
            byte[] salt = GenerateSaltBytes(saltLength);
            return Convert.ToBase64String(salt);
        }

        public byte[] GenerateSaltBytes(int saltLength)
        {
            var salt = new byte[saltLength];
            _randomBytesGenerator.GenerateRandomBytes(salt);

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
