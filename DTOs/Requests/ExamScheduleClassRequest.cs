namespace ISC_ELIB_SERVER.Requests
{
    public class ExamScheduleClassRequest
    {
        public int? ClassId { get; set; }
        public int? ExampleSchedule { get; set; }
        public int? SupervisoryTeacherId { get; set; }
        public bool Active { get; set; }
    }
}
