using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.Models
{
    public partial class EducationLevel
    {
        public EducationLevel()
        {
            Schools = new HashSet<School>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public bool? Status { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public virtual ICollection<School> Schools { get; set; }
    }
}
