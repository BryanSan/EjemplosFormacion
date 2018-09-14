using System;
using System.Collections.Generic;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IHasherDerivatedKey
    {
        byte[] GetByteHash<T>(T objectToEncrypt, string salt, int roundOfHashIterations);
        byte[] GetByteHash(byte[] byteValue, byte[] salt, int roundOfHashIterations);

        string GetHash<T>(T objectToEncrypt, string salt, int roundOfHashIterations);
        string GetHash(byte[] byteValue, byte[] salt, int roundOfHashIterations);
    }
}
