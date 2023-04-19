using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data
{
    public class WalletRepo : IWalletRepo
    {
        private readonly AppDbContext _context;

        public WalletRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateWallet(Wallet wallet)
        {
            if (wallet == null)
            {
                throw new ArgumentNullException(nameof(wallet));
            }
            await _context.Wallets.AddAsync(wallet);
        }

        public bool ExternalWalletExists(string externalUsername)
        {
            return _context.Wallets.Any(w => w.Username == externalUsername);
        }

        public async Task<IEnumerable<Wallet>> GetAllWallets()
        {
            return await _context.Wallets.ToListAsync();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool WalletExits(string username)
        {
            return _context.Wallets.Any(w => w.Username == username);
        }
         public async Task <Wallet> TopUp(string username, decimal Amount)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Username == username);
            if (wallet != null)
            {
                wallet.Cash += (Amount-=wallet.Cash);
                await _context.SaveChangesAsync();
                return wallet;
            }
            else
            {
                throw new ArgumentException("Username not found");
            }
        }
    }
}