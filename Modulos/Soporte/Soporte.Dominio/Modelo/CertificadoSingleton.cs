namespace Soporte.Dominio.Modelo
{
    public sealed class CertificadoSingleton
    {
        protected static CertificadoSingleton instance;
        protected static readonly object padlock = new object();

        public CertificadoRentas certificado { get; set; }

        public static CertificadoSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new CertificadoSingleton();
                        }
                    }
                }
                return instance;
            }
        }
    }
}
