using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Models;

namespace WalletService.Data
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
            if(!context.Wallets.Any())
            {
                Console.WriteLine("--> Seeding data");
                context.Wallets.AddRange(
                    new Wallet()
                {
                    Username = "John01",
                    FullName = "John William Smith",
                    Cash = 4000                  
                },
                     new Wallet()
                {
                    Username = "Sarah02",
                    FullName = "Sarah Elizabeth Johnson",
                    Cash = 5000                  
                },
                    new Wallet()
                {
                    Username = "Michael03",
                    FullName = "Michael Christopher Davis",
                    Cash = 8000                  
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