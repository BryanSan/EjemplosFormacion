using EjemplosFormacion.HelperClasess.Networking.Abstract;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EjemplosFormacion.HelperClasess.Networking
{
    public class HttpClientApi : IClientApi
    {

        bool disposed = false;
        readonly HttpClient client;

        public HttpClientApi(string baseAddress)
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<T> GetAsync<T>(string address)
        {
            HttpResponseMessage response = await client.GetAsync(address);
            T responseContent = await response.Content.ReadAsAsync<T>();

            return responseContent;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                client.Dispose();
            }

            disposed = true;
        }
    }
}
