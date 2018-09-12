using EjemplosFormacion.HelperClasess.CriptographyHelpers.Abstract;
using System;
using System.Security.Cryptography;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Factories
{
    public class SymmetricAlgorithmFactory<TSymmetricAlgorithm, THasherAlgorithm> : ISymmetricAlgorithmFactory<TSymmetricAlgorithm>
        where TSymmetricAlgorithm : SymmetricAlgorithm, new()
        where THasherAlgorithm : HashAlgorithm, new()
    {
        readonly byte[] _keyArray;
        readonly byte[] _IVKeyBytes;

        public SymmetricAlgorithmFactory(string key, string IVKey, IHasher<THasherAlgorithm> hasher)
        {
            // Construimos el Key y IV Key a usar por el SymmetriAlgorithm
            (byte[] keyArray, byte[] IVKeyBytes) = CreateKeyAndIVKey(key, IVKey, hasher);

            _keyArray = keyArray;
            _IVKeyBytes = IVKeyBytes;
        }

        public TSymmetricAlgorithm CreateSymmetricAlgorithm()
        {
            var symmetricAlgorithm = new TSymmetricAlgorithm();

            // padding mode(if any extra byte added)
            // Los algoritmos de encriptacion, separan los bytes que quieres encriptar en bloques de tamaño fijo, 
            // Dgamos que si tenemos un bloque de 10 los separa en bloque de 2, (Cada algoritmo tiene un valor fijo por el cual va a separar, 128,256 ,etc)
            // Es complicado que el total de bytes sea justo, por lo tanto si al bloque final le quedan huecos el padding se encarga de llenarlo
            // El padding mode es donde puedes configurar como se va a llenar esos huecos, son 0, con 8, con random data, etc.
            symmetricAlgorithm.Padding = PaddingMode.ISO10126;

            // Dice en que modo va a encriptar, cada bloque por separado y aislado, o como en este caso el CBC encripta el bloque actual junto con el resultado del anterior
            // En caso de que usemos el CBC necesitamos un IV - Initialization Vector para decir que usaremos para nuestro primer bloque (ya que el primer bloque no tiene bloque previo para encriptar con el)
            symmetricAlgorithm.Mode = CipherMode.CBC;

            // Asignas el key que se usara para la encriptacion
            symmetricAlgorithm.Key = _keyArray;

            // In case you go with CBC then another term you’ll need to be familiar with is the IV – Initialization Vector. 
            // The IV determines what kind of random data you’re going to use for the first block simply because there’s no block before the first block to be used as input.
            // The IV is just some random data that will be used as input in the encryption of the first block.
            // The IV doesn’t need to be some secret string – it must be redistributed along with the cipher text to the receiver of our message.
            // The only rule is not to reuse the IV to keep the randomness that comes with it.
            symmetricAlgorithm.IV = _IVKeyBytes;

            return symmetricAlgorithm;
        }

        (byte[] keyBytes, byte[] IVKeyBytes) CreateKeyAndIVKey(string key, string IVKey, IHasher<THasherAlgorithm> hasher)
        {
            // Necesitamos una instancia del SymmetricAlgorithm para saber que tamaño soporta en la Key y IVKey
            using (var symmetricAlgorithm = new TSymmetricAlgorithm())
            using (hasher)
            {
                // Obtenemos los bytes del Key y IVKey
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] IVKeyBytes = Encoding.UTF8.GetBytes(IVKey);

                // Los algorimos de encriptacion usan un tamaño de key especifico, por eso haces un resize para hacer que el tamaño este dentro del soportado
                Array.Resize(ref keyBytes, symmetricAlgorithm.Key.Length);
                Array.Resize(ref IVKeyBytes, symmetricAlgorithm.IV.Length);

                // Hasheamos los bytes de las Keys con el Hasher Algorithm escogido, apoyandonos en la clase wrapper Hasher
                keyBytes = hasher.GetByteHash(keyBytes);
                IVKeyBytes = hasher.GetByteHash(IVKeyBytes);

                // Limpiar resources
                symmetricAlgorithm.Clear();

                return (keyBytes, IVKeyBytes);
            }
        }
    }
}
