using System.Threading.Tasks;

namespace EjemplosFormacion.HelperClasess.FullDotNet.HelperClasses.Abstract
{
    public interface ICacheServiceTransaction
    {
        bool Commit();

        Task<bool> CommitAsync();
    }
}