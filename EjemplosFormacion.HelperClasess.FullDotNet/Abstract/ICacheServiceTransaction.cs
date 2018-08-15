using System.Threading.Tasks;

namespace EjemplosFormacion.HelperClasess.FullDotNet.Abstract
{
    public interface ICacheServiceTransaction
    {
        bool Commit();

        Task<bool> CommitAsync();
    }
}