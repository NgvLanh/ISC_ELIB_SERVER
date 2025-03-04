using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ClassesResponse
    {

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? StudentQuantity { get; set; }

        public int? SubjectQuantity { get; set; }

        public string? Description { get; set; }

        public int GradeLevelId { get; set; }

        public int AcademicYearId { get; set; }

        public int UserId { get; set; }

        public int ClassTypeId { get; set; }


    }
}
