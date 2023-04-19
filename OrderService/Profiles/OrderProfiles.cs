using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Profiles
{
    public class OrderProfiles : Profile
    {
        public OrderProfiles()
        {
            CreateMap<ProductPublishedDto, Product>();
            CreateMap<Product, ReadProductDto>();
            CreateMap<WalletPublishedDto, Wallet>();
            CreateMap<Wallet, ReadWalletDto>();
            CreateMap<CreateOrderDto, Order>();
            CreateMap<Order, ReadOrderDto>();
            CreateMap<TopUpPublishedDto, ReadTopUpDto>();
        }
    }
}