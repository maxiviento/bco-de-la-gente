// Decompiled with JetBrains decompiler
// Type: AppComunicacion.ApiModels.PersonaUnica
// Assembly: AppComunicacion, Version=1.2.9.1, Culture=neutral, PublicKeyToken=null
// MVID: A24B4A3D-7CD1-4592-8BBC-E1FD77441103
// Assembly location: C:\Users\CIDS\Desktop\DLL\AppComunicacion.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace AppComunicacion.ApiModels
{
    public class PersonaUnica
    {
        public Sexo Sexo { get; set; }

        public string NroDocumento { get; set; }

        public Pais Nacionalidad { get; set; }

        public Localidad Localidad { get; set; }

        public TipoDocumento TipoDocumento { get; set; }

        public Pais PaisTD { get; set; }

        public int Id_Numero { get; set; }

        public string Apellido { get; set; }

        public string Nombre { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string NroActaNacimiento { get; set; }

        public EstadoCivil EstadoCivil { get; set; }

        public DateTime? FechaDefuncion { get; set; }

        public string NroActaDefuncion { get; set; }

        public string Parentezco { get; set; }


        public string Fallecido { get; set; }

        public string CUIL { get; set; }

        public Domicilio DomicilioLegal { get; set; }

        public Domicilio DomicilioReal { get; set; }

        public Domicilio DomicilioGrupoFamiliar { get; set; }

        public Aplicacion Aplicacion { get; set; }

        public List<Caracteristica> Caracteristicas { get; set; }

        public string NombreCompleto
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (!string.IsNullOrEmpty(this.Apellido))
                    stringBuilder.Append(this.Apellido);
                if (!string.IsNullOrEmpty(this.Nombre))
                    stringBuilder.Append(", " + this.Nombre);
                return stringBuilder.ToString();
            }
        }

        public string FechaNacimientoFormateada
        {
            get
            {
                return this.FechaNacimiento.HasValue ? this.FechaNacimiento.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }

        public string CuilFormateado
        {
            get
            {
                string str = string.Empty;
                if (!string.IsNullOrEmpty(this.CUIL) && this.CUIL.Length == 11)
                    str = this.CUIL.Substring(0, 2) + "-" + this.CUIL.Substring(2, 8) + "-" + this.CUIL.Substring(10, 1);
                return str;
            }
        }
    }
}
