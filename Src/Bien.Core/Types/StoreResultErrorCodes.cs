namespace Bien.Core.Types
{
    /// <summary>
    /// Defines common error codes for <see cref="StoreResult"/> objects.
    /// </summary>
    public static class StoreResultErrorCodes
    {
        /// <summary>
        /// An optimistic concurrency error, usually indicating an out-of-date concurrency token.
        /// </summary>
        public const string Concurrency = "CONCURRENCY_ERROR";

        /// <summary>
        /// The data supplied failed one or more validation rules.
        /// </summary>
        public const string InvalidData = "INVALID_DATA";

        /// <summary>
        /// A SQL exception.
        /// </summary>
        public const string DbException = "DB_ERROR";

        /// <summary>
        /// A fault in an external or third-party service.
        /// </summary>
        public const string ExternalServiceFault = "SERVICE_FAULT";

        /// <summary>
        /// An unhandled exception.
        /// </summary>
        public const string Unhandled = "ERROR";

        /// <summary>
        /// No data was found for a given key.
        /// </summary>
        public const string NotFound = "NOT_FOUND";
    }
}