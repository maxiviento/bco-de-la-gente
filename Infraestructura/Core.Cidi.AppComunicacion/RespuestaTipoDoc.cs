// Decompiled with JetBrains decompiler
// Type: AppComunicacion.RespuestaTipoDoc
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

using System.Collections.Generic;

namespace AppComunicacion
{
  internal class RespuestaTipoDoc : Respuesta
  {
    public List<TipoDocumentacion> TipoDocList { get; set; }

    public RespuestaTipoDoc()
    {
      this.TipoDocList = new List<TipoDocumentacion>();
    }
  }
}
