namespace Infraestructura.Core.Comun.Presentacion
{
    public class ClaveValorResultado<V>
    {
        public ClaveValorResultado(string clave, V valor)
        {
            this.Clave = clave;
            this.Valor = valor;
        }

        public string Clave { get; private set; }
        public V Valor { get; private set; }
    }
}
