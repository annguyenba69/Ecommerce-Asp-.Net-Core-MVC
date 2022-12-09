using Ecommerce.DataAccess.Service;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Editor")]
    [Area("admin")]
    [Route("admin/order")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly OrderStatusService _orderStatusService;
        public OrderController(OrderService orderService, OrderStatusService orderStatusService)
        {
            _orderService = orderService;
            _orderStatusService = orderStatusService;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index(string sortOrder, int? pageNumber, string searchString,
            string currentFilter, string orderStatusId)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["Status"] = orderStatusId;
            ViewData["CountTotal"] = _orderService.CountTotal();
            int pageSize = 6;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            IQueryable<Order> orders = _orderService.GetAllOrdersWithFilter(searchString, sortOrder, orderStatusId);
            ViewBag.OrderStatuses = _orderStatusService.GetAllOrderStatuses();
            return View(PaginatedList<Order>.Create(orders, pageNumber ?? 1, pageSize));
        }

        [Route("detail")]
        public IActionResult Detail(int id)
        {
            if (id == null)
            {
                TempData["error"] = "Order Not Found";
                return RedirectToAction(nameof(Index));
            }
            var order = _orderService.FindById(id);
            if (order == null)
            {
                TempData["error"] = "Order Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.OrderStatuses = _orderStatusService.GetAllOrderStatuses();
            return View(order);
        }

        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (Order order)
        {
            if (ModelState.IsValid)
            {
                if (_orderService.Update(order))
                {
                    TempData["success"] = "Update Order Successfull";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Update Order Fail";
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.OrderStatuses = _orderStatusService.GetAllOrderStatuses();
            var orderAfterValidate = _orderService.FindById(order.Id);
            return View("Detail", orderAfterValidate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                TempData["error"] = "Order Not Found";
                return RedirectToAction(nameof(Index));
            }
            var order = _orderService.FindById(id);
            if (order == null)
            {
                TempData["error"] = "Order Not Found";
                return RedirectToAction(nameof(Index));
            }
            if (_orderService.Remove(order))
            {
                TempData["success"] = "Delete Order Successfull";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Delete Order Fail";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
