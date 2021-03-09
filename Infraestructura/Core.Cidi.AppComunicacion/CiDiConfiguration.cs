// Decompiled with JetBrains decompiler
// Type: AppComunicacion.CiDiConfiguration
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

using System.Web;

namespace AppComunicacion
{
  internal class CiDiConfiguration
  {
    private string _entorno;
    public const string CiDiConfigName = "CiDiConfiguration";

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

    public string ConnectionString { get; set; }

    public string SchemaName { get; set; }

    public bool ShowErrorMessage { get; set; }

    public void Register(HttpApplicationState ApplicationState)
    {
      ApplicationState[nameof (CiDiConfiguration)] = (object) this;
    }
  }
}
