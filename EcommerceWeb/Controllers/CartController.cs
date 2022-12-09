using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Service;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe.Checkout;
using System.Security.Claims;

namespace EcommerceWeb.Controllers
{
    [Authorize]
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly OrderProductService _orderProductService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _db;
        public CartController(ProductService productService, OrderService orderService,
            OrderProductService orderProductService, UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor, ApplicationDbContext db)
        {
            _productService = productService;
            _orderService = orderService;
            _orderProductService = orderProductService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            List<Item> items = new List<Item>();
            if (HttpContext.Session.GetString("cart") != null)
            {
                items = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
                ViewBag.total = items.Sum(i => i.Quantity * i.Product.Price);
            }
            return View(items);
        }

        [Route("buy/{id}")]
        public IActionResult Buy(int id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var product = _productService.FindOnlyProduct(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }
            if (HttpContext.Session.GetString("cart") == null)
            {
                var cart = new List<Item>();
                cart.Add(new Item
                {
                    Product = product,
                    Quantity = 1
                });
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            }
            else
            {
                var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
                var index = Exists(id, cart);
                if (index == -1)
                {
                    cart.Add(new Item
                    {
                        Product = product,
                        Quantity = 1
                    });
                }
                else
                {
                    cart[index].Quantity++;
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update(int[] quantity)
        {
            var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
            for (var i = 0; i < cart.Count; i++)
            {
                cart[i].Quantity = quantity[i];
            }
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction(nameof(Index));
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
            var index = Exists(id, cart);
            cart.RemoveAt(index);
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("index");
        }

        [Route("checkout")]
        public IActionResult Checkout()
        {
            if (HttpContext.Session.GetString("cart") != null)
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewBag.user = _db.ApplicationUsers.Find(userId);
                var items = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
                ViewBag.items = items;
                ViewBag.total = items.Sum(i => i.Quantity * i.Product.Price);
                return View();
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("checkout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(Order order)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("cart") != null)
                {
                    var items = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
                    var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var user = _db.ApplicationUsers.Find(userId);
                    //In this case default payment by Stripe
                    var orderToAdd = new Order
                    {
                        Address = order.Address,
                        Note = order.Note,
                        Created = DateTime.Now,
                        PaymentMethodId = 4,
                        OrderStatusId = 1,
                        ApplicationUser = user,
                        PaymentStatus = false,
                    };
                    if (_orderService.Create(orderToAdd))
                    {
                        foreach (var item in items)
                        {
                            var orderProduct = new OrderProduct
                            {
                                OrderId = orderToAdd.Id,
                                ProductId = item.Product.Id,
                                Quantity = item.Quantity,
                                Total = item.Quantity * item.Product.Price
                            };
                            _orderProductService.Create(orderProduct);
                        }


                        var domain = "https://localhost:7121/";
                        var options = new SessionCreateOptions
                        {
                            LineItems = new List<SessionLineItemOptions>(),
                            Mode = "payment",
                            SuccessUrl = domain + $"cart/OrderConfirmation?id={orderToAdd.Id}",
                            CancelUrl = domain + $"cart/index",
                        };
                        foreach (var item in items)
                        {
                            var sessionLineItem = new SessionLineItemOptions()
                            {
                                PriceData = new SessionLineItemPriceDataOptions
                                {
                                    UnitAmount = (long)(item.Product.Price * 100),
                                    Currency = "usd",
                                    ProductData = new SessionLineItemPriceDataProductDataOptions
                                    {
                                        Name = item.Product.Name,
                                    },

                                },
                                Quantity = item.Quantity,
                            };
                            options.LineItems.Add(sessionLineItem);
                        }

                        var service = new SessionService();
                        Session session = service.Create(options);
                        orderToAdd.SessionId = session.Id;
                        orderToAdd.PaymentIntentId = session.PaymentIntentId;
                        _orderService.Update(orderToAdd);

                        /*                        HttpContext.Session.Remove("cart");
                                                return RedirectToAction("Index", "Home");*/
                        Response.Headers.Add("Location", session.Url);
                        return new StatusCodeResult(303);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        [Route("OrderConfirmation")]
        public IActionResult OrderConfirmation(int id)
        {
            Order order = _orderService.FindById(id);
            if (!order.PaymentStatus)
            {
                //check the stripe status
                var service = new SessionService();
                Session session = service.Get(order.SessionId);
                //check the stripe status
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    order.PaymentStatus = true;
                    _orderService.Update(order);
                }
            }
            return View();
        }

        private int Exists(int id, List<Item> items)
        {
            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].Product.Id == id)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
