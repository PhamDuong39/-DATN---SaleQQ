using System.ComponentModel.DataAnnotations;

namespace _DATN____SaleQQ.Entity
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public string? PaymentMethod { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdateAt { get; set; }
        public virtual IEnumerable<Order> Orders { get; set; } = new List<Order>();
    }
}
