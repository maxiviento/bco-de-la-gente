// Decompiled with JetBrains decompiler
// Type: AppComunicacion.RespuestaDocList
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

using System.Collections.Generic;

namespace AppComunicacion
{
  internal class RespuestaDocList : Respuesta
  {
    public List<Documentacion> Documentos { get; set; }

    public RespuestaDocList()
    {
      this.Documentos = new List<Documentacion>();
    }
  }
}
