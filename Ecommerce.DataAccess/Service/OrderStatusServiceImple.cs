using Ecommerce.DataAccess.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public class OrderStatusServiceImple : OrderStatusService
    {
        private readonly ApplicationDbContext _db;
        public OrderStatusServiceImple(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<OrderStatus> GetAllOrderStatuses()
        {
            return _db.OrderStatuses.Include(os => os.Orders)
                .ToList();
        }
    }
}
