// Decompiled with JetBrains decompiler
// Type: AppComunicacion.Configuracion
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

namespace AppComunicacion
{
  public class Configuracion
  {
    private string _entorno;
    public const string CiDiConfigName = "AppComunicacion";

    public string Entorno
    {
      get
      {
        return this._entorno;
      }
      set
      {
        this._entorno = value == "produccion" || value == "desarrollo" ? value : string.Empty;
      }
    }

    public string AppId { get; set; }

    public string AppPass { get; set; }

    public string AppKey { get; set; }

    public string SesionHash { get; set; }
  }
}
