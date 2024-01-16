using Microsoft.AspNetCore.Identity;

namespace _DATN____SaleQQ.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string? Avatar { get; set; }

        public int? Status { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
