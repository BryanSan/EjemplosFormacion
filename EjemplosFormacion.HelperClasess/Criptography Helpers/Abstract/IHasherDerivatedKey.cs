namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract
{
    public interface IHasherDerivatedKey
    {
        string GetHash<T>(T objectToHash, string salt, int roundOfHashIterations);
        string GetHash(byte[] bytesToHash, byte[] salt, int roundOfHashIterations);

        byte[] GetByteHash<T>(T objectToHash, string salt, int roundOfHashIterations);
        byte[] GetByteHash(byte[] bytesToHash, byte[] salt, int roundOfHashIterations);
    }
}
