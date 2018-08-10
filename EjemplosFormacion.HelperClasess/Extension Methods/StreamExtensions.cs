using System;
using System.Diagnostics;
using System.IO;

namespace EjemplosFormacion.HelperClasess.ExtensionMethods
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Extension method usado para escribir en un stream el rango de bytes solicitado de mi Source Stream
        /// </summary>
        public static void WritePartialContentToOutputStream(this Stream sourceStream, Stream outputStream, long start, long end)
        {
            // This will be used in copying input stream to output stream.
            int readStreamBufferSize = 1024 * 1024;
            int count = 0;
            long remainingBytes = end - start + 1;
            long position = start;
            byte[] buffer = new byte[readStreamBufferSize];

            sourceStream.Position = start;
            do
            {
                try
                {
                    if (remainingBytes > readStreamBufferSize)
                        count = sourceStream.Read(buffer, 0, readStreamBufferSize);
                    else
                        count = sourceStream.Read(buffer, 0, (int)remainingBytes);
                    outputStream.Write(buffer, 0, count);
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error);
                    break;
                }
                position = sourceStream.Position;
                remainingBytes = end - position + 1;
            } while (position <= end);
        }
    }
}
