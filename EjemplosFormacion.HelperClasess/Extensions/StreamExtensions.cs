using System;
using System.Diagnostics;
using System.IO;

namespace EjemplosFormacion.HelperClasess.Extensions
{
    public static class StreamExtensions
    {
        public static void WritePartialContentToOutputStream(this Stream inputStream, Stream outputStream, long start, long end)
        {
            // This will be used in copying input stream to output stream.
            int readStreamBufferSize = 1024 * 1024;
            int count = 0;
            long remainingBytes = end - start + 1;
            long position = start;
            byte[] buffer = new byte[readStreamBufferSize];

            inputStream.Position = start;
            do
            {
                try
                {
                    if (remainingBytes > readStreamBufferSize)
                        count = inputStream.Read(buffer, 0, readStreamBufferSize);
                    else
                        count = inputStream.Read(buffer, 0, (int)remainingBytes);
                    outputStream.Write(buffer, 0, count);
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error);
                    break;
                }
                position = inputStream.Position;
                remainingBytes = end - position + 1;
            } while (position <= end);
        }
    }
}
