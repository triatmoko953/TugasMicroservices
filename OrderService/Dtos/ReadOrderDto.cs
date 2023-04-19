using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class ReadOrderDto
    {
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    public int Qty { get; set; }
    
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public string Username { get; set; }
    }
}