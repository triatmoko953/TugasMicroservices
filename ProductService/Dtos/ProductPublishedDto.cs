using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Dtos
{
    public class ProductPublishedDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Event { get; set; } = string.Empty;
    }
}