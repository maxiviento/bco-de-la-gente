// Decompiled with JetBrains decompiler
// Type: AppComunicacion.ApiModels.TipoCaracteristica
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

namespace AppComunicacion.ApiModels
{
  public class TipoCaracteristica
  {
    public string IdTipoCaracteristica { get; set; }

    public string Descripcion { get; set; }

    public bool MultipleValor { get; set; }

    public string Punto { get; set; }

    public string Detalle { get; set; }

    public OrigenCaracteristica Origen { get; set; }

    public TipoCaracteristica()
    {
      this.Origen = new OrigenCaracteristica();
    }
  }
}
