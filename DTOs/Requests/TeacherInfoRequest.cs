namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TeacherInfoRequest
    {
        public long Id { get; set; }
        public string? Cccd { get; set; }
        public DateTime? IssuedDate { get; set; }
        public string? IssuedPlace { get; set; }
        public bool? UnionMember { get; set; }
        public DateTime? UnionDate { get; set; }
        public string? UnionPlace { get; set; }
        public bool? PartyMember { get; set; }
        public DateTime? PartyDate { get; set; }
        public long? UserId { get; set; }
        public string? AddressFull { get; set; }
        public long ProvinceCode { get; set; }
        public long DistrictCode { get; set; }
        public long WardCode { get; set; }

    }
}
