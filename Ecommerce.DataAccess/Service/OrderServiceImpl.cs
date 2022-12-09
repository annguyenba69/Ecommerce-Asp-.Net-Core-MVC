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
    public class OrderServiceImpl : OrderService
    {
        private readonly ApplicationDbContext _db;
        public OrderServiceImpl(ApplicationDbContext db)
        {
            _db= db;    
        }

        public int CountByStatus(int orderStatusId)
        {
            return _db.Orders.Where(o => o.OrderStatusId== orderStatusId).Count();
        }

        public int CountTotal()
        {
            return _db.Orders.Count();
        }

        public bool Create(Order order)
        {
            try
            {
                _db.Orders.Add(order);
                return _db.SaveChanges() > 0;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public Order FindById(int id)
        {
            return _db.Orders.Include(o => o.OrderStatus)
                .Include(o => o.PaymentMethod)
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .FirstOrDefault(o => o.Id == id);
        }

        public IQueryable<Order> GetAllOrders()
        {
            return _db.Orders.Include(o => o.OrderStatus)
                .Include(o => o.PaymentMethod)
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderProducts);
        }

        public IQueryable<Order> GetAllOrdersWithFilter(string keyword, string sortOrder, string orderStatusId)
        {
            IQueryable<Order> orders = GetAllOrdersWithSearch(keyword);
            if (!string.IsNullOrEmpty(orderStatusId))
            {
                int parseOrderStatusId = Int32.Parse(orderStatusId);
                orders = orders.Where(o => o.OrderStatusId == parseOrderStatusId);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderByDescending(o => o.ApplicationUser.Fullname);
                    break;
                case "Date":
                    orders = orders.OrderBy(o => o.Created);
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(o => o.Created);
                    break;
                default:
                    orders = orders.OrderBy(o => o.ApplicationUser.Fullname);
                    break;
            }
            return orders;
        }

        public IQueryable<Order> GetAllOrdersWithSearch(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return GetAllOrders();
            }
            else
            {
                return GetAllOrders().Where(o => o.ApplicationUser.Fullname.Contains(keyword));
            }
        }

        public bool Remove(Order order)
        {
            try
            {
                _db.Orders.Remove(order);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Order order)
        {
            try
            {
                _db.Orders.Update(order);
                return _db.SaveChanges() > 0;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool UpdateStripePaymentID(int id, string sessionId, string PaymentIntentId)
        {
            try
            {
                var orderFromDb = FindById(id);
                orderFromDb.SessionId = sessionId;
                orderFromDb.PaymentIntentId = PaymentIntentId;
/*                _db.Orders.Update(order);*/
                return _db.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
