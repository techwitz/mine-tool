using System.Data;

namespace Bien.DataAcess
{
    /// <summary>
    /// Creates database connections for use in repositories.
    /// </summary>
    public interface IDbFactory
    {
        /// <summary>
        /// Creates a new database connection.
        /// </summary>
        IDbConnection CreateConnection();
    }
}