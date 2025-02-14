using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class SubjectGroupResponse
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long TeacherId { get; set; }
    }
}
