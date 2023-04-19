using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class ReadTopUpDto
    {
        [Required]
        public string Username {get;set;}
        [Required]
        public decimal Cash {get;set;}
    }
}