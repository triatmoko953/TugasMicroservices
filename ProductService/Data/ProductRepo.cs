using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Data
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;

        public ProductRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task Create(Product product)
        {    
            if(product==null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Products.Add(product);          
        }

        public async Task Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product==null)
            {
                throw new Exception("Product not found");
            }
            _context.Products.Remove(product);
            
        }
        public async Task<IEnumerable<Product>> GetAll()
        {
            return  await _context.Products.ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product==null)
            {
                throw new Exception("Product not found");
            }
            return product;
        }

        public async Task<Product> GetByName(string name)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
            if (product==null)
            {
                throw new Exception("Product not found");
            }
            return product;
        }

        public async Task Update(int id, Product product)
        {
            try
            {
                var existingProduct = await GetById(product.ProductId);
            
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception ($"Error when updating product : {ex.Message}");
            }
        }
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}