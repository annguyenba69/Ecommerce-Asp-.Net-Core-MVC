using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public interface OrderStatusService
    {
        public List<OrderStatus> GetAllOrderStatuses();
    }
}
