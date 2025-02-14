namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class SubjectResponse
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int HoursSemester1 { get; set; }
        public int HoursSemester2 { get; set; }
        public long SubjectGroupId { get; set; }
        public long SubjectTypeId { get; set; }
    }
}
