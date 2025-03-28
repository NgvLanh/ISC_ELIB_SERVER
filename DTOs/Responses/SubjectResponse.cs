namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class SubjectResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int HoursSemester1 { get; set; }
        public int HoursSemester2 { get; set; }
        public int SubjectGroupId { get; set; }
        public int SubjectTypeId { get; set; }
    }
}
