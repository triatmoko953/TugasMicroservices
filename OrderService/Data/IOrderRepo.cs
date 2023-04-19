using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Data
{
    public interface IOrderRepo
    {
        Task<Order> CreateOrder(Order order);
        Task<IEnumerable<Order>> GetOrderAll();
        Task<Order> GetOrderById(int orderId);
        bool SaveChanges();

    }
}