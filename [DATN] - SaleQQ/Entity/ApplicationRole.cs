using Microsoft.AspNetCore.Identity;

namespace _DATN____SaleQQ.Entity
{
    public class ApplicationRole : IdentityRole
    {
        public string? AuthorityName { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdateAt { get; set; }
    }
}
