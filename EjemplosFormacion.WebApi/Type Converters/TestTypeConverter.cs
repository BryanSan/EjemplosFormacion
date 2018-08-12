using System;
using System.ComponentModel;
using System.Globalization;

namespace EjemplosFormacion.WebApi.TypeConverters
{
    /// <summary>
    /// You can make Web API treat a class as a simple type(so that Web API will try to bind it from the URI) by creating a TypeConverter and providing a string conversion.
    /// http://localhost:6719/api/TestTypeConverter/TestTypeConverter?location=47.678558,-122.130989
    /// Custom Type Converter para hallar el valor de un parametro, en este caso location
    /// En este caso obtendra el valor de un parametro del Query String, separandolo por "," y hallando el valor para todas sus propiedades
    /// </summary>
    public class TestTypeConverter : TypeConverter
    {
        // Override the CanConvertFrom method that specifies which type the converter can convert from. This method is overloaded.
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        // Metodo que sera llamado por Web Api pasandole el valor del parametro del Query String, haras tu logica para convertir este valor en el objeto para el cual este Type Converter existe
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                GeoPoint point;
                if (GeoPoint.TryParse((string)value, out point))
                {
                    return point;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        [TypeConverter(typeof(TestTypeConverter))]
        public class GeoPoint
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public static bool TryParse(string value, out GeoPoint result)
            {
                result = null;

                var parts = value.Split(',');
                if (parts.Length != 2)
                {
                    return false;
                }

                double latitude, longitude;
                if (double.TryParse(parts[0], out latitude) &&
                    double.TryParse(parts[1], out longitude))
                {
                    result = new GeoPoint() { Longitude = longitude, Latitude = latitude };
                    return true;
                }
                return false;
            }
        }
    }
}