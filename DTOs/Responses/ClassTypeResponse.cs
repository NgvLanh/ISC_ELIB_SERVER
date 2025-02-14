using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ClassTypeResponse
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        [JsonIgnoreCondition(JsonIgnoreCondition.WhenWritingNull)]
        public bool? Status { get; set; }
        public string? Description { get; set; }

    }
}
