namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class Exemption_UpdateRequest
    {
        public long StudentId { get; set; }
        public long ClassId { get; set; }
        public string? ExemptedObjects { get; set; }
        public string? FormExemption { get; set; }
        public bool? IsActive { get; set; }

    }
}
