using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerceAPI.Data.Entities
{

    [Index(nameof(Product.Code), IsUnique = true)]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public required string Code { get; set; }

        public string? Name { get; set; }

        public string? Category { get; set; }

        public decimal? Price { get; set; }

        public int? MinimumQuantity { get; set; }

        public decimal? DiscountRate { get; set; }

        public string? ImagePath { get; set; }


    }
}
