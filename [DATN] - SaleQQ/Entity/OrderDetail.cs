using System.ComponentModel.DataAnnotations;

namespace _DATN____SaleQQ.Entity
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        public double? PriceTotal { get; set; }

        public int? Quantity { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdateAt { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }

    }
}
