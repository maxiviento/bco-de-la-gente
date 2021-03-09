namespace Identidad.Dominio.Modelo
{
    public interface ISesionUsuario
    {
        Usuario Usuario { get; }
        string CiDiHash { get; }
        bool EstaCerrada { get; }

    }
}
