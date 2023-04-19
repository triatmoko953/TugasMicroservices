using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductService.Models;

namespace ProductService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app,bool v)
        {
            using (var serviceScope = app.ApplicationServices.CreateAsyncScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }
        public static void SeedData(AppDbContext context)
        {
            System.Console.WriteLine("Adding data - sedding...");
            if(!context.Products.Any())
            {
                Console.WriteLine("--> Seeding data");
                context.Products.AddRange(
                    new Product()
                {
                    Name = "Docker",
                    Description = "Container",
                    Price = 350,
                    Stock = 20                  
                },
                    new Product()
                {
                    Name = "Rabbitmq",
                    Description = "Message Broker",
                    Price = 250,
                    Stock = 10                  
                },
                    new Product()
                {
                    Name = "Kubernetes",
                    Description = "Orchestratior",
                    Price = 150,
                    Stock = 15                  
                });
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("-->Sudah ada data");
            }
        }
    }
}