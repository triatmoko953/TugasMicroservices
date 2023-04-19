using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Models;

namespace WalletService.Data
{
    public interface IWalletRepo
    {
        Task<Wallet> GetByUsername(string username);
        Task <IEnumerable<Wallet>> GetAll();
        Task<Wallet> TopUp(string username, decimal amount);
        Task<Wallet> Credit(string username, decimal amount); 
        Task Create(Wallet wallet);
        bool SaveChanges();   
    }
}