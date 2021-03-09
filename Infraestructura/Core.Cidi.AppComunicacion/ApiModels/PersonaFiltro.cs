// Decompiled with JetBrains decompiler
// Type: AppComunicacion.ApiModels.PersonaFiltro
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

namespace AppComunicacion.ApiModels
{
  public class PersonaFiltro
  {
    public string Sexo { get; set; }

    public string PaisTD { get; set; }

    public string NroDocumento { get; set; }

    public int? Id_numero { get; set; }

    public string CUIL { get; set; }

    public bool IsValid()
    {
      return !string.IsNullOrEmpty(this.Sexo) && this.Sexo.Length == 2 && (!string.IsNullOrEmpty(this.PaisTD) && this.PaisTD.Length == 3) && !string.IsNullOrEmpty(this.NroDocumento) || !string.IsNullOrEmpty(this.CUIL);
    }

    public string ObtenerIdEntidad()
    {
      return this.Sexo + this.PaisTD + this.NroDocumento + (this.Id_numero.HasValue ? this.Id_numero.Value.ToString() : "-");
    }
  }
}
