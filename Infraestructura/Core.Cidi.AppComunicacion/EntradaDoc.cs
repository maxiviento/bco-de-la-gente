// Decompiled with JetBrains decompiler
// Type: AppComunicacion.EntradaDoc
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

namespace AppComunicacion
{
  internal class EntradaDoc : Entrada
  {
    public Documentacion Documentacion { get; set; }

    public EntradaDoc()
    {
      this.Documentacion = new Documentacion();
    }
  }
}
