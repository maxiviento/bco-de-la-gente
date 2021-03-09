// Decompiled with JetBrains decompiler
// Type: AppComunicacion.ApiModels.OrigenCaracteristica
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

namespace AppComunicacion.ApiModels
{
  public class OrigenCaracteristica
  {
    public string IdOrigenCaracteristica { get; set; }

    public string Descripcion { get; set; }

    public TipoDato TipoDato { get; set; }

    public OrigenCaracteristica()
    {
      this.TipoDato = new TipoDato();
    }
  }
}
