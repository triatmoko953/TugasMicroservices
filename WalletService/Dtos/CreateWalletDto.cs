using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletService.Dtos
{
    public class CreateWalletDto
    {
        public string Username {get;set;}
        public string FullName {get;set;}
        public decimal Cash {get;set;}
    }
}