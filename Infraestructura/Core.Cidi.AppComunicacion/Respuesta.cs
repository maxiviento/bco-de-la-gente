// Decompiled with JetBrains decompiler
// Type: AppComunicacion.Respuesta
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

using System.Collections.Generic;

namespace AppComunicacion
{
  internal class Respuesta
  {
    public string Resultado { get; set; }

    public string CodigoError { get; set; }

    public int Cantidad { get; set; }

    public string ExisteUsuario { get; set; }

    public string SesionHash { get; set; }

    public List<UsuarioCiDi> Usuarios { get; set; }
  }
}
