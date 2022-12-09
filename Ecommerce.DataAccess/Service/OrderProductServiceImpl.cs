using Ecommerce.DataAccess.Data;
using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public class OrderProductServiceImpl : OrderProductService
    {
        private readonly ApplicationDbContext _db;
        public OrderProductServiceImpl(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(OrderProduct orderProduct)
        {
            try
            {
                _db.OrderProducts.Add(orderProduct);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
