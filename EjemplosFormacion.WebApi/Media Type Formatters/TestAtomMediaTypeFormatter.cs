using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace EjemplosFormacion.WebApi.MediaTypeFormatters
{
    /// <summary>
    /// Custom Media Type Formatter para serializar un objeto a formato application/atom+xml usando un encoding especial
    /// Hereda de la clase MediaTypeFormatter para tener operaciones asincronicas 
    /// Puedes hacer override a los metodos asincronicos segun necesites
    /// Si necesitas usar la contraparte sincronica hereda directamente de la clase BufferedMediaTypeFormatter
    /// El Media Type Formatter lo que hace es que si el Cliente ha solicitado (Accept Header) o a enviado (Content Type Header) un MIME Type
    /// Que este configurado en esta clase, se evaluara si el type del objeto es soportado por esta clase y se serializara o deseralizara segun sea el caso
    /// En este caso se ha configurado el MIME Type, entonces Web API seleccionara este Media Type Formatter
    /// Cuando llegue cualquier Request con Accept Header o Content Type Header que tenga el MIME Type application/atom+xml
    /// </summary>
    public class TestAtomMediaTypeFormatter : MediaTypeFormatter
    {
        private HttpRequestMessage _request;

        public TestAtomMediaTypeFormatter()
        {
            // Agrega los Media Type con el que este Formatter trabajara
            // Web Api comparara los MIME Type que vendran del Request en el Accept Header y/o Content Type Header
            // Con los configurados en el MediaTypeFormatter para saber si es candidato para ser seleccionado para hacer la tarea o no
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/atom+xml"));
        }

        public TestAtomMediaTypeFormatter(HttpRequestMessage request)
        {
            _request = request;
        }

        // Metodo para evaluar si el type pasado es deserializable por este Media Type Formatter
        // Evalua el type y devuelve true o false si este Media Type Formatter es capaz de deserealizarlo
        // Si devuelves un true recuerda hacerle override al metodo ReadFromStream
        public override bool CanReadType(Type type)
        {
            return false;
        }

        // Metodo para deserealizar un objeto desde un Stream, este metodo sera llamado si el metodo CanReadType devolvio true
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return base.ReadFromStreamAsync(type, readStream, content, formatterLogger);
        }

        // Metodo para evaluar si el type pasado es serializable por este Media Type Formatter
        // Evalua el type y devuelve true o false si este Media Type Formatter es capaz de serializarlo
        // Si devuelves un true recuerda hacerle override al metodo WriteToStream
        public override bool CanWriteType(Type type)
        { 
            return true;
        }

        // Metodo para serializar un objeto hacia un Stream, este metodo sera llamado si el metodo CanWriteType devolvio true
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            // creating a System.ServiceModel.Syndication.SyndicationFeed
            var feed = CreateFeed(type, value);

            return Task.Run(() =>
            {
                using (var writer = XmlWriter.Create(writeStream))
                {
                    Atom10FeedFormatter atomformatter = new Atom10FeedFormatter(feed);
                    atomformatter.WriteTo(writer);
                }
            });
        }

        // this method is overridden in order to pass the HttpRequestMessage into the formatter
        // when overridden the GetPerRequestFormatterInstance is called in order to create 
        // new instances of the formatter (if the method is not overridden the same instance is
        // being reused for every call) allowing to pass the request to the Formatter constructor.
        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type,
                                                                          HttpRequestMessage request,
                                                                          MediaTypeHeaderValue mediaType)
        {
            return new TestAtomMediaTypeFormatter(request);
        }

        private SyndicationFeed CreateFeed(Type type, object value)
        {
            // this object represent a syndication feed. Based on the formatter, the actual syndication format will be created
            var feed = new SyndicationFeed
            {
                Title = new TextSyndicationContent("Atom Content")
            };

            var items = from product in (IEnumerable<Product>)value
                        select new SyndicationItem
                        {
                            Title = new TextSyndicationContent(String.Format("Product {0} {1}", product.Id, product.Name)),
                            Id = product.Id.ToString(),
                            BaseUri = new Uri(_request.RequestUri, string.Format("{0}/{1}", _request.RequestUri.AbsolutePath, product.Id)),

                        };

            feed.Items = items;
            return feed;
        }

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public decimal Price { get; set; }
        }
    }
}