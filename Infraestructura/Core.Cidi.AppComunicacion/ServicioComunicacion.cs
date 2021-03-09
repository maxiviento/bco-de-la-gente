// Decompiled with JetBrains decompiler
// Type: AppComunicacion.ServicioComunicacion
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

using AppComunicacion.ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace AppComunicacion
{
  public class ServicioComunicacion
  {
    private const int IdAppComun = 178;

    private Configuracion _Configuracion { get; set; }

    public ServicioComunicacion(Configuracion config)
    {
      try
      {
        if (config == null)
          throw new ApplicationException("Debe proveer los datos de configuración.");
        if (string.IsNullOrEmpty(config.AppId) || string.IsNullOrEmpty(config.AppKey) || string.IsNullOrEmpty(config.AppPass) || string.IsNullOrEmpty(config.Entorno))
          throw new ApplicationException("Faltan datos de configuración.");
        int result;
        if (!int.TryParse(config.AppId, out result))
          throw new ApplicationException("El Id de aplicación debe ser numérico.");
        if (!int.TryParse(config.AppId, out result))
          throw new ApplicationException("El Id de aplicación debe ser numérico.");
        this._Configuracion = config;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public string GruposFamiliares(
      string UsuarioCiDi,
      PersonaFiltro filtroPersona,
      RolesGruposFamiliar rol,
      ParametrosURL paramUrl = null)
    {
      try
      {
        string str = string.Empty;
        ParametrosComunicacion parametrosComunicacion = this.ObtenerParametrosComunicacion(UsuarioCiDi, filtroPersona.ObtenerIdEntidad(), (int) rol);
        switch (rol)
        {
          case RolesGruposFamiliar.MODIFICAR_DOMI_GRUPOS:
            str = RutasAppComunes.GetUrlGruposFamiliaresModifDomiUrl(this._Configuracion.Entorno) + this._Configuracion.AppId + "/" + parametrosComunicacion.TimeStamp + "/" + parametrosComunicacion.TokenSesion + "/" + ((int) rol).ToString() + "/" + 1.ToString() + "/" + filtroPersona.ObtenerIdEntidad();
            break;
          case RolesGruposFamiliar.MODIFICAR_GRUPOS_APP_INTERNA:
            str = RutasAppComunes.GetUrlGruposFamiliaresUrl(this._Configuracion.Entorno) + this._Configuracion.AppId + "/" + parametrosComunicacion.TimeStamp + "/" + parametrosComunicacion.TokenSesion + "/" + ((int) rol).ToString() + "/" + 1.ToString() + "/" + filtroPersona.ObtenerIdEntidad();
            break;
          case RolesGruposFamiliar.CONSULTAR_GRUPOS:
            str = RutasAppComunes.GetUrlGruposFamiliaresConsultaUrl(this._Configuracion.Entorno) + this._Configuracion.AppId + "/" + parametrosComunicacion.TimeStamp + "/" + parametrosComunicacion.TokenSesion + "/" + ((int) rol).ToString() + "/" + 1.ToString() + "/" + filtroPersona.ObtenerIdEntidad();
            break;
        }
        return paramUrl == null ? str + "/?NavBar=False" : str + "/?NavBar=" + paramUrl.ShowNavBar.ToString();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public string Personas(string UsuarioCiDi, PersonaFiltro filtroPersona, RolesPersonas rol)
    {
      try
      {
        string str = string.Empty;
        ParametrosComunicacion parametrosComunicacion = this.ObtenerParametrosComunicacion(UsuarioCiDi, filtroPersona.ObtenerIdEntidad(), (int) rol);
        switch (rol)
        {
          case RolesPersonas.CONSULTAR_PERSONA_BASICO:
            str = RutasAppComunes.GetUrlConsultarPersonasBasico(this._Configuracion.Entorno);
            break;
          case RolesPersonas.CONSULTAR_PERSONA_COMPLETO:
            str = RutasAppComunes.GetUrlConsultarPersonas(this._Configuracion.Entorno);
            break;
          case RolesPersonas.CONSULTAR_PERSONA_ESPECIAL:
            str = RutasAppComunes.GetUrlConsultarPersonasEspUrl(this._Configuracion.Entorno);
            break;
          case RolesPersonas.ALTA_PERSONA:
            str = RutasAppComunes.GetUrlAgregarPersona(this._Configuracion.Entorno);
            break;
          case RolesPersonas.MODIFICAR_PERSONA:
            str = RutasAppComunes.GetUrlModificarPersona(this._Configuracion.Entorno);
            break;
          case RolesPersonas.CONSULTAR_CARACTERISTICAS:
            str = RutasAppComunes.GetUrlConsultarCaracteristicasPersona(this._Configuracion.Entorno);
            break;
          case RolesPersonas.MODIFICAR_CARACTERISTICAS:
            str = RutasAppComunes.GetUrlModificarCaracteristicasPersona(this._Configuracion.Entorno);
            break;
          case RolesPersonas.ALTA_PERSONA_IND:
            str = RutasAppComunes.GetUrlAgregarIndPersona(this._Configuracion.Entorno);
            break;
        }
        if (string.IsNullOrEmpty(str))
          throw new ApplicationException("Debe proveer un rol válido.");
        return str + this._Configuracion.AppId + "/" + parametrosComunicacion.TimeStamp + "/" + parametrosComunicacion.TokenSesion + "/" + ((int) rol).ToString() + "/" + 1.ToString() + "/" + filtroPersona.ObtenerIdEntidad();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public string Domicilios(string UsuarioCiDi, string EntidadConsulta, RolesDomicilio rol)
    {
      try
      {
        string str = string.Empty;
        ParametrosComunicacion parametrosComunicacion = this.ObtenerParametrosComunicacion(UsuarioCiDi, EntidadConsulta, (int) rol);
        switch (rol)
        {
          case RolesDomicilio.CONSULTAR_DOMICILIO:
            str = RutasAppComunes.GetUrlDomiciliosConsultar(this._Configuracion.Entorno);
            break;
          case RolesDomicilio.CONSULTAR_CARACTERISTICAS:
            str = RutasAppComunes.GetUrlConsultarCaracteristicasDomicilio(this._Configuracion.Entorno);
            break;
          case RolesDomicilio.MODIFICAR_CARACTERISTICAS:
            str = RutasAppComunes.GetUrlModificarCaracteristicasDomicilio(this._Configuracion.Entorno);
            break;
          case RolesDomicilio.ALTA_DOMICILIO_GENERICO:
            str = RutasAppComunes.GetUrlDomiciliosAgregar(this._Configuracion.Entorno);
            break;
        }
        if (string.IsNullOrEmpty(str))
          throw new ApplicationException("Debe proveer un rol válido.");
        return str + this._Configuracion.AppId + "/" + parametrosComunicacion.TimeStamp + "/" + parametrosComunicacion.TokenSesion + "/" + ((int) rol).ToString() + "/" + 3.ToString() + "/" + EntidadConsulta;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public string Historicos(string UsuarioCiDi, PersonaFiltro filtroPersona, RolesHistoricos rol)
    {
      try
      {
        string str = string.Empty;
        ParametrosComunicacion parametrosComunicacion = this.ObtenerParametrosComunicacion(UsuarioCiDi, filtroPersona.ObtenerIdEntidad(), (int) rol);
        switch (rol)
        {
          case RolesHistoricos.CONSULTAR_HIST_CAR_PERSONA:
            str = RutasAppComunes.GetUrlHistCaractPersonaUrl(this._Configuracion.Entorno);
            break;
          case RolesHistoricos.CONSULTAR_HIST_GRUPO_PERSONA:
            str = RutasAppComunes.GetUrlHistGrupoPersonaUrl(this._Configuracion.Entorno);
            break;
        }
        if (string.IsNullOrEmpty(str))
          throw new ApplicationException("Debe proveer un rol válido.");
        return str + this._Configuracion.AppId + "/" + parametrosComunicacion.TimeStamp + "/" + parametrosComunicacion.TokenSesion + "/" + ((int) rol).ToString() + "/" + 1.ToString() + "/" + filtroPersona.ObtenerIdEntidad();
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public string Domicilios(
      string UsuarioCiDi,
      string EntidadConsulta,
      RolesDomicilio rol,
      int tipoDomicilio,
      JurisdiccionDomicilio jurisdiccion)
    {
      try
      {
        string str = string.Empty;
        ParametrosComunicacion parametrosComunicacion = this.ObtenerParametrosComunicacion(UsuarioCiDi, EntidadConsulta, (int) rol, tipoDomicilio, (int) jurisdiccion);
        switch (rol)
        {
          case RolesDomicilio.CONSULTAR_DOMICILIO:
            str = RutasAppComunes.GetUrlDomiciliosConsultar(this._Configuracion.Entorno);
            break;
          case RolesDomicilio.CONSULTAR_CARACTERISTICAS:
            str = RutasAppComunes.GetUrlConsultarCaracteristicasDomicilio(this._Configuracion.Entorno);
            break;
          case RolesDomicilio.MODIFICAR_CARACTERISTICAS:
            str = RutasAppComunes.GetUrlModificarCaracteristicasDomicilio(this._Configuracion.Entorno);
            break;
          case RolesDomicilio.ALTA_DOMICILIO_GENERICO:
            str = RutasAppComunes.GetUrlDomiciliosAgregar(this._Configuracion.Entorno);
            break;
        }
        if (string.IsNullOrEmpty(str))
          throw new ApplicationException("Debe proveer un rol válido.");
        object[] objArray = new object[16]
        {
          (object) str,
          (object) this._Configuracion.AppId,
          (object) "/",
          (object) parametrosComunicacion.TimeStamp,
          (object) "/",
          (object) parametrosComunicacion.TokenSesion,
          (object) "/",
          (object) ((int) rol).ToString(),
          (object) "/",
          null,
          null,
          null,
          null,
          null,
          null,
          null
        };
        int num = 3;
        objArray[9] = (object) num.ToString();
        objArray[10] = (object) "/";
        objArray[11] = (object) EntidadConsulta;
        objArray[12] = (object) "/";
        objArray[13] = (object) tipoDomicilio;
        objArray[14] = (object) "/";
        num = (int) jurisdiccion;
        objArray[15] = (object) num.ToString();
        return string.Concat(objArray);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public RespuestaAPIGrupoFamiliar ApiGruposFamiliares(
      string TokenUsuarioCiDi,
      PersonaFiltro filtroPersona,
      RolesAPIGruposFamiliar rol)
    {
      try
      {
        RespuestaAPIGrupoFamiliar apiGrupoFamiliar = (RespuestaAPIGrupoFamiliar) JsonConvert.DeserializeObject<RespuestaAPIGrupoFamiliar>(this.ApiGruposFamiliaresJSON(TokenUsuarioCiDi, filtroPersona, rol));
        if (apiGrupoFamiliar == null)
          throw new ApplicationException("Se ha producido un error al deserializar la respuesta de la API.");
        return apiGrupoFamiliar;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public PersonaUnica ApiPersonas(
      string TokenUsuarioCiDi,
      PersonaFiltro filtroPersona,
      RolesAPIPersonas rol)
    {
      try
      {
        PersonaUnica personaUnica = (PersonaUnica) JsonConvert.DeserializeObject<PersonaUnica>(this.ApiPersonasJSON(TokenUsuarioCiDi, filtroPersona, rol));
        if (personaUnica == null)
          throw new ApplicationException("Se ha producido un error al deserializar la respuesta de la API.");
        return personaUnica;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public Domicilio ApiDomicilios(
      string TokenUsuarioCiDi,
      string EntidadConsulta,
      RolesAPIDomicilios rol)
    {
      try
      {
        Domicilio domicilio = (Domicilio) JsonConvert.DeserializeObject<Domicilio>(this.ApiDomiciliosJSON(TokenUsuarioCiDi, EntidadConsulta, rol));
        if (domicilio == null)
          throw new ApplicationException("Se ha producido un error al deserializar la respuesta de la API.");
        return domicilio;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public Domicilio ApiDomicilios(
      string TokenUsuarioCiDi,
      string entidad,
      RolesAPIDomicilios rol,
      int idApp,
      int tipoDomicilio,
      string cuilUsuario)
    {
      try
      {
        Domicilio domicilio = (Domicilio) JsonConvert.DeserializeObject<Domicilio>(this.ApiDomiciliosJSON(TokenUsuarioCiDi, entidad, rol, idApp, tipoDomicilio, cuilUsuario));
        if (domicilio == null)
          throw new ApplicationException("Se ha producido un error al deserializar la respuesta de la API.");
        return domicilio;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public List<Caracteristica> ApiCaracteristicasPersona(
      string TokenUsuarioCiDi,
      PersonaFiltro filtroPersona,
      RolesAPICaracteristicasPersona rol)
    {
      try
      {
        List<Caracteristica> caracteristicaList = (List<Caracteristica>) JsonConvert.DeserializeObject<List<Caracteristica>>(this.ApiCaracteristicasPersonaJSON(TokenUsuarioCiDi, filtroPersona, rol));
        if (caracteristicaList == null)
          throw new ApplicationException("Se ha producido un error al deserializar la respuesta de la API.");
        return caracteristicaList;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public CaracteristicasDomicilio ApiCaracteristicasDomicilio(
      string TokenUsuarioCiDi,
      string EntidadConsulta,
      RolesAPICaracteristicasDomicilio rol)
    {
      try
      {
        CaracteristicasDomicilio caracteristicasDomicilio = (CaracteristicasDomicilio) JsonConvert.DeserializeObject<CaracteristicasDomicilio>(this.ApiCaracteristicasDomicilioJSON(TokenUsuarioCiDi, EntidadConsulta, rol));
        if (caracteristicasDomicilio == null)
          throw new ApplicationException("Se ha producido un error al deserializar la respuesta de la API.");
        return caracteristicasDomicilio;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public string ApiGruposFamiliaresJSON(
      string TokenUsuarioCiDi,
      PersonaFiltro filtroPersona,
      RolesAPIGruposFamiliar rol)
    {
      string str1 = (string) null;
      if (!filtroPersona.IsValid())
        throw new ApplicationException("Los datos para identificar a la persona no son válidos");
      try
      {
        ParametrosComunicacion parametrosComunicacion = this.ObtenerParametrosComunicacionAPI(TokenUsuarioCiDi, filtroPersona.ObtenerIdEntidad(), (int) rol);
        string gruposFamiliares = RutasAppComunes.GetUrlAPIGruposFamiliares(this._Configuracion.Entorno);
        int num = 1;
        string str2 = num.ToString();
        string str3 = filtroPersona.ObtenerIdEntidad();
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(gruposFamiliares + str2 + "/" + str3);
        httpWebRequest.ContentType = "application/json; charset=utf-8";
        httpWebRequest.Method = "GET";
        httpWebRequest.Accept = "application/json; charset=utf-8";
        httpWebRequest.Headers.Add("idapp", this._Configuracion.AppId);
        httpWebRequest.Headers.Add("ts", parametrosComunicacion.TimeStamp);
        httpWebRequest.Headers.Add("token", parametrosComunicacion.TokenSesion);
        httpWebRequest.Headers.Add("tokenusuariocidi", TokenUsuarioCiDi);
        WebHeaderCollection headers = httpWebRequest.Headers;
        num = (int) rol;
        string str4 = num.ToString();
        headers.Add("idrol", str4);
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
        {
          if (response.StatusCode == HttpStatusCode.OK)
          {
            try
            {
              str1 = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
            }
          }
          else
            throw new ApplicationException("Se ha producido un error en la API de Grupos Familiares: " + response.StatusCode.ToString() + " - " + response.StatusDescription + " Contenido: " + response.ToString());
        }
        return str1;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public string ApiPersonasJSON(
      string TokenUsuarioCiDi,
      PersonaFiltro filtroPersona,
      RolesAPIPersonas rol)
    {
      string str = (string) null;
      if (!filtroPersona.IsValid())
        throw new ApplicationException("Los datos para identificar a la persona no son válidos");
      try
      {
        ParametrosComunicacion parametrosComunicacion = string.IsNullOrEmpty(filtroPersona.CUIL) ? this.ObtenerParametrosComunicacionAPI(TokenUsuarioCiDi, filtroPersona.ObtenerIdEntidad(), (int) rol) : this.ObtenerParametrosComunicacionAPI(TokenUsuarioCiDi, filtroPersona.CUIL, (int) rol);
        HttpWebRequest httpWebRequest = string.IsNullOrEmpty(filtroPersona.CUIL) ? (HttpWebRequest) WebRequest.Create(RutasAppComunes.GetUrlAPIPersonas(this._Configuracion.Entorno) + 1.ToString() + "/" + filtroPersona.ObtenerIdEntidad()) : (HttpWebRequest) WebRequest.Create(RutasAppComunes.GetUrlAPIPersonas(this._Configuracion.Entorno) + 4.ToString() + "/" + filtroPersona.CUIL);
        httpWebRequest.ContentType = "application/json; charset=utf-8";
        httpWebRequest.Method = "GET";
        httpWebRequest.Accept = "application/json; charset=utf-8";
        httpWebRequest.Headers.Add("idapp", this._Configuracion.AppId);
        httpWebRequest.Headers.Add("ts", parametrosComunicacion.TimeStamp);
        httpWebRequest.Headers.Add("token", parametrosComunicacion.TokenSesion);
        httpWebRequest.Headers.Add("tokenusuariocidi", TokenUsuarioCiDi);
        httpWebRequest.Headers.Add("idrol", ((int) rol).ToString());
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
        {
          if (response.StatusCode == HttpStatusCode.OK)
          {
            try
            {
              str = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
            }
          }
          else
            throw new ApplicationException("Se ha producido un error en la API de Grupos Familiares: " + response.StatusCode.ToString() + " - " + response.StatusDescription + " Contenido: " + response.ToString());
        }
        return str;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public string ApiDomiciliosJSON(
      string TokenUsuarioCiDi,
      string EntidadConsulta,
      RolesAPIDomicilios rol)
    {
      string str = (string) null;
      try
      {
        ParametrosComunicacion parametrosComunicacion = this.ObtenerParametrosComunicacionAPI(TokenUsuarioCiDi, EntidadConsulta, (int) rol);
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(RutasAppComunes.GetUrlAPIDomicilios(this._Configuracion.Entorno) + 3.ToString() + "/" + EntidadConsulta);
        httpWebRequest.ContentType = "application/json; charset=utf-8";
        httpWebRequest.Method = "GET";
        httpWebRequest.Accept = "application/json; charset=utf-8";
        httpWebRequest.Headers.Add("idapp", this._Configuracion.AppId);
        httpWebRequest.Headers.Add("ts", parametrosComunicacion.TimeStamp);
        httpWebRequest.Headers.Add("token", parametrosComunicacion.TokenSesion);
        httpWebRequest.Headers.Add("tokenusuariocidi", TokenUsuarioCiDi);
        httpWebRequest.Headers.Add("idrol", ((int) rol).ToString());
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
        {
          if (response.StatusCode == HttpStatusCode.OK)
          {
            try
            {
              str = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
            }
          }
          else
            throw new ApplicationException("Se ha producido un error en la API de Domicilios: " + response.StatusCode.ToString() + " - " + response.StatusDescription + " Contenido: " + response.ToString());
        }
        return str;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public string ApiDomiciliosJSON(
      string TokenUsuarioCiDi,
      string entidad,
      RolesAPIDomicilios rol,
      int idApp,
      int tipoDomicilio,
      string cuilUsuario)
    {
      string str = (string) null;
      try
      {
        ParametrosComunicacion parametrosComunicacion = this.ObtenerParametrosComunicacionAPI(TokenUsuarioCiDi, entidad, (int) rol);
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(RutasAppComunes.GetUrlAPIDomicilios(this._Configuracion.Entorno) + entidad + "/" + (object) idApp + "/" + (object) tipoDomicilio + "/" + cuilUsuario);
        httpWebRequest.ContentType = "application/json; charset=utf-8";
        httpWebRequest.Method = "GET";
        httpWebRequest.Accept = "application/json; charset=utf-8";
        httpWebRequest.Headers.Add("idapp", this._Configuracion.AppId);
        httpWebRequest.Headers.Add("ts", parametrosComunicacion.TimeStamp);
        httpWebRequest.Headers.Add("token", parametrosComunicacion.TokenSesion);
        httpWebRequest.Headers.Add("tokenusuariocidi", TokenUsuarioCiDi);
        httpWebRequest.Headers.Add("idrol", ((int) rol).ToString());
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
        {
          if (response.StatusCode == HttpStatusCode.OK)
          {
            try
            {
              str = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
            }
          }
          else
            throw new ApplicationException("Se ha producido un error en la API de Domicilios: " + response.StatusCode.ToString() + " - " + response.StatusDescription + " Contenido: " + response.ToString());
        }
        return str;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public string ApiCaracteristicasPersonaJSON(
      string TokenUsuarioCiDi,
      PersonaFiltro filtroPersona,
      RolesAPICaracteristicasPersona rol)
    {
      string str1 = (string) null;
      if (!filtroPersona.IsValid())
        throw new ApplicationException("Los datos para identificar a la persona no son válidos");
      try
      {
        ParametrosComunicacion parametrosComunicacion = this.ObtenerParametrosComunicacionAPI(TokenUsuarioCiDi, filtroPersona.ObtenerIdEntidad(), (int) rol);
        string caracteristicasPersona = RutasAppComunes.GetUrlAPICaracteristicasPersona(this._Configuracion.Entorno);
        int num = 1;
        string str2 = num.ToString();
        string str3 = filtroPersona.ObtenerIdEntidad();
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(caracteristicasPersona + str2 + "/" + str3);
        httpWebRequest.ContentType = "application/json; charset=utf-8";
        httpWebRequest.Method = "GET";
        httpWebRequest.Accept = "application/json; charset=utf-8";
        httpWebRequest.Headers.Add("idapp", this._Configuracion.AppId);
        httpWebRequest.Headers.Add("ts", parametrosComunicacion.TimeStamp);
        httpWebRequest.Headers.Add("token", parametrosComunicacion.TokenSesion);
        httpWebRequest.Headers.Add("tokenusuariocidi", TokenUsuarioCiDi);
        WebHeaderCollection headers = httpWebRequest.Headers;
        num = (int) rol;
        string str4 = num.ToString();
        headers.Add("idrol", str4);
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
        {
          if (response.StatusCode == HttpStatusCode.OK)
          {
            try
            {
              str1 = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
            }
          }
          else
            throw new ApplicationException("Se ha producido un error en la API de Caracteristicas Persona: " + response.StatusCode.ToString() + " - " + response.StatusDescription + " Contenido: " + response.ToString());
        }
        return str1;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public string ApiCaracteristicasDomicilioJSON(
      string TokenUsuarioCiDi,
      string EntidadConsulta,
      RolesAPICaracteristicasDomicilio rol)
    {
      string str = (string) null;
      try
      {
        ParametrosComunicacion parametrosComunicacion = this.ObtenerParametrosComunicacionAPI(TokenUsuarioCiDi, EntidadConsulta, (int) rol);
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(RutasAppComunes.GetUrlAPICaracteristicasDomicilio(this._Configuracion.Entorno) + 3.ToString() + "/" + EntidadConsulta);
        httpWebRequest.ContentType = "application/json; charset=utf-8";
        httpWebRequest.Method = "GET";
        httpWebRequest.Accept = "application/json; charset=utf-8";
        httpWebRequest.Headers.Add("idapp", this._Configuracion.AppId);
        httpWebRequest.Headers.Add("ts", parametrosComunicacion.TimeStamp);
        httpWebRequest.Headers.Add("token", parametrosComunicacion.TokenSesion);
        httpWebRequest.Headers.Add("tokenusuariocidi", TokenUsuarioCiDi);
        httpWebRequest.Headers.Add("idrol", ((int) rol).ToString());
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
        {
          if (response.StatusCode == HttpStatusCode.OK)
          {
            try
            {
              str = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
            }
          }
          else
            throw new ApplicationException("Se ha producido un error en la API de Caracteristicas de Domicilio: " + response.StatusCode.ToString() + " - " + response.StatusDescription + " Contenido: " + response.ToString());
        }
        return str;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private ParametrosComunicacion ObtenerParametrosComunicacion(
      string UsuarioCiDi,
      string EntidadConsulta,
      int rol)
    {
      try
      {
        string error = string.Empty;
        string str1 = string.Empty;
        using (CiDiService ciDiService = new CiDiService(this._Configuracion))
          str1 = ciDiService.RegisterAppCommunication(178, out error);
        if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(str1))
          throw new ApplicationException(error);
        if (string.IsNullOrEmpty(str1))
          throw new ApplicationException("No se pudo obtener el token de comunicación de la API CiDi");
        string TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string str2 = Security.ObtenerTokenSHA256(TimeStamp, this._Configuracion.AppId + UsuarioCiDi + str1 + rol.ToString() + EntidadConsulta);
        return new ParametrosComunicacion()
        {
          TimeStamp = TimeStamp,
          TokenSesion = str2
        };
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private ParametrosComunicacion ObtenerParametrosComunicacionAPI(
      string TokenUsuarioCiDi,
      string EntidadConsulta,
      int rol)
    {
      try
      {
        string error = string.Empty;
        string str1 = string.Empty;
        using (CiDiService ciDiService = new CiDiService(this._Configuracion))
          str1 = ciDiService.RegisterAppCommunication(178, out error);
        if (!string.IsNullOrEmpty(error))
          throw new ApplicationException(error);
        if (string.IsNullOrEmpty(str1))
          throw new ApplicationException("No se pudo obtener el token de comunicación de la API CiDi");
        string TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string str2 = Security.ObtenerTokenSHA256(TimeStamp, this._Configuracion.AppId + TokenUsuarioCiDi + str1 + rol.ToString() + EntidadConsulta);
        return new ParametrosComunicacion()
        {
          TimeStamp = TimeStamp,
          TokenSesion = str2
        };
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private ParametrosComunicacion ObtenerParametrosComunicacion(
      string UsuarioCiDi,
      string EntidadConsulta,
      int rol,
      int TipoDomicilio,
      int jurisdiccion)
    {
      try
      {
        string error = string.Empty;
        string str1 = string.Empty;
        using (CiDiService ciDiService = new CiDiService(this._Configuracion))
          str1 = ciDiService.RegisterAppCommunication(178, out error);
        if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(str1))
          throw new ApplicationException(error);
        if (string.IsNullOrEmpty(str1))
          throw new ApplicationException("No se pudo obtener el token de comunicación de la API CiDi");
        string TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string str2 = Security.ObtenerTokenSHA256(TimeStamp, this._Configuracion.AppId + UsuarioCiDi + str1 + rol.ToString() + EntidadConsulta + TipoDomicilio.ToString() + jurisdiccion.ToString());
        return new ParametrosComunicacion()
        {
          TimeStamp = TimeStamp,
          TokenSesion = str2
        };
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private ParametrosComunicacion ObtenerParametrosComunicacion(
      string UsuarioCiDi,
      string EntidadConsulta,
      int rol,
      int TipoDomicilio)
    {
      try
      {
        string error = string.Empty;
        string str1 = string.Empty;
        using (CiDiService ciDiService = new CiDiService(this._Configuracion))
          str1 = ciDiService.RegisterAppCommunication(178, out error);
        if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(str1))
          throw new ApplicationException(error);
        if (string.IsNullOrEmpty(str1))
          throw new ApplicationException("No se pudo obtener el token de comunicación de la API CiDi");
        string TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string str2 = Security.ObtenerTokenSHA256(TimeStamp, this._Configuracion.AppId + UsuarioCiDi + str1 + rol.ToString() + EntidadConsulta + TipoDomicilio.ToString());
        return new ParametrosComunicacion()
        {
          TimeStamp = TimeStamp,
          TokenSesion = str2
        };
      }
      catch (Exception ex)
      {
        throw;
      }
    }
  }
}
