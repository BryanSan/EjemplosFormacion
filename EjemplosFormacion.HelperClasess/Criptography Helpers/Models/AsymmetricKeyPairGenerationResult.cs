using System;
using System.Collections.Generic;
using System.Text;

namespace EjemplosFormacion.HelperClasess.CriptographyHelpers.Models
{
    public class AsymmetricKeyPairGenerationResult
    {
        public string PublicKeyXml { get; }
        public string PublicPrivateKeyPairXml { get; }

        public AsymmetricKeyPairGenerationResult(string publicKeyXml, string publicPrivateKeyPairXml)
        {
            PublicKeyXml = publicKeyXml;
            PublicPrivateKeyPairXml = publicPrivateKeyPairXml;
        }
    }
}