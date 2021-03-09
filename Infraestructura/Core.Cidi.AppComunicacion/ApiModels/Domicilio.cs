// Decompiled with JetBrains decompiler
// Type: AppComunicacion.ApiModels.Domicilio
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

using System.Text;

namespace AppComunicacion.ApiModels
{
  public class Domicilio
  {
    public int IdVin { get; set; }

    public string Altura { get; set; }

    public string Manzana { get; set; }

    public string Lote { get; set; }

    public string Torre { get; set; }

    public string Piso { get; set; }

    public string Dpto { get; set; }

    public string KM { get; set; }

    public string Latitud { get; set; }

    public string Longitud { get; set; }

    public string CodigoPostal { get; set; }

    public Calle Calle { get; set; }

    public TipoCalle TipoCalle { get; set; }

    public Barrio Barrio { get; set; }

    public Complejo Complejo { get; set; }

    public Localidad Localidad { get; set; }

    public Departamento Departamento { get; set; }

    public Provincia Provincia { get; set; }

    public Pais Pais { get; set; }

    public TipoDomicilio TipoDomicilio { get; set; }

    public Aplicacion Aplicacion { get; set; }

    public string NomCatastral { get; set; }

    public string NroCta { get; set; }

    public CaracteristicasDomicilio Caracteristicas { get; set; }

    public string Referencia { get; set; }

    public string DireccionCompleta
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (!string.IsNullOrEmpty(this.Calle.Nombre))
          stringBuilder.Append(this.Calle.Nombre);
        if (!string.IsNullOrEmpty(this.Altura))
          stringBuilder.Append(" " + this.Altura);
        if (!string.IsNullOrEmpty(this.Manzana))
          stringBuilder.Append(" MZA " + this.Manzana);
        if (!string.IsNullOrEmpty(this.Lote))
          stringBuilder.Append(" LTE " + this.Lote);
        if (!string.IsNullOrEmpty(this.Torre))
          stringBuilder.Append(" TORRE " + this.Torre);
        if (!string.IsNullOrEmpty(this.Piso))
          stringBuilder.Append(" P " + this.Piso);
        if (!string.IsNullOrEmpty(this.Dpto))
          stringBuilder.Append(" DPTO " + this.Dpto);
        return stringBuilder.ToString();
      }
    }
  }
}
