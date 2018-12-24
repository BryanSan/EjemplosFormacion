using EjemplosFormacion.HelperClasess.HelperClasses.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace EjemplosFormacion.HelperClasess.HelperClasses
{
    public class AlphabetProvider : IAlphabetProvider
    {
        public IList<string> GetAlphabet(bool lowerCase)
        {
            char A = lowerCase ? 'a' : 'A';
            char Z = lowerCase ? 'z' : 'Z';

            return Enumerable.Range(A, Z - A + 1).Select(x => ((char)x).ToString()).ToList();
        }
    }
}
