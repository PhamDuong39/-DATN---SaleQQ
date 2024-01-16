using System.ComponentModel.DataAnnotations;

namespace _DATN____SaleQQ.Dto.Request.AuthRequest
{
    public class ConfirmEmailRequest
    {
        [Required] public string UserNameOrEmail { get; set; }

        [Required] public string Token { get; set; }
    }
}
