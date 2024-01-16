using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace _DATN____SaleQQ.Entity
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int? PaymentId { get; set; }

        public string? UserId { get; set; }

        public double? OriginalPrice { get; set; }

        public double? ActualPrice { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public int? OrderStatusId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdateAt { get; set; }
        public virtual IdentityUser? User { get; set; }
        public virtual Payment? Payment { get; set; }
        public virtual OrderStatus? OrderStatus { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
