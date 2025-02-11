using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class ExamScheduleClass
    {
        public long Id { get; set; }
        public long ClassId { get; set; }
        public long ExampleSchedule { get; set; }
        public long SupervisoryTeacherId { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual ExamSchedule ExampleScheduleNavigation { get; set; } = null!;
        public virtual TeacherInfo SupervisoryTeacher { get; set; } = null!;
    }
}
