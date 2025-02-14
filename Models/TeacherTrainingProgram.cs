//using System;
//using System.Collections.Generic;

//namespace ISC_ELIB_SERVER.Models
//{
//    public partial class TeacherTrainingProgram
//    {
//        public long TeacherId { get; set; }
//        public long TrainingProgramId { get; set; }
//    }
//}
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TeacherTrainingProgram
    {
        public long TeacherId { get; set; }
        public long TrainingProgramId { get; set; }

        public virtual TeacherInfo Teacher { get; set; } = null!;

        public virtual TrainingProgram TrainingProgram { get; set; } = null!;
    }
}
