using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class EducationLevelResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [JsonIgnoreCondition(JsonIgnoreCondition.WhenWritingNull)]
        public bool? Status { get; set; }
        public string? Description { get; set; }
    }
}
