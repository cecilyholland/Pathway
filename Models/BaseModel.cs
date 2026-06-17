using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathway1.Models
{
    /// <summary>
    /// Base model with common properties for all entities
    /// </summary>
    public abstract class BaseModel
    {
        [PrimaryKey]
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public DateTime? LastSyncedAt { get; set; }
        public bool IsSynced { get; set; }
    }
}
