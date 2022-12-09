using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public interface OrderService
    {
        public IQueryable<Order> GetAllOrders();
        public IQueryable<Order> GetAllOrdersWithSearch(string keyword);
        public IQueryable<Order> GetAllOrdersWithFilter(string keyword, string sortOrder, string status);
        public bool Create(Order order);
        public bool Update(Order order);
        public bool Remove(Order order);
        public int CountTotal();
        public int CountByStatus(int orderStatusId);
        public Order FindById(int id);
        public bool UpdateStripePaymentID(int id, string sessionId, string PaymentIntentId);
    }
}
