using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bien.Core.Types
{
    /// <summary>
    /// Describes data fetched as part of a paged dataset, containing the current rows
    /// and information about the pagination.
    /// </summary>
    /// <typeparam name="T">Type of object in this data set</typeparam>
    public class PagedDataSet<T>
    {
        private readonly Lazy<int> _totalPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedDataSet{T}"/> class.
        /// </summary>
        /// <param name="data">The rows returned in this dataset</param>
        /// <param name="currentPage">The current page number</param>
        /// <param name="pageSize">The maximum number of rows that can be returned in this page</param>
        /// <param name="recordsTotal">The total records available for this query</param>
        public PagedDataSet(IList<T> data, int currentPage, int pageSize, int recordsTotal)
        {
            Data = data;
            RecordsTotal = recordsTotal;
            CurrentPage = currentPage;
            PageSize = pageSize;
            _totalPages = new Lazy<int>(() => (int)Math.Ceiling((double)RecordsTotal / Math.Max(1, PageSize)));
        }

        /// <summary>
        /// Gets the total, un-paged number of records in this collection, or 0 if not known.
        /// </summary>
        [JsonPropertyName("recordsTotal")]
        public int RecordsTotal { get; }

        /// <summary>
        /// Gets the number of records in this page.
        /// </summary>
        [JsonPropertyName("recordsFiltered")]
        public int RecordsFiltered => RecordsTotal;

        /// <summary>
        /// Gets the current 1-based page number.
        /// </summary>
        [JsonPropertyName("page")]
        public int CurrentPage { get; }

        /// <summary>
        /// Gets the number of records shown per page.
        /// </summary>
        /// <remarks>This may not be the same as <c>Data.Count</c>, if there are less items in
        /// the data collection than the maximum number per page.</remarks>
        [JsonPropertyName("pageSize")]
        public int PageSize { get; }

        /// <summary>
        /// Gets the total number of pages in this set.
        /// </summary>
        [JsonPropertyName("numPages")]
        public int TotalPages => _totalPages.Value;

        /// <summary>
        /// Gets the rows in this page.
        /// </summary>
        [JsonPropertyName("data")]
        public IList<T> Data { get; } = new List<T>();
    }
}