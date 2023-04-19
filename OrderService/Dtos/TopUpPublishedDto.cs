using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class TopUpPublishedDto
    {
        [Required]
        public string Username {get;set;}
        [Required]
        public decimal Cash {get;set;}
        [Required]
        public string Event { get; set; } = string.Empty;
        
    }
}