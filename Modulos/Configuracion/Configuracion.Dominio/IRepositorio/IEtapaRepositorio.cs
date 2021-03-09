using System.Collections.Generic;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Configuracion.Dominio.IRepositorio
{
    public interface IEtapaRepositorio : IRepositorio<Etapa>
    {
        decimal RegistrarEtapa(Etapa etapa);

        bool ExisteEtapaConElMismoNombre(string nombre);

        Resultado<EtapaResultado.Consulta> ConsultarPorNombre(ConsultaEtapas consulta);

        Etapa ConsultarPorId(decimal id);

        void DarDeBaja(Etapa etapa);

        void Modificar(Etapa etapa);
        IList<EtapaResultado.Consulta> ConsultarEtapas();
        IList<EtapaResultado.Consulta> ConsultarEtapasPorPrestamo(long idPrestamo);
    }
}