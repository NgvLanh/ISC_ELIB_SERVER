using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class SubjectGroupRequest
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Id teacher không được để trống")]
        public int TeacherId { get; set; }
        [Required(ErrorMessage = "Mảng subjectId không được để trống")]
        public int[] subjectId { get; set; }
    }
}
