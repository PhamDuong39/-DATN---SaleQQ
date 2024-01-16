using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace _DATN____SaleQQ.Entity
{
    public class ProductReview
    {
        [Key]
        public int Id { get; set; }

        public int? ProductId { get; set; }

        public string? UserId { get; set; }


        public string? ContentRated { get; set; }

        public int? PointEvaluation { get; set; }

        public string? ContentSeen { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual Product? Product { get; set; }
        public virtual IdentityUser? User { get; set; }
    }
}
