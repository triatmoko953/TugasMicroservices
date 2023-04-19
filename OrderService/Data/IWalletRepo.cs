using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Models;

namespace OrderService.Data
{
    public interface IWalletRepo
    {
        Task <IEnumerable<Wallet>> GetAllWallets();
        Task <Wallet> TopUp(string username, decimal amount);
        Task CreateWallet(Wallet wallet);
        bool WalletExits(string username);
        bool ExternalWalletExists(string externalUsername);
        bool SaveChanges(); 
    }
}