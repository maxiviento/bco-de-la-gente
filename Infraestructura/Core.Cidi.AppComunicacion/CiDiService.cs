// Decompiled with JetBrains decompiler
// Type: AppComunicacion.CiDiService
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Web;

namespace AppComunicacion
{
  internal class CiDiService : IDisposable
  {
    private CiDiConfiguration _Configuration;
    private HttpContextBase _HttpContext;
    private const string CookieName = "CiDi";
    private const string ObtenerUsuarioAplicacionUrl = "/api/Usuario/Obtener_Usuario_Aplicacion";
    private const string ObtenerUsuarioByCUILUrl = "/api/Usuario/Obtener_Usuario";
    private const string CerrarSessionUsuariosUrl = "/api/Usuario/Cerrar_Sesion_Usuario_Aplicacion";
    private const string ObtenerFotoPerfilUrl = "/api/Documentacion/Obtener_Foto_Perfil";
    private const string RegistrarAppComunicacion = "/api/Interaccion/AutenticarAppComunicacion";
    private const string AutorizarAppComunicacion = "api/Interaccion/AutorizarAppComunicacion";

    public CiDiService(Configuracion config)
    {
      this._Configuration = new CiDiConfiguration()
      {
        AppId = config.AppId,
        AppKey = config.AppKey,
        AppPass = config.AppPass,
        SesionHash = config.SesionHash,
        Entorno = config.Entorno
      };
    }

    public CiDiConfiguration Configuration
    {
      get
      {
        return this._Configuration;
      }
    }

    public string RegisterAppCommunication(int IdAppCom, out string error)
    {
      error = string.Empty;
      string str1 = string.Empty;
      try
      {
        if (this._Configuration != null)
        {
          string appKey = this._Configuration.AppKey;
          string TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
          string str2 = Security.ObtenerTokenSHA512(TimeStamp, appKey);
          HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(CiDiRoutes.GetCiDiUrlAPIInteraccion(this._Configuration.Entorno) + "/api/Interaccion/AutenticarAppComunicacion");
          httpWebRequest.ContentType = "application/json; charset=utf-8";
          EntradaAppCom entradaAppCom = new EntradaAppCom();
          entradaAppCom.IdAplicacion = int.Parse(this._Configuration.AppId);
          if (string.IsNullOrEmpty(this.Configuration.SesionHash))
            entradaAppCom.Contrasenia = this._Configuration.AppPass;
          else
            entradaAppCom.SesionHash = this._Configuration.SesionHash;
          entradaAppCom.IdAplicacionCom = IdAppCom;
          entradaAppCom.TokenValue = str2;
          entradaAppCom.TimeStamp = TimeStamp;
          string str3 = JsonConvert.SerializeObject((object) entradaAppCom);
          httpWebRequest.Method = "POST";
          using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
          {
            streamWriter.Write(str3);
            streamWriter.Flush();
            streamWriter.Close();
            using (StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()))
            {
              RespuestaAppCom respuestaAppCom = (RespuestaAppCom) JsonConvert.DeserializeObject<RespuestaAppCom>(streamReader.ReadToEnd());
              if (respuestaAppCom != null)
              {
                if (respuestaAppCom.Resultado == "OK")
                  str1 = respuestaAppCom.TokenCom;
                else
                  error = respuestaAppCom.Resultado;
              }
              else
                error = "Error al deserialzar la respuesta";
            }
          }
        }
        else
          error = "No se encontro la configuración para el servicio de CiDi";
        return str1;
      }
      catch (Exception ex)
      {
        error = ex.Message;
        return (string) null;
      }
    }

    public void Dispose()
    {
      this._HttpContext = (HttpContextBase) null;
      this._Configuration = (CiDiConfiguration) null;
    }
  }
}
