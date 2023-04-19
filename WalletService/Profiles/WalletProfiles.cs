using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WalletService.Dtos;
using WalletService.Models;

namespace WalletService.Profiles
{
    public class WalletProfiles : Profile
    {
        public WalletProfiles()
        {
            CreateMap<Wallet,ReadWalletDto>(); 
            CreateMap<decimal, ReadWalletDto>()
            .ConvertUsing(value => new ReadWalletDto { Cash = value });
            CreateMap<ReadWalletDto,WalletPublishedDto>(); 
            CreateMap<CreateWalletDto,Wallet>(); 
            CreateMap<ReadWalletDto,TopUpPublishedDto>(); 
            var config = new MapperConfiguration(cfg =>
            {
            cfg.CreateMap<Wallet, ReadTopUpDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Cash));
            });
            var mapper = config.CreateMapper();
        }
    }
}