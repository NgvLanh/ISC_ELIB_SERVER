using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TrainingProgramsResponse
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        //public string? Description { get; set; }
        public long MajorId { get; set; }
        public long schoolFacilitiesID { get; set; }

        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly? StartDate { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly? EndDate { get; set; } 
        public string? Degree { get; set; }
        public string? TrainingForm { get; set; } 
        //public bool? Active { get; set; } 
        public string? FilePath { get; set; }
        public string? FileName{ get; set; }
    }
}
