// Decompiled with JetBrains decompiler
// Type: AppComunicacion.ApiModels.IntegranteGrupo
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

namespace AppComunicacion.ApiModels
{
  public class IntegranteGrupo : PersonaUnica
  {
    public long IdGrupo { get; set; }

    public EstadoGrupo Estado { get; set; }

    public bool Pivote { get; set; }
  }
}
