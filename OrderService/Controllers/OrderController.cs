using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepo orderRepo,IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<ReadOrderDto>>> GetOrderAll()
        {
            var orders = await _orderRepo.GetOrderAll();
            var listOrders = _mapper.Map<IEnumerable<ReadOrderDto>>(orders);
            return Ok(listOrders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderRepo.GetOrderById(id);
            var readOrderDto = _mapper.Map<ReadOrderDto>(order);
            return Ok(readOrderDto);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto createOrderDto)
        {
            var order = _mapper.Map<Order>(createOrderDto);
            await _orderRepo.CreateOrder(order);
            _orderRepo.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
        }
    }
}