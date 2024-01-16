using System.ComponentModel.DataAnnotations;

namespace _DATN____SaleQQ.Dto.Request.AuthRequest
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
