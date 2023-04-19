using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Dtos;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletRepo _walletRepo;
        private readonly IMapper _mapper;

        public WalletController(IWalletRepo walletRepo, IMapper mapper)
        {
            _walletRepo = walletRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWallets()
        {
            var wallets = await _walletRepo.GetAllWallets();
            var walletReadDtoList = _mapper.Map<IEnumerable<ReadWalletDto>>(wallets);
            return Ok(walletReadDtoList);
        }
        
    }
}