using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace EjemplosFormacion.WebApi.ActionResults
{
    /// <summary>
    /// Custom Action Result para devolver MultiPartResult, esta clase te permite devolver tanto varios Files junto a un Form Data
    /// El navegador Firefox los descarga por separados, los demas como un unico archivo sin formato
    /// Sin embargo en una aplicacion cliente lo puedes leer con un ReadAsMultiPartResult y procesar los Files y Form Data
    /// Para habilitar el ReadAsMultiPartAsync() en el Cliente, agregar el Assembly necesario o el Nuget de Formatting.Extensions
    /// Aprovechar Custom la implementacion del MemoryStreamProvider "InMemoryMultipartFormDataStreamProvider" para separas los Files y Form Data y que sea mas comodo 
    /// </summary>
    class MultipartActionResult : IHttpActionResult
    {
        const string _subType = "byteranges";
        readonly MultipartContent _content;
        readonly MultipartFormDataItem _multiFormDataItem;
        readonly List<MultipartFileItem> _multiPartListFileItem;

        // Validamos que los datos pasados esten correctos
        public MultipartActionResult(MultipartFormDataItem multiPartFormDataItem, List<MultipartFileItem> multiPartListFileItem, string subtype = _subType, string boundary = null)
        {
            if (multiPartFormDataItem == null && (multiPartListFileItem == null || multiPartListFileItem.Count <= 0)) throw new ArgumentException("multiFormDataItem y multiPartListFileItem invalidos!.");
            else if (multiPartListFileItem != null && !multiPartListFileItem.TrueForAll(x => x.Stream != null && !string.IsNullOrWhiteSpace(x.FileName) && !string.IsNullOrWhiteSpace(x.ContentType)))
            {
                throw new ArgumentException("Alguna de las propiedades de los items de multiPartListFileItem estan invalidos!.");
            }
            else if (multiPartFormDataItem != null && (string.IsNullOrWhiteSpace(multiPartFormDataItem.ParameterName) || multiPartFormDataItem.ObjectData == null))
            {
                throw new ArgumentException("Alguna de las propiedades de los items de multiFormDataItem estan invalidos!.");
            }

            _multiFormDataItem = multiPartFormDataItem;
            _multiPartListFileItem = multiPartListFileItem;

            if (boundary == null)
            {
                _content = new MultipartContent(subtype);
            }
            else
            {
                _content = new MultipartContent(subtype, boundary);
            }
        }

        public MultipartActionResult(MultipartFormDataItem multiPartFormDataItem, string subtype = _subType, string boundary = null) : this(multiPartFormDataItem, null, subtype, boundary)
        {
            
        }

        public MultipartActionResult(List<MultipartFileItem> multiPartListFileItem, string subtype = _subType, string boundary = null) : this(null, multiPartListFileItem, subtype, boundary)
        {
            
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                // Gestion de Files
                GestionarMultiPartListFileItem();

                // Gestion de Form Data
                GestionarMultiPartFormDataItem();

                // Devolvemos el content buildeado con los metodos de gestion
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = _content;

                return response;
            });
        }

        // Metodo que gestiona el MultiPartFileItem convirtiendolo a un StreamContent y añadiendolo al HttpContent Global
        // Mapeo de MultiPartFileItem a StreamContent para agregarlo al MultiPartContent para posteriormente devolverlo
        void GestionarMultiPartListFileItem()
        {
            if (_multiPartListFileItem != null)
            {
                foreach (MultipartFileItem item in _multiPartListFileItem)
                {
                    var streamContent = new StreamContent(item.Stream);
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(item.ContentType);
                    streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = item.FileName
                        //FileNameStar = string.Empty
                    };

                    var contentDisposition = new ContentDispositionHeaderValue("attachment");

                    _content.Add(streamContent);
                }
            }
        }

        // Metodo que gestiona el MultiPartFormData convirtiendolo a un StringContent y añadiendolo al HttpContent Global
        // Mapeo de MultiPartFormData a StringContent para agregarlo al MultiPartContent para posteriormente devolverlo
        void GestionarMultiPartFormDataItem()
        {
            if (_multiFormDataItem != null)
            {
                // Serializamos el objeto
                string objectSerialized = JsonConvert.SerializeObject(_multiFormDataItem.ObjectData);

                // Lo agregamos como String Content
                var stringContent = new StringContent(objectSerialized, System.Text.Encoding.UTF8);

                // Marcamos el Header ContentDisposition como form-data (Para diferenciarlo de los Files)
                stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = _multiFormDataItem.ParameterName
                };

                // Añadimos el String Content al Content Global
                _content.Add(stringContent);
            }
        }

        // Modelo usado para agregar Archivos
        public class MultipartFileItem
        {
            public string ContentType { get; set; }

            public string FileName { get; set; }

            public Stream Stream { get; set; }
        }

        // Modelo usado para agregar Form Data
        public class MultipartFormDataItem
        {
            public string ParameterName { get; set; }

            public object ObjectData { get; set; }
        }
    }
}