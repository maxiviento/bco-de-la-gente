using System;
using System.Security.Cryptography;
using System.Text;

namespace Infraestructura.Core.CiDi.Util
{
    public static class TokenUtil
    {
        /// <summary>
        /// Metodo para obtener el token para enviar a la WebAPI. Este token consiste en un hash SHA512 del Timestamp + KEY de aplicación para validar la integridad y autenticidad de los parámetros utilizados.
        /// </summary>
        /// <param name="timeStamp">Recibe un TimeStamp con formato debe ser yyyyMMddHHmmssfff Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff")</param>
        /// <param name="key"></param>
        /// <returns>String</returns>
        public static string ObtenerToken_SHA512(string timeStamp, string key)
        {
            var buffer = Encoding.ASCII.GetBytes(timeStamp + key);
            SHA512 sha512M = new SHA512Managed();
            return BitConverter.ToString(sha512M.ComputeHash(buffer)).Replace("-", "");
        }

        /// <summary>
        /// Metodo para obtener el token para enviar a la WebAPI. Este token consiste en un hash SHA1 del Timestamp + KEY de aplicación para validar la integridad y autenticidad de los parámetros utilizados.
        /// </summary>
        /// <param name="timeStamp">Recibe un TimeStamp con formato debe ser yyyyMMddHHmmssfff Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff")</param>
        /// <param name="key"></param>
        /// <returns>String</returns>
        public static string ObtenerToken_SHA1(string timeStamp, string key)
        {
            var buffer = Encoding.ASCII.GetBytes(timeStamp + key);
            SHA1 sha1 = new SHA1Managed();
            return BitConverter.ToString(sha1.ComputeHash(buffer)).Replace("-", "");
        }
    }
}
