using Ecommerce.DataAccess.Service;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EcommerceWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductService _productService;
        private readonly SlideService _slideService;

        public HomeController(ILogger<HomeController> logger, ProductService productService,
            SlideService slideService)
        {
            _logger = logger;
            _productService = productService;
            _slideService = slideService;
        }

        public IActionResult Index()
        {
            ViewBag.NewestProducts = _productService.GetNewestProducts(6).ToList();
            ViewBag.RandomProducts = _productService.GetRandomProducts(8).ToList();
            ViewBag.Slides = _slideService.GetAllSlides();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}