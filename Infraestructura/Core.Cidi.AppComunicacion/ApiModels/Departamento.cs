﻿// Decompiled with JetBrains decompiler
// Type: AppComunicacion.ApiModels.Departamento
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

namespace AppComunicacion.ApiModels
{
  public class Departamento
  {
    public int IdDepartamento { get; set; }

    public string Nombre { get; set; }

    public Provincia Provincia { get; set; }
  }
}
