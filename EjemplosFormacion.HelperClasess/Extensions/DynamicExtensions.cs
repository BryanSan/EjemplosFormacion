using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Xml.Linq;

namespace EjemplosFormacion.HelperClasess.Extensions
{
    public static class DynamicExtensions
    {
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
