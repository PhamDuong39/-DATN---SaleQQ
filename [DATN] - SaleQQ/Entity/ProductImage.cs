using System.ComponentModel.DataAnnotations;

namespace _DATN____SaleQQ.Entity
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? ImageProduct { get; set; }

        public int? ProductId { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public virtual Product? Product { get; set; }
    }
}
