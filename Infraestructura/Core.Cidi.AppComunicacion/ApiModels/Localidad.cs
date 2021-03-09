// Decompiled with JetBrains decompiler
// Type: AppComunicacion.ApiModels.Localidad
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

namespace AppComunicacion.ApiModels
{
  public class Localidad
  {
    public int IdLocalidad { get; set; }

    public string Nombre { get; set; }

    public string Tipo { get; set; }

    public double? Latitud { get; set; }

    public double? Longitud { get; set; }

    public string CodigoPostal { get; set; }

    public Departamento Departamento { get; set; }
  }
}
