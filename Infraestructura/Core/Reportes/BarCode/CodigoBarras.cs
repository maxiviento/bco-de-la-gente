using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using br.com.arcnet.barcodegenerator;

namespace Infraestructura.Core.Reportes.BarCode
{
    public class CodigoBarras
    {
        public CodigoBarras()
        {
            Tipo = TypeBarcode.Interleaved2Of5;
            Width = 2500;
            Height = 250;
        }

        public CodigoBarras(string data)
        {
            Data = data;
            Tipo = TypeBarcode.Interleaved2Of5;
            Width = 2500;
            Height = 250;
            GenerarCodigoDeBarras();
        }

        public CodigoBarras(string data, TypeBarcode tipo, int width, int height)
        {
            Data = data;
            Tipo = tipo;
            Width = width;
            Height = height;
            GenerarCodigoDeBarras();
        }

        public string Data { get; private set; }
        public Image Imagen { get; private set; }
        private ImageFormat FormatoImagen { get; set; }
        private int Width { get; }
        private int Height { get; }
        private TypeBarcode Tipo { get; }

        public long Length => Data.Length;

        public void GenerarCodigoDeBarras(string data = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(data)) Data = data;
                if (!string.IsNullOrEmpty(Data))
                {
                    var barcode = new Barcode(Data, Tipo);
                    Imagen = barcode.Encode(Tipo, Data, Width, Height);
                    FormatoImagen = ImageFormat.Png;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetBarCodeInBase64()
        {
            byte[] imageArray;

            if (Imagen == null) return "";

            using (MemoryStream imageStream = new MemoryStream())
            {
                Imagen.Save(imageStream, FormatoImagen);
                imageArray = new byte[imageStream.Length];
                imageStream.Seek(0, SeekOrigin.Begin);
                imageStream.Read(imageArray, 0, (int)imageStream.Length);
            }

            return Convert.ToBase64String(imageArray);
        }
    }
}
