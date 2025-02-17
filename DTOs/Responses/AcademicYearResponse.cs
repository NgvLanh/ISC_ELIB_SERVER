using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class AcademicYearResponse
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public long SchoolId { get; set; }
    }
}
