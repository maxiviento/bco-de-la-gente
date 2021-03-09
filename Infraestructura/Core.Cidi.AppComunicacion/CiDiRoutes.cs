// Decompiled with JetBrains decompiler
// Type: AppComunicacion.CiDiRoutes
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

namespace AppComunicacion
{
  internal static class CiDiRoutes
  {
    private const string CiDi_Url_produccion = "https://cidi.cba.gov.ar/Login.aspx";
    private const string CiDi_UrlApiCuentaCiDi_produccion = "https://cuentacidi.cba.gov.ar";
    private const string CiDi_UrlChangePass_produccion = "https://cidi.cba.gov.ar/inicio#micuenta";
    private const string CiDi_UrlApiDocumentacion_produccion = "https://documentacioncidi.cba.gov.ar";
    private const string CiDi_UrlApiComunicacion_produccion = "https://comunicacioncidi.cba.gov.ar";
    private const string CiDi_UrlApiMobile_produccion = "https://mobilecidi.cba.gov.ar";
    private const string CiDi_UrlApiInteraccion_produccion = "https://interaccioncidi.cba.gov.ar";
    private const string CiDi_Url_desarrollo = "https://cidi.test.cba.gov.ar/Login.aspx";
    private const string CiDi_UrlApiCuentaCiDi_desarrollo = "https://cuentacidi.test.cba.gov.ar";
    private const string CiDi_UrlChangePass_desarrollo = "https://cidi.test.cba.gov.ar/inicio#micuenta";
    private const string CiDi_UrlApiDocumentacion_desarrollo = "https://documentacioncidi.test.cba.gov.ar";
    private const string CiDi_UrlApiComunicacion_desarrollo = "https://comunicacioncidi.test.cba.gov.ar";
    private const string CiDi_UrlApiMobile_desarrollo = "https://mobilecidi.test.cba.gov.ar";
    private const string CiDi_UrlApiInteraccion_desarrollo = "https://interaccioncidi.test.cba.gov.ar";

    public static string GetCiDiUrl(string entorno)
    {
      string str1 = string.Empty;
      string str2 = entorno;
      if (!(str2 == "produccion"))
      {
        if (str2 == "desarrollo")
          str1 = "https://cidi.test.cba.gov.ar/Login.aspx";
      }
      else
        str1 = "https://cidi.cba.gov.ar/Login.aspx";
      return str1;
    }

    public static string GetCiDiChangePassUrl(string entorno)
    {
      string str1 = string.Empty;
      string str2 = entorno;
      if (!(str2 == "produccion"))
      {
        if (str2 == "desarrollo")
          str1 = "https://cidi.test.cba.gov.ar/inicio#micuenta";
      }
      else
        str1 = "https://cidi.cba.gov.ar/inicio#micuenta";
      return str1;
    }

    public static string GetCiDiUrlAPICuenta(string entorno)
    {
      string str1 = string.Empty;
      string str2 = entorno;
      if (!(str2 == "produccion"))
      {
        if (str2 == "desarrollo")
          str1 = "https://cuentacidi.test.cba.gov.ar";
      }
      else
        str1 = "https://cuentacidi.cba.gov.ar";
      return str1;
    }

    public static string GetCiDiUrlAPIDocumentacion(string entorno)
    {
      string str1 = string.Empty;
      string str2 = entorno;
      if (!(str2 == "produccion"))
      {
        if (str2 == "desarrollo")
          str1 = "https://documentacioncidi.test.cba.gov.ar";
      }
      else
        str1 = "https://documentacioncidi.cba.gov.ar";
      return str1;
    }

    public static string GetCiDiUrlAPIComunicacion(string entorno)
    {
      string str1 = string.Empty;
      string str2 = entorno;
      if (!(str2 == "produccion"))
      {
        if (str2 == "desarrollo")
          str1 = "https://comunicacioncidi.test.cba.gov.ar";
      }
      else
        str1 = "https://comunicacioncidi.cba.gov.ar";
      return str1;
    }

    public static string GetCiDiUrlAPIInteraccion(string entorno)
    {
      string str1 = string.Empty;
      string str2 = entorno;
      if (!(str2 == "produccion"))
      {
        if (str2 == "desarrollo")
          str1 = "https://interaccioncidi.test.cba.gov.ar";
      }
      else
        str1 = "https://interaccioncidi.cba.gov.ar";
      return str1;
    }
  }
}
