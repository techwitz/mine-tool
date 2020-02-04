using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bien.DataAcess.SqlServer
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds the SQL Server database factory into the services collection.
        /// </summary>
        /// <param name="services">The services collection to extend</param>
        /// <param name="config">The configuration to read from</param>
        /// <param name="connectionName">The name of the connection string in <paramref name="config"/></param>
        public static void AddSqlDbFactory(this IServiceCollection services, IConfiguration config, string connectionName)
        {
            services.Configure<SqlStoreOptions>(o =>
            {
                o.ConnectionString = config.GetConnectionString(connectionName);
            });

            services.AddSingleton<IDbFactory, SqlDbFactory>();
        }
    }
}