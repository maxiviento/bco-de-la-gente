// Decompiled with JetBrains decompiler
// Type: AppComunicacion.ApiModels.GrupoFamiliar
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

using System.Collections.Generic;

namespace AppComunicacion.ApiModels
{
  public class GrupoFamiliar
  {
    public long? IdGrupo { get; set; }

    public Domicilio Domicilio { get; set; }

    public IList<IntegranteGrupo> Integrantes { get; set; }

    public EntradaHistoricoGrupos UltimaModificacionGrupo { get; set; }

    public GrupoFamiliar()
    {
      this.Integrantes = (IList<IntegranteGrupo>) new List<IntegranteGrupo>();
    }
  }
}
