using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/dashboardrest")]
    public class DashboardRestController : Controller
    {
        /*[Produces("application/json")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = new List<Product>
                {
                    new Product { Id = 1, Name = "Product 1", Price = 100, Quantity = 20 },
                    new Product { Id = "p02", Name = "Product 2", Price = 120, Quantity = 12 },
                    new Product { Id = "p03", Name = "Product 3", Price = 80, Quantity = 60 },
                    new Product { Id = "p04", Name = "Product 4", Price = 290, Quantity = 34 },
                    new Product { Id = "p05", Name = "Product 5", Price = 200, Quantity = 29 }
                };
                return Ok(products);
            }
            catch
            {
                return BadRequest();
            }
        }*/
    }
}
