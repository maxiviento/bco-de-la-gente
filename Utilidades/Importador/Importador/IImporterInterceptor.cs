namespace Utilidades.Importador
{
    public interface IImporterInterceptor<T>
    {
        bool Pre(int rowIndex, string[] columns);
        bool Pos(int rowIndex, ref T t);
        int ValidarDevengadoSuaf(int rowIndex, ref T t);
    }
}