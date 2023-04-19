using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.EventProccessing
{
    public interface IEventProccessor
    {
        void ProccessEvent(string message);
    }
}