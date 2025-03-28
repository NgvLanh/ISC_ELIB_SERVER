namespace ISC_ELIB_SERVER.Responses
{
    public class ExamScheduleClassResponse
    {
        public int Id { get; set; }
        public int? ClassId { get; set; }
        public int? ExampleSchedule { get; set; }
        public int? SupervisoryTeacherId { get; set; }
        public bool Active { get; set; }
    }
}
