using System.Collections.Generic;

namespace Formulario.Aplicacion.Comandos
{
    public class GuardarMercadoComercializacionComando
    {
        public decimal IdFormulario { get; set; }
        public List<SeleccionItemsComando> ItemsCheckeados { get; set; }
        public List<FormaPagoItem> FormasPago { get; set; }
        public EstimaClientesComando EstimaClientes { get; set; }
    }
}
