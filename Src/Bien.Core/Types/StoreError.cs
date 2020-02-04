namespace Bien.Core.Types
{
    /// <summary>
    /// Describes an error.
    /// </summary>
    public class StoreError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoreError"/> class.
        /// </summary>
        /// <param name="code">The error code</param>
        /// <param name="description">The error description (optional)</param>
        public StoreError(string code, string description = null)
        {
            Code = code;
            Description = description ?? string.Empty;
        }

        /// <summary>
        /// Gets the code for this error.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the description of this error, if any.
        /// </summary>
        public string Description { get; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return Code;
            }
            else
            {
                return $"{Code}={Description}";
            }
        }
    }
}