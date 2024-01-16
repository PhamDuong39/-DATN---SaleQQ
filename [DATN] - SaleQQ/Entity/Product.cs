using System.ComponentModel.DataAnnotations;

namespace _DATN____SaleQQ.Entity
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public int? ProductTypeId { get; set; }

        public string? NameProduct { get; set; }

        public double? Price { get; set; }

        public string? AvartarImageProduct { get; set; }

        public string? Title { get; set; }

        public int? Discount { get; set; }

        public int? Status { get; set; }

        public int? NumberOfViews { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdateAt { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public IEnumerable<CartItem> CartItems { get; set; } = new List<CartItem>();
        public IEnumerable<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
        public IEnumerable<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public virtual ProductType? ProductType { get; set; }
    }
}
