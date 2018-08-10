using EjemplosFormacion.HelperClasess.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace EjemplosFormacion.WebApi.MediaTypeFormatters
{
    /// <summary>
    /// Custom Media Type Formatter para serializar un objeto a formato text/csv
    /// Hereda de la clase BufferedMediaTypeFormatter para tener operaciones sincronas 
    /// Puedes hacer override a los metodos sincronicos segun necesites
    /// Si necesitas usar la contraparte Asincronica hereda directamente de la clase MediaTypeFormatter
    /// El Media Type Formatter lo que hace es que si el Cliente ha solicitado (Accept Header) o a enviado (Content Type Header) un MIME Type
    /// Que este configurado en esta clase, se evaluara si el type del objeto es soportado por esta clase y se serializara o deseralizara segun sea el caso
    /// En este caso se ha configurado el MIME Type, entonces Web API seleccionara este Media Type Formatter
    /// Cuando llegue cualquier Request con Accept Header o Content Type Header que tenga el MIME Type text/csv
    /// </summary>
    public class TestCSVBufferedMediaTypeFormatter : BufferedMediaTypeFormatter
    {

        public TestCSVBufferedMediaTypeFormatter()
        {
            // Agrega los Media Type con el que este Formatter trabajara
            // Web Api comparara los MIME Type que vendran del Request en el Accept Header y/o Content Type Header
            // Con los configurados en el MediaTypeFormatter para saber si es candidato para ser seleccionado para hacer la tarea o no
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
        }

        // Metodo para evaluar si el type pasado es deserializable por este Media Type Formatter
        // Evalua el type y devuelve true o false si este Media Type Formatter es capaz de deserealizarlo
        // Si devuelves un true recuerda hacerle override al metodo ReadFromStream
        public override bool CanReadType(Type type)
        {
            return false;
        }

        // Metodo para deserealizar un objeto desde un Stream, este metodo sera llamado si el metodo CanReadType devolvio true
        public override object ReadFromStream(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return base.ReadFromStream(type, readStream, content, formatterLogger);
        }

        // Metodo para evaluar si el type pasado es serializable por este Media Type Formatter
        // Evalua el type y devuelve true o false si este Media Type Formatter es capaz de serializarlo
        // Si devuelves un true recuerda hacerle override al metodo WriteToStream
        public override bool CanWriteType(Type type)
        {
            if (type == typeof(Product))
            {
                return true;
            }
            else
            {
                Type enumerableType = typeof(IEnumerable<Product>);
                return enumerableType.IsAssignableFrom(type);
            }
        }

        // Metodo para serializar un objeto hacia un Stream, este metodo sera llamado si el metodo CanWriteType devolvio true
        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            // Configuramos un StreamWriter para escribir al Stream destino 
            using (var writer = new StreamWriter(writeStream))
            {
                // Escribimos los items segun el formato
                // Como es CSV escribimos el objeto segun nuestra logica y los requerimientos del formato
                // En este caso los valores de las propiedades separados por ,
                var products = value as IEnumerable<Product>;
                if (products != null)
                {
                    foreach (var product in products)
                    {
                        WriteItem(product, writer);
                    }
                }
                else
                {
                    var singleProduct = value as Product;
                    if (singleProduct == null)
                    {
                        throw new InvalidOperationException("Cannot serialize type");
                    }
                    WriteItem(singleProduct, writer);
                }
            }
        }

        // Helper methods for serializing Products to CSV format. 
        private void WriteItem(Product product, StreamWriter writer)
        {
            writer.WriteLine("{0},{1},{2},{3}", product.Id, product.Name.EscapeInvalidCharacters(), product.Category.EscapeInvalidCharacters(), product.Price);
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