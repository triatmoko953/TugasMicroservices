using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Dtos;

namespace WalletService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewWallet(WalletPublishedDto walletPublishedDto);
        void PublishNewTopUp(TopUpPublishedDto topUpPublishedDto);
    }
}