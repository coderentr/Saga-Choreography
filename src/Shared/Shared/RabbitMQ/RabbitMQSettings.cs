using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RabbitMQ
{
    public static class RabbitMQSettings
    {
        public const string Stock_OrderCreatedEventQueue = "stock-order-created-queue";
        public const string Payment_StockReservedEventQueue = "payment-stock-reserved-queue";
        
    }
}
