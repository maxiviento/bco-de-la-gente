using System.Collections.Generic;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Configuracion.Dominio.IRepositorio
{
    public interface IMotivoDestinoRepositorio
    {
        IList<MotivoDestino> ConsultarMotivosDestino();
        bool ExisteMotivoDestinoConMismoNombre(string nombre);
        decimal RegistrarMotivoDestino(MotivoDestino motivo);

        MotivoDestino ConsultarPorId(Id id);

        Resultado<MotivoDestinoResultado.Grilla> Consultar(ConsultaMotivoDestino consulta);
        void DarDeBaja(MotivoDestino area);

        void Modificar(MotivoDestino area);
    }
}