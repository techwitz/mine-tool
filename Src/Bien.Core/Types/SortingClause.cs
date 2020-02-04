namespace Bien.Core.Types
{
    /// <summary>
    /// Defines a sorting clause for a given column.
    /// </summary>
    public sealed class SortingClause
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortingClause"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="sortDescending">If set to <c>true</c> then sort descending.</param>
        public SortingClause(string columnName, bool sortDescending = false)
        {
            ColumnName = columnName;
            SortDescending = sortDescending;
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// Gets a value indicating whether to sort descending (if false, then sort ascending).
        /// </summary>
        public bool SortDescending { get; }

        /// <summary>
        /// Parses a <see cref="SortingClause"/> from a string representation.
        /// </summary>
        /// <param name="value">The value to parse - this should be a column or property name;
        /// prefixing it with '-' will cause the sort order to be reversed (i.e. descending).
        /// </param>
        /// <returns>A new <see cref="SortingClause"/> object.</returns>
        public static SortingClause FromString(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                // adding a '-' to the start of the string denotes
                // that this should sort in reverse order
                if (value[0] == '-')
                {
                    return new SortingClause(value.Substring(1), true);
                }
                else
                {
                    return new SortingClause(value, false);
                }
            }

            return new SortingClause(string.Empty);
        }

        public override string ToString() => SortDescending ? $"-{ColumnName}" : ColumnName;
    }
}