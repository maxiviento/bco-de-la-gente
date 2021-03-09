// Decompiled with JetBrains decompiler
// Type: AppComunicacion.Documentacion
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

namespace AppComunicacion
{
  internal class Documentacion
  {
    public int IdDocumento { get; set; }

    public int IdUsuario { get; set; }

    public int IdTipo { get; set; }

    public string NombreTipo { get; set; }

    public string FechaCreacion { get; set; }

    public string FechaVencimiento { get; set; }

    public int IdUbicacion { get; set; }

    public string Ubicacion { get; set; }

    public string IdOperador { get; set; }

    public string Operador { get; set; }

    public int IdOrganismo { get; set; }

    public string Organismo { get; set; }

    public byte[] Imagen { get; set; }

    public byte[] VistaPrevia { get; set; }

    public string Extension { get; set; }

    public string Descripcion { get; set; }

    public string Acumulable { get; set; }

    public string Repositorio { get; set; }
  }
}
