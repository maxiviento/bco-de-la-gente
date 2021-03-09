using System.Collections.Generic;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Configuracion.Aplicacion.Comandos;

namespace Formulario.Dominio.IRepositorio
{
    public interface IMotivoRechazoRepositorio
    {
        IList<MotivoRechazo> ConsultarMotivosRechazo(Ambito ambito);
        IList<MotivoRechazo> ConsultarMotivosRechazoPorAmbito(decimal idAmbito);
        MotivoRechazo ConsultarPorId(Id id, Ambito ambito);
        Resultado<ConsultaMotivoRechazoResultado.Grilla> Consultar(ConsultaMotivoRechazo consulta);
        bool ExisteMotivoRechazoConMismoNombre(string nombre);
        decimal RegistrarMotivoRechazo(MotivoRechazo motivo);
        ConsultaMotivoRechazoResultado.Detallado ConsultarPorIdGeneral(Id idMotivo, Id idAmbito);
        bool Modificar(ModificacionMotivoRechazoComando comando, Usuario usuario );
        void DarDeBaja(MotivoRechazo motivo);
        IList<Ambito> ConsultarAmbitosCombo();
        List<ConsultaMotivoRechazoResultado.Grilla> ObtenerAbreviaturas();
        bool ExisteMotivoRechazoConMismoNombre(int idAmbito, string nombre);
        bool ExisteMotivoRechazoConMismaAbreviatura(int idAmbito, string abreviatura);
        bool ExisteMotivoRechazoConMismoCodigo(int idAmbito, string codigo);
    }
}
