using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.Models
{
    public partial class ScoreType
    {
        public ScoreType()
        {
            StudentScores = new HashSet<StudentScore>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public int? Weight { get; set; }
        public int? QtyScoreSemester1 { get; set; }
        public int? QtyScoreSemester2 { get; set; }

        [JsonIgnore]
        public virtual ICollection<StudentScore> StudentScores { get; set; }
    }
}
