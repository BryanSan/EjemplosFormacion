using System;
using System.Threading.Tasks;

namespace EjemplosFormacion.HelperClasess.Networking.Abstract
{
    public interface IClientApi : IDisposable
    {
        Task<T> GetAsync<T>(string address);
    }
}
