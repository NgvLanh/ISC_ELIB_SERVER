namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class Exemption_UpdateRequest
    {
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public string? ExemptedObjects { get; set; }
        public string? FormExemption { get; set; }
        public bool Active { get; set; }

    }
}
