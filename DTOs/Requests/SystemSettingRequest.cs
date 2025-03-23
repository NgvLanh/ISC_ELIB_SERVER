using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class SystemSettingRequest
    {
        [Required(ErrorMessage = "UserId không được để trống")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "ThemeId không được để trống")]
        public int ThemeId { get; set; }

        public bool? Captcha { get; set; }
    }
}
