using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bien.Core.Types
{
    /// <summary>
    /// This class describes the result of a store operation.
    /// </summary>
    /// <remarks>
    /// The purpose of this object is to provide a way of returning error codes and
    /// information about the result of an operation without relying on implementation
    /// classes throwing the right type of exceptions. The class can be used in service
    /// contracts to enforce common behaviour.
    /// </remarks>
    /// <example>
    /// public interface IFooStore
    /// {
    ///     Task&lt;StoreResult&gt; SaveFoo(Foo foo);
    /// }
    ///
    /// public class SqlFooStore
    /// {
    ///     public async Task&lt;StoreResult&gt; SaveFoo(Foo foo)
    ///     {
    ///         try
    ///         {
    ///             // do something
    ///             return StoreResult.Success;
    ///         }
    ///         catch (DbException ex)
    ///         {
    ///             return StoreResult.SqlException(ex);
    ///         }
    ///         catch (Exception ex)
    ///         {
    ///             return StoreResult.UnhandledException(ex);
    ///         }
    ///     }
    /// }
    /// </example>
    public class StoreResult
    {
        protected StoreResult()
        {
        }

        protected StoreResult(bool succeeded, params StoreError[] errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToList();
        }

        protected StoreResult(Exception ex)
        {
            Succeeded = false;
            Exception = ex;
            Errors = new StoreError[]
            {
                new StoreError(StoreResultErrorCodes.Unhandled, ex.Message)
            };
        }

        /// <summary>
        /// Gets a success result.
        /// </summary>
        public static StoreResult Success { get; } = new StoreResult(true);

        /// <summary>
        /// Gets a value indicating whether the operation succeeded.
        /// </summary>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// Gets an enumeration of any errors encountered within this operation.
        /// </summary>
        public IList<StoreError> Errors { get; protected set; } = new List<StoreError>();

        public IList<Exception> Exceptions { get; protected set; } = new List<Exception>();

        /// <summary>
        /// Gets the exception thrown by this operation, if any (otherwise <c>null</c>).
        /// </summary>
        public Exception Exception { get; protected set; }

        public bool HasConcurrencyError => Errors.Any(e => e.Code == StoreResultErrorCodes.Concurrency);

        public bool HasUnhandledError => Errors.Any(e => e.Code == StoreResultErrorCodes.Unhandled);

        /// <summary>
        /// Generates a failed <see cref="StoreResult"/> with the specified errors.
        /// </summary>
        /// <param name="errors">Errors thrown in this operation</param>
        /// <returns>A <see cref="StoreResult"/> object representing the failure.</returns>
        public static StoreResult Failure(params StoreError[] errors)
        {
            return new StoreResult(false, errors);
        }

        /// <summary>
        /// Generates a failed <see cref="StoreResult"/> with the specified errors.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="description">Error description</param>
        /// <returns>A <see cref="StoreResult"/> object representing the failure.</returns>
        public static StoreResult Failure(string code, string description = null)
        {
            return new StoreResult(false, new StoreError(code, description));
        }

        /// <summary>
        /// Creates a new <see cref="StoreResult"/> for an unhandled exception.
        /// </summary>
        /// <param name="description">The description for this error (optional)</param>
        public static StoreResult ConcurrencyError(string description = null)
        {
            return new StoreResult(false, new StoreError(StoreResultErrorCodes.Concurrency, description));
        }

        /// <summary>
        /// Creates a new <see cref="StoreResult"/> for an unhandled exception.
        /// </summary>
        /// <param name="ex">The exception that occurred in this operation</param>
        /// <param name="description">The description for this error (optional)</param>
        /// <returns>A <see cref="StoreResult"/> for an exception.</returns>
        public static StoreResult UnhandledException(Exception ex, string description = null)
        {
            return ExceptionResult(ex, StoreResultErrorCodes.Unhandled, description);
        }

        /// <summary>
        /// Creates a new <see cref="StoreResult"/> for a SQL exception.
        /// </summary>
        /// <param name="ex">The SQL exception that occurred in this operation</param>
        /// <param name="description">The description for this error (optional)</param>
        /// <returns>A <see cref="StoreResult"/> for an exception.</returns>
        public static StoreResult DbException(Exception ex, string description = null)
        {
            return ExceptionResult(ex, StoreResultErrorCodes.DbException, description);
        }

        [Obsolete("Please use `DbException` instead; this method is provided for backwards-compatibility")]
        public static StoreResult SqlException(Exception ex, string description = null) => DbException(ex, description);

        /// <summary>
        /// Creates a new <see cref="StoreResult"/> for an exception.
        /// </summary>
        /// <param name="ex">The exception that occurred in this operation</param>
        /// <param name="code">The code for this error</param>
        /// <param name="description">The description for this error (optional)</param>
        /// <returns>A <see cref="StoreResult"/> for an exception.</returns>
        public static StoreResult ExceptionResult(Exception ex, string code, string description = null)
        {
            description = description ?? ex?.GetBaseException().Message;
            return new StoreResult(false, new StoreError(code, description))
            {
                Exception = ex
            };
        }

        /// <summary>
        /// Merges the data of this result with that of another, producing a third
        /// object containing the union of both objects.
        /// </summary>
        /// <param name="other">The other result to merge with</param>
        /// <returns>A new <see cref="StoreResult"/> containing all errors from both
        /// objects, and the logical outcome of both (so if either fails, the output
        /// result also fails).</returns>
        public StoreResult MergeWith(StoreResult other)
        {
            if (other == null)
            {
                return this;
            }

            var errors = Errors.Union(other.Errors).Distinct();
            var success = Succeeded && other.Succeeded;
            return new StoreResult(success, errors.ToArray())
            {
                Exception = other.Exception
            };
        }

        /// <summary>
        /// Merges the data of this result with that of another, producing a third
        /// object containing the union of both objects.
        /// </summary>
        /// <param name="task">Asynchronous task returning a <see cref="StoreResult"/>.</param>
        /// <returns>A new <see cref="StoreResult"/> containing all errors from both
        /// objects, and the logical outcome of both (so if either fails, the output
        /// result also fails).</returns>
        public async Task<StoreResult> MergeWithAsync(Task<StoreResult> task)
        {
            return MergeWith(await task);
        }

        public override string ToString()
        {
            if (Succeeded)
            {
                return "Success";
            }
            else if (Exception != null)
            {
                return Exception.Message;
            }
            else
            {
                var error = Errors.FirstOrDefault();
                return error?.ToString() ?? "Failed";
            }
        }
    }
}