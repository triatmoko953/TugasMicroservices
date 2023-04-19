using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductService.Dtos;

namespace ProductService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewProduct(ProductPublishedDto productPublishedDto);
    }
}