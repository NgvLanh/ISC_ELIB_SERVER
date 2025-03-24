namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ReserveListResponse
    {
        public int StudentId { get; set; }  // Student ID (Mã hồ sơ bảo lưu)
        public string? FullName { get; set; }  // Tên đầy đủ từ bảng User
        public DateTime? Dob { get; set; }  // Ngày sinh từ bảng User
        public string? Gender { get; set; }  // Giới tính từ bảng User
        public string? ClassName { get; set; } // Tên lớp từ bảng Class
        public DateTime? ReserveDate { get; set; }  // Ngày bảo lưu
        public string? RetentionPeriod { get; set; }  // Thời gian bảo lưu
        public string? Reason { get; set; }  // Lý do
    }
}
