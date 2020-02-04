using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Bien.Core.Models
{
    [Serializable]
    public abstract class EntityBase
    {
        /// <summary>
        /// The propose of this attribute is to hold Uid or identity of the table and this will not be avilable on UI.
        /// </summary>
        [JsonIgnore]
        public long Uid { get; set; }

        /// <summary>
        /// The propose of this attribute is to hold the record key or identifier for a row in database table
        /// </summary>
        public string EntityKey { get; set; }

        [JsonPropertyName("concurrencyToken")]
        public byte[] RowStamp { get; private set; } = new byte[] { };

        public DateTimeOffset? Created { get; set; }

        [JsonIgnore]
        public string CreateBy { get; set; }
    }
}