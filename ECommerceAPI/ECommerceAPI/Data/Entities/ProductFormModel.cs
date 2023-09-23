using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerceAPI.Data.Entities
{
    public class ProductFormModel
    {
        [Key]
        public required string Code { get; set; }

        public string? Name { get; set; }

        public string? Category { get; set; }

        public decimal? Price { get; set; }

        public int? MinimumQuantity { get; set; }

        public decimal? DiscountRate { get; set; }


        [Required]
        public required IFormFile ImageFile { get; set; }


    }
}
