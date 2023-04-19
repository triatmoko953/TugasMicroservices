using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using OrderService.Data;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.EventProccessing
{
    public class EventProccessor : IEventProccessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProccessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _scopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }
        public void ProccessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.ProductPublished:
                    addProduct(message);
                    break;
                case EventType.WalletPublished:
                    addWallet(message);
                    break;
                case EventType.TopupWalletPublished:
                    topupWallet(message);
                    break;
                default:
                    break;
            }
        }
        private EventType DetermineEvent(string notificationMessage)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch (eventType.Event)
            {
                case "Product_Published":
                    Console.WriteLine("--> Product_Published Event Detected");
                    return EventType.ProductPublished;
                case "Wallet_Published":
                    Console.WriteLine("--> Wallet_Published Event Detected");
                    return EventType.WalletPublished;
                case "TopUp_Published":
                    Console.WriteLine("--> TopupWallet_NewPublished Event Detected");
                    return EventType.TopupWalletPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }
        private void addProduct(string productPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();
                var productPublishedDto = JsonSerializer.Deserialize<ProductPublishedDto>(productPublishedMessage);
                try
                {
                    var product = _mapper.Map<Product>(productPublishedDto);
                    if (!repo.ExternalProductExists(product.ProductId))
                    {
                        repo.CreateProduct(product);
                        repo.SaveChanges();
                        Console.WriteLine($"--> Product {product.Name} added");
                    }
                    else
                    {
                        Console.WriteLine("--> Product already exists");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Product to DB: {ex.Message}");
                }
            }
        }
         private void addWallet(string walletPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IWalletRepo>();
                var walletPublishedDto = JsonSerializer.Deserialize<WalletPublishedDto>(walletPublishedMessage);
                try
                {
                    var wallet = _mapper.Map<Wallet>(walletPublishedDto);
                    if (!repo.ExternalWalletExists(wallet.Username))
                    {
                        repo.CreateWallet(wallet);
                        repo.SaveChanges();
                        Console.WriteLine($"--> Wallet {wallet.Username} added");
                    }
                    else
                    {
                        Console.WriteLine("--> Wallet already exists");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Wallet to DB: {ex.Message}");
                }
            }
        }
         private void topupWallet(string topupWalletMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IWalletRepo>();
                var topUpPublishedDto = JsonSerializer.Deserialize<TopUpPublishedDto>(topupWalletMessage);
                try
                {
                    var ReadTopUpDto = _mapper.Map<ReadTopUpDto>(topUpPublishedDto);
                    if (repo.ExternalWalletExists(ReadTopUpDto.Username))
                    {
                        repo.TopUp(ReadTopUpDto.Username,ReadTopUpDto.Cash);
                        repo.SaveChanges();
                        Console.WriteLine($"--> TopTup {ReadTopUpDto.Username},{ReadTopUpDto.Cash} added");
                    }
                    else
                    {
                        Console.WriteLine("--> Username not found");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add TopUp to DB: {ex.Message}");
                }

            }
        }
    }
    enum EventType
    {
        ProductPublished,
        WalletPublished,
        TopupWalletPublished,
        Undetermined
    }
}