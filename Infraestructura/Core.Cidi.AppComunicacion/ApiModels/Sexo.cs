// Decompiled with JetBrains decompiler
// Type: AppComunicacion.ApiModels.Sexo
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

namespace AppComunicacion.ApiModels
{
  public class Sexo
  {
    public string IdSexo { get; set; }

    public string Nombre { get; set; }

    public string NombreCorto
    {
      get
      {
        return this.IdSexo == "01" ? "M" : (this.IdSexo == "02" ? "F" : string.Empty);
      }
    }

    public Sexo()
    {
    }

    public Sexo(string idSexo)
    {
      this.IdSexo = idSexo;
    }

    public Sexo(string idSexo, string tipo)
    {
      this.IdSexo = idSexo;
      this.Nombre = tipo;
    }
  }
}
