using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using CryptoManagerV4._0.Clases;
using System.Security.Cryptography;
using System.Net;

namespace Core.CiDi.Documentos.Utils
{
    public class Helper
    {
        #region Atributos

        private const String Key_Cif_Decif = "CDD_2016";

        #endregion

        #region Metodos Array

        internal String Convertir_Array_Byte_En_Cadena(byte[] pArray)
        {
            String retCadena = String.Empty;

            try
            {
                retCadena = Encoding.ASCII.GetString(pArray, 0, pArray.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retCadena;
        }

        internal byte[] Convertir_Cadena_En_Array_Byte(String pCadenaArray)
        {
            byte[] retArray = null;

            try
            {
                retArray = Encoding.ASCII.GetBytes(pCadenaArray);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retArray;
        }

        public string Encriptar_Password(string appPassword)
        {
            var cry = new CryptoSymmetric(CryptoSymmetric.CryptoTypes.encTypeDES);

            string retPwd = string.Empty;

            try
            {
                retPwd = cry.Encrypt(appPassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retPwd;
        }

        internal string Obtener_Password_Desencriptado(string appPassword)
        {
            var cry = new CryptoSymmetric(CryptoSymmetric.CryptoTypes.encTypeDES);
            string retPwd = string.Empty;

            try
            {
                retPwd = cry.Decrypt(appPassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retPwd;
        }

        public byte[] Merge_Array_Elements(byte[] pByteArrayImage, byte[] pPublicBlobKey, byte[] pByteArrayKey)
        {
            List<byte[]> listByteArray = new List<byte[]>();
            byte[] separator = new byte[1] { 0x7c };    // Caracter separador: 0x7c = '|'
            byte[] arrayMerged = new byte[pByteArrayImage.Length + separator.Length + pPublicBlobKey.Length + separator.Length + pByteArrayKey.Length];
            int offset = 0;

            listByteArray.Add(pByteArrayImage);
            listByteArray.Add(separator);
            listByteArray.Add(pPublicBlobKey);
            listByteArray.Add(separator);
            listByteArray.Add(pByteArrayKey);

            foreach (byte[] itemArray in listByteArray)
            {
                System.Buffer.BlockCopy(itemArray, 0, arrayMerged, offset, itemArray.Length);
                //offset += itemArray.Length;
            }

            return arrayMerged;
        }

        public List<byte[]> Unmerge_Array_Elements(byte[] pArrData)
        {
            List<byte[]> listaArray = new List<byte[]>();
            byte[] caracterSeparador = new byte[1] { 0x7c };    // Caracter separador: 0x7c = '|'
            Int64 posicionSeparador = -1;

            // Obtengo la posicion del caracter separador
            for (Int64 iElem = 0; iElem < pArrData.Count(); iElem++)
            {
                if (pArrData[iElem] == caracterSeparador.First())
                    posicionSeparador = iElem;
            }

            byte[] arrayKey = new byte[posicionSeparador - 1];

            // Obtengo el array de la Key
            arrayKey = new byte[pArrData.Length - posicionSeparador];
            for (Int64 iElem = posicionSeparador + 1; iElem < arrayKey.Count(); iElem++)
            {
                arrayKey[iElem] = pArrData[iElem];
            }

            // Obtengo el array de la Imagen
            byte[] arrayImagen = new byte[pArrData.Length - posicionSeparador];
            for (Int64 iElem = posicionSeparador + 1; iElem < arrayImagen.Count(); iElem++)
            {
                arrayImagen[iElem] = pArrData[iElem];
            }

            // Conformo la lista de Array
            listaArray.Add(arrayKey);
            listaArray.Add(arrayImagen);

            return listaArray;
        }


        public bool Is_Valid_Key_Values(byte[] pSourceByteArrayKey, byte[] pTargetByteArrayKey)
        {
            bool retOk = true;

            for (int i = 0; i < pTargetByteArrayKey.Length; i++)
                if (pSourceByteArrayKey[i] != pTargetByteArrayKey[i])
                    retOk = false;

            return retOk;
        }

        public String Cifrar_Imagen_Documento(byte[] pImagen)
        {
            var objCryptoHash = new CryptoHash();
            String mensaje = String.Empty;
            String retStrImagenDocumento = String.Empty;

            try
            {
                retStrImagenDocumento = Convert.ToBase64String(pImagen);
                retStrImagenDocumento = objCryptoHash.Cifrar_Datos(retStrImagenDocumento, out mensaje);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retStrImagenDocumento;
        }

        public String Descifrar_Imagen_Documento(String pImagenDocumento)
        {
            var objCryptoHash = new CryptoHash();
            String mensaje = String.Empty;
            String retStrImagenDocumento = String.Empty;

            try
            {
                retStrImagenDocumento = objCryptoHash.Descifrar_Datos(pImagenDocumento, out mensaje);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retStrImagenDocumento;
        }

        #endregion

        #region Imagenes

        public Bitmap GetBitmap(byte[] buf)
        {
            Int16 width = BitConverter.ToInt16(buf, 18);
            Int16 height = BitConverter.ToInt16(buf, 22);

            Bitmap bitmap = new Bitmap(width, height);

            int imageSize = width * height * 4;
            int headerSize = BitConverter.ToInt16(buf, 10);

            System.Diagnostics.Debug.Assert(imageSize == buf.Length - headerSize);

            int offset = headerSize;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bitmap.SetPixel(x, height - y - 1, Color.FromArgb(buf[offset + 3], buf[offset], buf[offset + 1], buf[offset + 2]));
                    offset += 4;
                }
            }

            return bitmap;
        }

        public System.Drawing.Size LockAspectRatio(string sFilePath, double maxWidth, double maxHeight)
        {
            int newWidth, newHeight;
            var size = new Size();
            var image = Image.FromFile(sFilePath);

            size.Width = image.Width;
            size.Height = image.Height;

            newWidth = image.Width;
            newHeight = image.Height;

            double imgRatio = (double)image.Width / (double)image.Height;

            //Step 1: If image is bigger than container, shrink it
            if (image.Width > maxWidth)
            {
                size.Height = (int)Math.Round(maxWidth * imgRatio, 0);
                size.Width = (int)maxWidth;

                newWidth = size.Width;
                newHeight = size.Height;
            }

            if (newHeight > maxHeight)
            {
                size.Height = (int)maxHeight;
                size.Width = (int)Math.Round(maxHeight * imgRatio, 0);
            }

            //Step 2: If image is smaller than container, stretch it (optional)
            if ((image.Width < maxWidth) && (image.Height < maxHeight))
            {
                if (image.Width < maxWidth)
                {
                    size.Height = (int)Math.Round(maxWidth * imgRatio, 0);
                    size.Width = (int)maxWidth;

                    newWidth = size.Width;
                    newHeight = size.Height;
                }

                if (newHeight > maxHeight)
                {
                    size.Height = (int)maxHeight;
                    size.Width = (int)Math.Round(maxHeight * imgRatio, 0);
                }
            }
            image.Dispose();
            return size;
        }

        public Image Convertir_Array_Byte_En_Imagen(byte[] pArray)
        {
            Image returnImage = null;

            try
            {
                var ms = new MemoryStream(pArray, 0, pArray.Length);
                ms.Write(pArray, 0, pArray.Length);

                returnImage = Image.FromStream(ms, true);
            }
            catch { }

            return returnImage;
        }

        public byte[] Convertir_Imagen_En_Array_Byte(Image pImagen)
        {
            byte[] byteArray = new byte[0];

            using (MemoryStream stream = new MemoryStream())
            {
                pImagen.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }

            return byteArray;
        }

        public double Convertir_A_MegaBytes(long size)
        {
            return (size / 1024f) / 1024f;
        }

        #endregion    

        #region Varios

        /// <summary>
        /// Metodo para obtener el token para enviar a la WebAPI. Este token consiste en un hash SHA512 del Timestamp + KEY de aplicación para validar la integridad y autenticidad de los parámetros utilizados.
        /// </summary>
        /// <param name="TimeStamp">Recibe un TimeStamp con formato debe ser yyyyMMddHHmmssfff Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff")</param>
        /// <returns>String</returns>
        /*public String ObtenerToken_SHA512(String TimeStamp)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(TimeStamp + Convert.ToInt32(ConfigurationManager.AppSettings["Id_App"].ToString()));
            SHA512 SHA512M = new SHA512Managed();
            String token = BitConverter.ToString(SHA512M.ComputeHash(buffer)).Replace("-", "");

            return token;
        }*/

        /// <summary>
        /// Realiza la llamada a la WebAPI de Ciudadano Digital, serializa la Entrada y deserializa la Respuesta.
        /// </summary>
        /// <typeparam name="TEntrada">Declarar el objeto de Entrada al método.</typeparam>
        /// <typeparam name="TRespuesta">Declarar el objeto de Respuesta al método.</typeparam>
        /// <param name="Accion">Recibe la acción específica del controlador de la WebAPI.</param>
        /// <param name="tEntrada">Objeto de entrada de la WebAPI , especificado en TEntrada.</param>
        /// <returns>Objeto de salida de la WebAPI, especificado en TRespuesta.</returns>
        /*public TRespuesta LlamarWebAPI<TEntrada, TRespuesta>(String Accion, TEntrada tEntrada)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Accion);
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            try
            {
                String rawjson = JsonConvert.SerializeObject(tEntrada);
                httpWebRequest.Method = "POST";

                var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());

                streamWriter.Write(rawjson);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var result = streamReader.ReadToEnd();

                TRespuesta respuesta = JsonConvert.DeserializeObject<TRespuesta>(result);

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/

        public bool Es_Input_Numerico(String pCadena)
        {
            bool retOk = false;
            Regex regex = new Regex(@"^[0-9]+$");

            if (regex.IsMatch(pCadena)) retOk = true;

            return retOk;
        }

        public void Crear_Carpeta_Documentos_Temporal(String pPathDocs)
        {
            bool exists = System.IO.Directory.Exists(pPathDocs);
            if (!exists)
                System.IO.Directory.CreateDirectory(pPathDocs);
        }

        public void Eliminar_Carpeta_Documentos_Temporal(String pPathDocs)
        {
            DirectoryInfo di = new DirectoryInfo(pPathDocs);
            FileInfo[] FilesDoc = di.GetFiles("*.*");

            foreach (FileInfo itemfile in FilesDoc)
                File.Delete(itemfile.FullName);

            System.IO.Directory.Delete(pPathDocs);
        }

        public string Cifrar(string pTextoACifrar)
        {
            string key = Key_Cif_Decif;
            byte[] clearBytes = Encoding.Unicode.GetBytes(pTextoACifrar);

            using (Aes encriptadorAES = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encriptadorAES.Key = pdb.GetBytes(32);
                encriptadorAES.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encriptadorAES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }

                    pTextoACifrar = Convert.ToBase64String(ms.ToArray());
                }
            }

            return pTextoACifrar;
        }

        public string Descifrar(string pTextoCifrado)
        {
            string key = Key_Cif_Decif;

            pTextoCifrado = pTextoCifrado.Replace(" ", "+");
            byte[] arrBytesCifrados = Convert.FromBase64String(pTextoCifrado);

            using (Aes encriptadorAES = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encriptadorAES.Key = pdb.GetBytes(32);
                encriptadorAES.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encriptadorAES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(arrBytesCifrados, 0, arrBytesCifrados.Length);
                        cs.Close();
                    }

                    pTextoCifrado = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return pTextoCifrado;
        }

        #endregion
    }
}