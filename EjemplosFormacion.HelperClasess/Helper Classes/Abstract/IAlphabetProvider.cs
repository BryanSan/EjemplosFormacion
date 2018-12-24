using System.Collections.Generic;

namespace EjemplosFormacion.HelperClasess.HelperClasses.Abstract
{
    public interface IAlphabetProvider
    {
        IList<string> GetAlphabet(bool lowerCase);
    }
}