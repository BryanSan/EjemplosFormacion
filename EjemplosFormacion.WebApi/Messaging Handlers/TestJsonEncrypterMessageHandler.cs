using EjemplosFormacion.HelperClasess.Abstract;
using EjemplosFormacion.HelperClasess.Extensions;
using EjemplosFormacion.WebApi.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EjemplosFormacion.WebApi.MessagingHandlers
{
    /// <summary>
    /// Message Handler que lee el Content del Request y Response para Desencriptarlo y Encriptarlo segun sea el caso
    /// Soporta Request en formato Json por ahora y serializa el Response en formato Json por ahora
    /// Usa una entidad modelo para hostear el mensaje ya que da problemas para el mensaje en texto puro
    /// Usa una dependencia para encriptar y desencriptar Entidades de cualquier tipo con los algoritmos Simetricos especificados en los atributos de este message handler
    /// </summary>
    class TestJsonEncrypterMessageHandler : DelegatingHandler
    {
        readonly ISymmetricEncrypter<AesManaged, SHA256Managed> _symmetricEncrypter;

        // Passing the next Handler of the Pipeline If Any
        public TestJsonEncrypterMessageHandler(ISymmetricEncrypter<AesManaged, SHA256Managed> symmetricEncrypter,
                                               HttpMessageHandler messageHandler) : base(messageHandler)
        {
            _symmetricEncrypter = symmetricEncrypter;
        }

        public TestJsonEncrypterMessageHandler(ISymmetricEncrypter<AesManaged, SHA256Managed> symmetricEncrypter)
        {
            _symmetricEncrypter = symmetricEncrypter;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await ProcessRequest(request);

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            await ProcessResponse(response);

            return response;
        }

        async Task ProcessRequest(HttpRequestMessage request)
        {
            // Si no hay Content que revisar salimos
            if (request.Content == null) return;

            string contentRequest = await request.Content.ReadAsStringAsync();

            // Si el Content esta vacio salimos
            if (string.IsNullOrWhiteSpace(contentRequest)) return;

            // Si no es un Json Valido salimos
            if (!contentRequest.IsValidJson()) return;

            // Serializamos la peticion a un Entity que hace de host del mensaje encriptado
            // Esto debido que no se puede mandar el mensaje encriptado como texto puro ya que molesta y da error de formato el http
            MessageEncrypted hostMessageEntity = JsonConvert.DeserializeObject<MessageEncrypted>(contentRequest);

            // Desencriptamos el mensaje encriptado por la entidad
            string messageDecrypted = _symmetricEncrypter.Decrypt<string>(hostMessageEntity.Message);

            // Lo asignamos al Content del Request para que pueda verse los datos desencriptados en los Action
            request.Content = new StringContent(messageDecrypted, Encoding.UTF8, "application/json");
        }

        async Task ProcessResponse(HttpResponseMessage response)
        {
            // Si no hay Content que revisar salimos
            if (response.Content == null) return;

            string contentResponse = await response.Content.ReadAsStringAsync();

            // Si el Content esta vacio salimos
            if (string.IsNullOrWhiteSpace(contentResponse)) return;

            // Si no es un Json Valido salimos
            if (!contentResponse.IsValidJson()) return;

            // Encriptamos el Content
            string encryptedMessage = _symmetricEncrypter.Encrypt(contentResponse);

            // Lo asignamos a la entidad que hostea el mensaje encriptado
            MessageEncrypted hostMessageEntity = new MessageEncrypted
            {
                Message = encryptedMessage
            };

            // Serializamos la entidad
            string responseEncrypted = JsonConvert.SerializeObject(hostMessageEntity);

            // Lo asignamos al Content del Response para que pueda seguir su camino al cliente
            response.Content = new StringContent(responseEncrypted, Encoding.UTF8, "application/json");
        }
    }
}