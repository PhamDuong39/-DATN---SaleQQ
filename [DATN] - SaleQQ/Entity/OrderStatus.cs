using System.ComponentModel.DataAnnotations;

namespace _DATN____SaleQQ.Entity
{
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }

        public string? StatusName { get; set; }
        public virtual IEnumerable<Order> Orders { get; set; } = new List<Order>();
    }
}
