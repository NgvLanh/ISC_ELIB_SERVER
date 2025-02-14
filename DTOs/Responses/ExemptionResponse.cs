namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ExemptionResponse
    {
        public long Id { get; set; }
        public long StudentId { get; set; }
        public long ClassId { get; set; }
        public string? ExemptedObjects { get; set; }
        public string? FormExemption { get; set; }
        public bool? IsActive { get; set; }

    }
}
