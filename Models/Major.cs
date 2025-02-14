//using System;
//using System.Collections.Generic;

//namespace ISC_ELIB_SERVER.Models
//{
//    public partial class Major
//    {
//        public long Id { get; set; }
//        public string? Name { get; set; }
//        public string? Description { get; set; }
//        public bool? Active { get; set; }
//    }
//}
using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Major
    {
        public Major()
        {
            TrainingPrograms = new HashSet<TrainingProgram>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? Active { get; set; }

        // Mối quan hệ 1-N với TrainingProgram
        public virtual ICollection<TrainingProgram> TrainingPrograms { get; set; }
    }
}
