using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace Bien.DataAcess.SqlServer
{
    /// <summary>
    /// Implements <see cref="IDbFactory"/> TechWitz a <see cref="SqlConnection"/> object.
    /// </summary>
    public class SqlDbFactory : IDbFactory
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public SqlDbFactory(IOptions<SqlStoreOptions> options, ILoggerFactory loggerFactory)
        {
            _connectionString = options?.Value?.ConnectionString;
            _logger = loggerFactory.CreateLogger<SqlDbFactory>();
        }

        public IDbConnection CreateConnection()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.StateChange += SqlConnection_StateChange;
            sqlConnection.InfoMessage += SqlConnection_InfoMessage;

            _logger.LogTrace("Opening MSSQL connection to {ConnectionString}", sqlConnection.ConnectionString);
            return sqlConnection;
        }

        private void SqlConnection_StateChange(object sender, StateChangeEventArgs e)
        {
            _logger.LogTrace("SQL connection state changed from {OriginalState} to {CurrentState}", e.OriginalState, e.CurrentState);
        }

        private void SqlConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            if (e.Errors.Count > 0)
            {
                for (var i = 0; i < e.Errors.Count; i++)
                {
                    var error = e.Errors[i];
                    _logger.LogError(
                        "MSSQL error {Number}-{State} at {LineNumber} in {Procedure}: {Message}",
                        error.Number,
                        error.State,
                        error.LineNumber,
                        error.Procedure,
                        error.Message);
                }
            }
            else
            {
                _logger.LogDebug(e.Message);
            }
        }
    }
}