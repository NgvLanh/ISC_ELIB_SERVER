namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class RegisterResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
