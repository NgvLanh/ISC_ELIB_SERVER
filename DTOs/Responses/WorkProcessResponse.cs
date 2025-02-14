using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class WorkProcessResponse
    {
        public long Id { get; set; }
        public long TeacherId { get; set; }
        public string? Organization { get; set; }
        public long SubjectGroupsId { get; set; }
        public string? Position { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsCurrent { get; set; }
        public bool? Active { get; set; }
    }
}
