using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WalletService.AsyncDataServices;
using WalletService.Data;
using WalletService.Dtos;
using WalletService.Models;

namespace WalletService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletRepo _walletrepo;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public WalletController(IWalletRepo walletrepo,IMapper mapper,IMessageBusClient messageBusClient)
        {
            _walletrepo = walletrepo;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }
        [HttpPost]
        public async Task <ActionResult> Create(CreateWalletDto createWalletDto)
        {
            var wallet = _mapper.Map<Wallet>(createWalletDto);
            _walletrepo.Create(wallet);
            _walletrepo.SaveChanges();
            var readWalletDto = _mapper.Map<ReadWalletDto>(wallet);
            try
            {
                //send async message    
                var walletPublishedDto = _mapper.Map<WalletPublishedDto>(readWalletDto); 
                walletPublishedDto.Event = "Wallet_Published";
                _messageBusClient.PublishNewWallet(walletPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetByUsername),new
                {Username=readWalletDto.Username},readWalletDto);
        }

        [HttpGet("{username}")]
        public async Task <ActionResult> GetByUsername(string username)
        {
            var wallet = await _walletrepo.GetByUsername(username);
            var readWallet = _mapper.Map<ReadWalletDto>(wallet);
            return Ok(readWallet);
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<ReadWalletDto>>> GetAll()
        {
            var wallet = await _walletrepo.GetAll();
            var listWallet = _mapper.Map<IEnumerable<ReadWalletDto>>(wallet);
            return Ok(listWallet);
        }

        [HttpPut("TopUp/{username}/{amount}")]
        public async Task <ActionResult> TopUp(string username,decimal amount)
        {
            var wallet = await _walletrepo.TopUp(username,amount);
            var readWallet = _mapper.Map<ReadWalletDto>(wallet);
            _walletrepo.SaveChanges();
            try
            {
                //send async message    
                var walletPublishedDto = _mapper.Map<WalletPublishedDto>(readWallet); 
                walletPublishedDto.Event = "TopUp_Published";
                _messageBusClient.PublishNewWallet(walletPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }
            return Ok(readWallet);            
        }

        [HttpPut("Credit/{username}/{amount}")]
        public async Task <ActionResult> Credit(string username,decimal amount)
        {
            var wallet = await _walletrepo.Credit(username,amount);
            var readWallet = _mapper.Map<ReadWalletDto>(wallet);
            _walletrepo.SaveChanges();
            return Ok(readWallet);
        }
        
    }
}