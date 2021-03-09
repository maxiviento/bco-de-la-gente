using System;

namespace Infraestructura.Core.Datos
{
    public interface IConventionDefinition
    {
        string FromDatabaseColum(Type type, string originalColumnName);
        string ToDatabaseColumn(Type type, string originalColumName);
    }
}
