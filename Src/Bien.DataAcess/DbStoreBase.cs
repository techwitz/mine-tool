using Dapper;
using MicroOrm.Dapper.Repositories;
using System;
using System.Data;

namespace Bien.DataAcess
{
    /// <summary>
    /// Base class for database repositories.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class DbStoreBase : IDisposable
    {
        private readonly Lazy<IDbConnection> _connection;

        static DbStoreBase()
        {
            // add type handlers for all custom (i.e. non-primitive, non-SQL Server default)
            // types that might be passed into Dapper as parameters
            //SqlMapper.AddTypeHandler(new HostnameTypeHandler());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbStoreBase"/> class.
        /// </summary>
        /// <param name="factory">Factory to create the inner <see cref="IDbConnection"/> object.</param>
        protected DbStoreBase(IDbFactory factory)
        {
            _connection = new Lazy<IDbConnection>(
                () =>
                {
                    var db = factory.CreateConnection();
                    db.Open();
                    return db;
                },
                true);
        }

        /// <summary>
        /// Finalizes the <see cref="DbStoreBase"/> object.
        /// </summary>
        ~DbStoreBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the inner SQL connection.
        /// </summary>
        protected IDbConnection Db => _connection.Value;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_connection.IsValueCreated)
                {
                    if (_connection.Value.State != ConnectionState.Closed)
                    {
                        _connection.Value.Close();
                    }

                    _connection.Value.Dispose();
                }
            }
        }
    }

    public abstract class DbStoreBase<TEntity> : DapperRepository<TEntity>, IDisposable where TEntity : class
    {
        private readonly Lazy<IDbConnection> _connection;

        static DbStoreBase()
        {
            // add type handlers for all custom (i.e. non-primitive, non-SQL Server default)
            // types that might be passed into Dapper as parameters
            //SqlMapper.AddTypeHandler(new HostnameTypeHandler());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbStoreBase"/> class.
        /// </summary>
        /// <param name="factory">Factory to create the inner <see cref="IDbConnection"/> object.</param>
        protected DbStoreBase(IDbFactory factory) : base(factory.CreateConnection())
        {
            _connection = new Lazy<IDbConnection>(
                () =>
                {
                    var db = factory.CreateConnection();
                    db.Open();
                    return db;
                },
                true);
        }

        /// <summary>
        /// Finalizes the <see cref="DbStoreBase"/> object.
        /// </summary>
        ~DbStoreBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the inner SQL connection.
        /// </summary>
        protected IDbConnection Db => _connection.Value;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_connection.IsValueCreated)
                {
                    if (_connection.Value.State != ConnectionState.Closed)
                    {
                        _connection.Value.Close();
                    }

                    _connection.Value.Dispose();
                }
            }
        }
    }
}