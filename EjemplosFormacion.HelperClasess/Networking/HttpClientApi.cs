using EjemplosFormacion.HelperClasess.Networking.Abstract;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EjemplosFormacion.HelperClasess.Networking
{
    public class HttpClientApi : IClientApi
    {

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

        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
