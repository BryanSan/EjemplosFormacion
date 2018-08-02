using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Xml.Linq;

namespace EjemplosFormacion.HelperClasess.FullDotNet.Extensions
{
    /// <summary>
    /// Helper Methods para trabajar con dynamic objects, 
    /// Como no se puede crear un extension metodo con el "this", lo dejamos como una metodo estatico normal
    /// </summary>
    public static class DynamicExtensions
    {
        /// <summary>
        /// Metodo para convertir un ExpandoObject y sus propiedades a un XmlNode
        /// https://blogs.msdn.microsoft.com/csharpfaq/2009/09/30/dynamic-in-c-4-0-introducing-the-expandoobject/
        /// </summary>
        private static XElement ExpandoObjectToXmlNode(dynamic node, String nodeName)
        {
            XElement xmlNode = new XElement(nodeName);

            foreach (var property in (IDictionary<String, Object>)node)
            {
                if (property.Value.GetType() == typeof(ExpandoObject))
                {
                    xmlNode.Add(ExpandoObjectToXmlNode(property.Value, property.Key));
                }
                else
                {
                    if (property.Value.GetType() == typeof(List<dynamic>))
                    {
                        foreach (var element in (List<dynamic>)property.Value)
                        {
                            xmlNode.Add(ExpandoObjectToXmlNode(element, property.Key));
                        }
                    }
                    else
                    {
                        xmlNode.Add(new XElement(property.Key, property.Value));
                    }
                }
            }

            return xmlNode;
        }
    }
}
