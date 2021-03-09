namespace Soporte.Dominio.Modelo
{
    public enum MotivoRechazoEnum
    {
        PorDefectoRecupero = 24,
        NoCoincidenImporteCabeceraConDetalle = 25,
        TipoPagoDetalleDistintoCabecera = 26,
        LongitudFila70Banco = 28,
        NoSeEncuentraFormularioConDni = 29,
        ErrorDatoResultadoBanco = 31,
        EstadoFormularioIncorrecto = 32,
        FormularioYaFuePagado = 33,
        FormularioYaImpago = 34,
        LongitudFila35Banco = 36,
        NoSeEncontroFormulario = 39,
        PrestamoNoTienePlanPagoGenerado = 42,
        PorDefecto = 62,
        CaracteresNoNumericos = 68
    }
}