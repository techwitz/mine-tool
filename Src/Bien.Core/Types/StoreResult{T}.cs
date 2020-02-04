using System;

namespace Bien.Core.Types
{
    /// <summary>
    /// This class describes the result of a store operation with a given return object type.
    /// </summary>
    /// <typeparam name="T">Type of object returned</typeparam>
    public class StoreResult<T> : StoreResult
    {
        public StoreResult(T value)
            : this(value, true)
        {
        }

        public StoreResult(T value, bool succeeded, params StoreError[] errors)
            : base(succeeded, errors)
        {
            Value = value;
        }

        public StoreResult(T value, bool succeeded, Exception ex, params StoreError[] errors)
            : base(succeeded, errors)
        {
            Exception = ex;
            Value = value;
        }

        public StoreResult(Exception ex)
            : base(ex)
        {
        }

        protected StoreResult()
            : base()
        {
        }

        /// <summary>
        /// Gets the return value, or null.
        /// </summary>
        public T Value { get; protected set; }

        /// <summary>
        /// Creates a new <see cref="StoreResult"/> for an unhandled exception.
        /// </summary>
        /// <param name="ex">The exception that occurred in this operation</param>
        /// <param name="description">The description for this error (optional)</param>
        /// <returns>A <see cref="StoreResult"/> for an exception.</returns>
        public static StoreResult<TResult> UnhandledException<TResult>(Exception ex, string description = null)
        {
            return new StoreResult<TResult>
            {
                Succeeded = false,
                Exception = ex,
                Errors = new StoreError[] { new StoreError(StoreResultErrorCodes.Unhandled, description) }
            };
        }
    }
}