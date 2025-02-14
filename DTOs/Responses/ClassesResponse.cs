using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ClassesResponse
    {
        public long Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? StudentQuantity { get; set; }

        public int? SubjectQuantity { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        public long GradeLevelId { get; set; }

        public long AcademicYearId { get; set; }

        public long UserId { get; set; }

        public long ClassTypeId { get; set; }


    }
}
