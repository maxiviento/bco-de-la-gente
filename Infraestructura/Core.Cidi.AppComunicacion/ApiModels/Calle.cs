// Decompiled with JetBrains decompiler
// Type: AppComunicacion.ApiModels.Calle
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

namespace AppComunicacion.ApiModels
{
  public class Calle
  {
    public int IdCalle { get; set; }

    public string Nombre { get; set; }

    public string id { get; set; }

    public string text { get; set; }

    public Localidad Localidad { get; set; }
  }
}
