using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WalletService.Models;

namespace WalletService.Data
{
    public class WalletRepo : IWalletRepo
    {
        private readonly AppDbContext _context;

        public WalletRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task Create(Wallet wallet)
        {
            if(wallet==null)
            {
                throw new ArgumentNullException(nameof(wallet));
            }
            _context.Wallets.Add(wallet);
        }

        public async Task<Wallet> Credit(string username, decimal amount)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Username == username);
            if (wallet != null)
            {
                wallet.Cash -= amount;
                await _context.SaveChangesAsync();
                return wallet;
            }
            else
            {
                throw new ArgumentException("Username not found");
            }
        }

        public async Task<IEnumerable<Wallet>> GetAll()
        {
            return  await _context.Wallets.ToListAsync();
        }

        public async Task<Wallet> GetByUsername(string username)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Username == username);
            if (wallet != null)
            {
                return wallet;
            }
            else
            {
                throw new ArgumentException("Username not found");
            }
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public async Task<Wallet> TopUp(string username, decimal amount)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Username == username);
            if (wallet != null)
            {
                wallet.Cash += amount;
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