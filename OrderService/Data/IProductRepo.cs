using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Models;

namespace OrderService.Data
{
    public interface IProductRepo
    {
        Task <IEnumerable<Product>> GetAllProducts();
        Task CreateProduct(Product product);
        bool ProductExits(int productId);
        bool ExternalProductExists(int externalProductId);
        bool SaveChanges();
    }
}