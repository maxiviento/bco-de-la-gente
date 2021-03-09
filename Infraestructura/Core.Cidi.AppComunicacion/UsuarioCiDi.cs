// Decompiled with JetBrains decompiler
// Type: AppComunicacion.UsuarioCiDi
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

namespace AppComunicacion
{
  internal class UsuarioCiDi
  {
    public string CUIL { get; set; }

    public string NroDocumento { get; set; }

    public string Apellido { get; set; }

    public string Nombre { get; set; }

    public string Id_Sexo { get; set; }

    public string PaiCodPais { get; set; }

    public int Id_Numero { get; set; }

    public int? Id_Estado { get; set; }

    public string Estado { get; set; }

    public string Email { get; set; }

    public string TelArea { get; set; }

    public string TelNro { get; set; }

    public string CelArea { get; set; }

    public string CelNro { get; set; }

    public string Empleado { get; set; }

    public string Id_Empleado { get; set; }

    public string FechaRegistro { get; set; }

    public string FechaBloqueo { get; set; }

    public DomicilioCiDi Domicilio { get; set; }

    public Respuesta Respuesta { get; set; }

    public UsuarioCiDi()
    {
      this.Domicilio = new DomicilioCiDi();
      this.Respuesta = new Respuesta();
    }

    public string CUILConFormato
    {
      get
      {
        return this.CUIL.Length == 11 ? this.CUIL.Insert(2, "-").Insert(11, "-") : string.Empty;
      }
    }

    public string NombreCompleto
    {
      get
      {
        return this.Apellido + ", " + this.Nombre;
      }
    }
  }
}
