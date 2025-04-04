namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class SessionStudentResponse
    {
        public int ClassId { get; set; }
        public string? ClassCode { get; set; }
        public int SessionId { get; set; }
        public SubjectDto? Subject { get; set; }
        public TeacherDto? Teacher { get; set; }
        public string? Status { get; set; }
        public string? SessionTime { get; set; } // Thêm thuộc tính này
    }

    public class SubjectDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class TeacherDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
