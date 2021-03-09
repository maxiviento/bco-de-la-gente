// Decompiled with JetBrains decompiler
// Type: AppComunicacion.Security
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

using System;
using System.Security.Cryptography;
using System.Text;

namespace AppComunicacion
{
  internal class Security
  {
    public static string Encrypt(string sTexto)
    {
      return AppComunicacion.Security.GetString(new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(sTexto)));
    }

    private static string GetString(byte[] b)
    {
      StringBuilder stringBuilder = new StringBuilder(b.Length);
      for (int index = 0; index < b.Length - 1; ++index)
        stringBuilder.Append(b[index].ToString("X2"));
      return stringBuilder.ToString();
    }

    public static string ObtenerTokenSHA1(string TimeStamp, string Key)
    {
      return BitConverter.ToString(new SHA1Managed().ComputeHash(Encoding.ASCII.GetBytes(TimeStamp + Key))).Replace("-", "");
    }

    public static string ObtenerTokenSHA256(string TimeStamp, string Key)
    {
      return BitConverter.ToString(new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(TimeStamp + Key))).Replace("-", "");
    }

    public static string ObtenerTokenSHA512(string TimeStamp, string Key)
    {
      return BitConverter.ToString(new SHA512Managed().ComputeHash(Encoding.ASCII.GetBytes(TimeStamp + Key))).Replace("-", "");
    }
  }
}
