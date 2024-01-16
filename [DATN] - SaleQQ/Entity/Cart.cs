using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace _DATN____SaleQQ.Entity
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdateAt { get; set; }
        public virtual IdentityUser? User { get; set; }
        public virtual IEnumerable<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
