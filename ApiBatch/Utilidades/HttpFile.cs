using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiBatch.Utilidades
{
    public class HttpFile
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public string MediaType { get; set; }
        public byte[] BufferArray { get; set; }
        public string Buffer { get; set; }

        public HttpFile() { }

        public HttpFile(string fileName, string mediaType, byte[] buffer)
        {
            FileName = fileName;
            MediaType = mediaType;
            BufferArray = buffer;
        }
    }
}